using CoreLibModule.Utils;
using InfrastructurePlugins.BaseModule.ModuleCatalogs;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.ModuleCatalogs
{

    public abstract class ModuleCatalogBase : IModuleCatalog
    {
        /// <summary>
        /// 模块标识
        /// </summary>
        public Guid ModuleId { get; set; }

        public string ModuleCode { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 模块备注
        /// </summary>
        public string ModuleRemark { get; set; }

    }
    /// <summary>
    /// 基础管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class BasicModuleCatalog : ModuleCatalogBase
    {
        public BasicModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "BMS";
            ModuleName = GetType().Name;
            ModuleRemark = "基础管理系统";
        }

    }
    /// <summary>
    /// 组织管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class OrganizationModuleCatalog : ModuleCatalogBase
    {
        public OrganizationModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "OMS";
            ModuleName = GetType().Name;
            ModuleRemark = "组织管理系统";
        }

    }

    /// <summary>
    /// 销售管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SalesModuleCatalog : ModuleCatalogBase
    {
        public SalesModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "SMS";
            ModuleName = GetType().Name;
            ModuleRemark = "销售管理系统";
        }

    }

    /// <summary>
    /// 产品工程管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ProductEngineeringModuleCatalog : ModuleCatalogBase
    {
        public ProductEngineeringModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "PEMS";
            ModuleName = GetType().Name;
            ModuleRemark = "产品工程管理系统";
        }
    }
    /// <summary>
    /// 生产计划管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ProductionScheduleModuleCatalog : ModuleCatalogBase
    {
        public ProductionScheduleModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "PSMS";
            ModuleName = GetType().Name;
            ModuleRemark = "生产计划管理系统";
        }
    }
    /// <summary>
    /// 生产管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ProductionModuleCatalog : ModuleCatalogBase
    {
        public ProductionModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "MMS";
            ModuleName = GetType().Name;
            ModuleRemark = "生产管理系统";
        }
    }
    /// <summary>
    /// 采购管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PurchaseModuleCatalog : ModuleCatalogBase
    {
        public PurchaseModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "PMS";
            ModuleName = GetType().Name;
            ModuleRemark = "采购管理系统";
        }
    }

    /// <summary>
    /// 仓库管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class WarehouseModuleCatalog : ModuleCatalogBase
    {
        public WarehouseModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "WHMS";
            ModuleName = GetType().Name;
            ModuleRemark = "仓库管理系统";
        }
    }
    /// <summary>
    /// 仓库管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class FinanceModuleCatalog : ModuleCatalogBase
    {
        public FinanceModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "FIMS";
            ModuleName = GetType().Name;
            ModuleRemark = "财务管理系统";
        }
    }
    /// <summary>
    /// 质量管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class QualityControlModuleCatalog : ModuleCatalogBase
    {
        public QualityControlModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "QCMS";
            ModuleName = GetType().Name;
            ModuleRemark = "质控管理系统";
        }
    }

    /// <summary>
    /// 系统管理模块
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SystemAdminModuleCatalog : ModuleCatalogBase
    {
        public SystemAdminModuleCatalog()
        {
            ModuleId = MD5Utils.GetGuidByMD5(GetType().FullName, "X2");
            ModuleCode = "SAMS";
            ModuleName = GetType().Name;
            ModuleRemark = "系统管理系统";
        }
    }
}
