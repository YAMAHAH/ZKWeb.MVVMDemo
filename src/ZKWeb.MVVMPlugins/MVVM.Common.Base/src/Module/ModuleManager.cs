using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module
{
    [ExportMany, SingletonReuse]
    public class ModuleManager
    {
        private IEnumerable<IApplicationService> xTemplates;

        public IEnumerable<IApplicationService> AllModuleTemplates
        {
            get
            {
                if (xTemplates == null)
                {
                    xTemplates = ZKWeb.Application.Ioc.ResolveMany<IApplicationService>();
                }
                return xTemplates;
            }
        }
        private Dictionary<Type, List<Type>> moduleTemplateDict;
        public ModuleManager()
        {
            moduleTemplateDict = new Dictionary<Type, List<Type>>();
        }

        public void RegisterModuleTemplate(Type moduleType, List<Type> templates)
        {
            var moduleTypes = moduleTemplateDict.GetOrCreate(moduleType, () => new List<Type>());
            var addTypes = templates.Where(t => !moduleTypes.Contains(t)).ToList();
            moduleTypes.AddRange(addTypes);
        }

        public void RegisterModuleTemplate(Type moduleType, Type template)
        {
            var moduleTypes = moduleTemplateDict.GetOrCreate(moduleType, () => new List<Type>());

            if (!moduleTypes.Contains(template))
            {
                moduleTypes.Add(template);
            }
        }

        public List<Type> GetModuleTemplates(Type moduleType)
        {
            return moduleTemplateDict.GetOrCreate(moduleType, () => new List<Type>());
        }
    }
}
