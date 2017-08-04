using BusinessPlugins.BasicModule.Domain.Entities;
using System;
using System.Collections.Generic;
using ZKWebStandard.Collections;

namespace BusinessPlugins.BasicModule.Cache
{
    public interface ICacheManager
    {
        /// <summary>
        /// 获取用户模板的单个对象
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tempId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        TemplateObject GetUserTemplateObject(Guid userId, Guid tempId, Guid objectId);
        /// <summary>
        /// 获取用户模板的对象字典
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tempId"></param>
        /// <returns></returns>
        Dictionary<Guid, TemplateObject> GetUserTemplateDictionary(Guid userId, Guid tempId);
        /// <summary>
        /// 更新缓存数据
        /// </summary>
        void UpdateCache();

        /// <summary>
        /// 清理过期的缓存信息
        /// </summary>
        void CleanCacheKey();
    }
}
