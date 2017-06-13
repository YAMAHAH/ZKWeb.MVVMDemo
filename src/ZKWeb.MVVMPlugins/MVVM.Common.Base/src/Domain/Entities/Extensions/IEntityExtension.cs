using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Extensions
{
    public static class IEntityExtension
    {
        public static bool HaveCreateTime(this IEntity entity)
        {
            return typeof(IHaveCreateTime).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HaveUpdateTime(this IEntity entity)
        {
            return typeof(IHaveUpdateTime).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HaveDeleted(this IEntity entity)
        {
            return typeof(IHaveDeleted).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HaveChildren(this IEntity entity)
        {
            return typeof(IHaveChildren<>).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HaveNodeVersion(this IEntity entity)
        {
            return typeof(IHaveNodeVersion<>).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HaveRootVersion(this IEntity entity)
        {
            return typeof(IHaveRootVersion<>).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HaveTreeNode(this IEntity entity)
        {
            return typeof(IHaveTreeNode<>).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
        public static bool HavePrimaryKey(this IEntity entity)
        {
            return typeof(IEntity<>).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }
    }
}
