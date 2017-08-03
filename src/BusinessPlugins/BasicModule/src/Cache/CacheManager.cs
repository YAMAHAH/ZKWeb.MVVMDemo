using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Services;
using System;
using System.Collections.Generic;
using ZKWeb.Cache;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Cache
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class CacheManager : ICacheManager
    {

        private Guid PrivilegeUpdateId;

        private IKeyValueCache<string, Dictionary<Guid, TemplateObject>> xUserPrivCache;

        private List<KeyValuePair<Guid, Guid>> UserTempIds = new List<KeyValuePair<Guid, Guid>>();

        private IContainer Injector => ZKWeb.Application.Ioc;
        public CacheManager()
        {
            PrivilegeUpdateId = Guid.NewGuid();
            var cacheFactory = Injector.Resolve<ICacheFactory>();
            xUserPrivCache = cacheFactory.CreateCache<string, Dictionary<Guid, TemplateObject>>(new CacheFactoryOptions() { Lifetime = CacheLifetime.Singleton });
        }
        private String GetCacheKey(Guid userId, Guid tempId)
        {
            return userId.ToString() + "_" + PrivilegeUpdateId.ToString() + "_" + tempId.ToString();
        }

        public Dictionary<Guid, TemplateObject> GetUserTemplateDictionary(Guid userId, Guid tempId)
        {
            Dictionary<Guid, TemplateObject> tempObjDictCache = null;
            var cacheKey = GetCacheKey(userId, tempId);
            xUserPrivCache.TryGetValue(cacheKey, out tempObjDictCache);
            if (tempObjDictCache == null)
            {
                var empMan = Injector.Resolve<IEmployeeManager>();
                var tempObjects = empMan.GetTemplateObjectPrivileges(userId, tempId);
                if (tempObjects != null)
                {
                    xUserPrivCache.Put(cacheKey, tempObjects, TimeSpan.FromDays(1));
                    UserTempIds.Add(new KeyValuePair<Guid, Guid>(userId, tempId));
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

        public void UpdateCache()
        {
            var newUpdateId = Guid.NewGuid();
            //根据UserID,TempId获取最新的权限字典
            var empMan = Injector.Resolve<IEmployeeManager>();
            var userTempObjs = empMan.GetTemplateObjectPrivileges(UserTempIds);

            foreach (var dict in userTempObjs)
            {
                var newKey = dict.Key.Replace("_", "_" + newUpdateId.ToString() + "_");
                xUserPrivCache.Put(newKey, dict.Value, TimeSpan.FromDays(1));
            }
            //更新完成时重新生成ID
            PrivilegeUpdateId = newUpdateId;
        }
    }
}
