using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Cache
{
    /// <summary>
    /// 缓存KEY信息
    /// </summary>
    public struct CacheKey
    {
        public Guid UserId { get; set; }
        public Guid TempId { get; set; }
        public DateTime CacheTime { get; set; }
    }

    /// <summary>
    /// 缓存管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class CacheManager : ICacheManager
    {
        private string xPrivilegeUpdateIdKey = "PrivilegeUpdateId";
        private ConcurrentDictionary<string, Guid> xUpdateIdDict = new ConcurrentDictionary<string, Guid>();

        private IKeyValueCache<string, Dictionary<Guid, TemplateObject>> xUserPrivCache;

        private ConcurrentDictionary<string, CacheKey> xCacheKeyDict = new ConcurrentDictionary<string, CacheKey>();
        private IContainer Injector => ZKWeb.Application.Ioc;

        /// <summary>
        /// 缓存的时间
        /// </summary>
        public TimeSpan KeepTime { get; set; } = TimeSpan.FromDays(1);

        public Guid PrivilegeUpdateId
        {
            get
            {
                Guid privUpdateId;
                xUpdateIdDict.TryGetValue(xPrivilegeUpdateIdKey, out privUpdateId);
                return privUpdateId;
            }
        }

        private Guid AddOrUpdateId(string key, Guid value)
        {
            return xUpdateIdDict.AddOrUpdate(key, value, (k, v) => v = value);
        }

        public CacheManager()
        {
            var newGuid = Guid.NewGuid();
            AddOrUpdateId(xPrivilegeUpdateIdKey, newGuid);

            var cacheFactory = Injector.Resolve<ICacheFactory>();

            xUserPrivCache = cacheFactory.CreateCache<string, Dictionary<Guid, TemplateObject>>();
        }
        private string GetCacheKey(Guid userId, Guid tempId)
        {
            return string.Join("_", userId, PrivilegeUpdateId, tempId);
        }

        public Dictionary<Guid, TemplateObject> GetUserTemplateDictionary(Guid userId, Guid tempId)
        {
            Dictionary<Guid, TemplateObject> tempObjDictCache = null;

            var cacheKey = GetCacheKey(userId, tempId);

            if (!xUserPrivCache.TryGetValue(cacheKey, out tempObjDictCache))
            {
                var empMan = Injector.Resolve<IEmployeeManager>();

                var tempObjects = empMan.GetTemplateObjectPrivileges(userId, tempId);

                if (tempObjects != null)
                {
                    xUserPrivCache.Put(cacheKey, tempObjects, KeepTime);

                    var cacheKeyValue = new CacheKey() { UserId = userId, TempId = tempId, CacheTime = DateTime.UtcNow + KeepTime };

                    var localCacheKey = string.Join("_", userId, tempId);

                    xCacheKeyDict.AddOrUpdate(localCacheKey, cacheKeyValue, (key, val) => val = cacheKeyValue);

                    tempObjDictCache = tempObjects;
                }
            }
            return tempObjDictCache;
        }

        public TemplateObject GetUserTemplateObject(Guid userId, Guid tempId, Guid objectId)
        {
            TemplateObject tempObjCache = null;
            //从缓存中获取
            Dictionary<Guid, TemplateObject> tempObjDictCache = null;
            var cacheKey = GetCacheKey(userId, tempId);
            xUserPrivCache.TryGetValue(cacheKey, out tempObjDictCache);
            //获取失败,从数据库读取
            if (tempObjDictCache == null)
            {
                var tempObjDict = GetUserTemplateDictionary(userId, tempId);
                if (tempObjDict != null)
                {
                    tempObjDict.TryGetValue(objectId, out tempObjCache);
                }
            }
            return tempObjCache;
        }

        public void CleanCacheKey()
        {
            var now = DateTime.UtcNow;
            //清除过期的值
            var expireKeys = xCacheKeyDict.Where(c => c.Value.CacheTime < now).Select(c => c.Key).ToList();
            CacheKey cacheKeyValue;
            foreach (var key in expireKeys)
            {
                xCacheKeyDict.TryRemove(key, out cacheKeyValue);
            }
        }

        public void UpdateCache()
        {
            var newUpdateId = Guid.NewGuid();
            //清除过期缓存KEY
            CleanCacheKey();
            //根据UserID,TempId获取最新的权限字典
            var empMan = Injector.Resolve<IEmployeeManager>();

            var UserTempIds = xCacheKeyDict.Values.Select(c => new KeyValuePair<Guid, Guid>(c.UserId, c.TempId)).ToList();

            var userTempObjs = empMan.GetTemplateObjectPrivileges(UserTempIds);
            //如果相应的KEY获取不到,则删除相应的项
            var addKeys = new List<string>();
            foreach (var dict in userTempObjs)
            {
                addKeys.Add(dict.Key);
                var newKey = dict.Key.Replace("_", "_" + newUpdateId.ToString() + "_");
                //添加到缓存
                xUserPrivCache.Put(newKey, dict.Value, KeepTime);
            }
            //移除没有获取字典的项
            foreach (var kv in xCacheKeyDict.Keys.Where(k => !addKeys.Contains(k)))
            {
                CacheKey cacheKeyValue;
                xCacheKeyDict.TryRemove(kv, out cacheKeyValue);
            }
            //更新完成时重新生成ID
            AddOrUpdateId(xPrivilegeUpdateIdKey, newUpdateId);
        }
    }
}
