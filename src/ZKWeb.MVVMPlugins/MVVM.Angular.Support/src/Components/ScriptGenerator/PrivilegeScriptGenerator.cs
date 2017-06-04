using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeTranslators.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator
{
    /// <summary>
    /// 权限的脚本生成器
    /// </summary>
    [ExportMany]
    public class PrivilegeScriptGenerator
    {
        /// <summary>
        /// 生成用户类型列表的脚本
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateUserTypesScript()
        {
            // 获取系统中注册的所有用户类型和实现的接口类型
            var userTypes = ZKWeb.Application.Ioc.ResolveMany<IUserType>();
            var allTypes = new HashSet<Type>();
            foreach (var userType in userTypes)
            {
                var userTypeType = userType.GetType();
                foreach (var interfaceType in userTypeType.FastGetInterfaces())
                {
                    allTypes.Add(interfaceType);
                }
                while (userTypeType != null && userTypeType != typeof(object))
                {
                    allTypes.Add(userTypeType);
                    userTypeType = userTypeType.GetTypeInfo().BaseType;
                }
            }
            // 生成脚本代码
            var classBuilder = new StringBuilder();
            classBuilder.AppendLine($"export class UserTypes {{");
            foreach (var type in allTypes)
            {
                var typeName = type.Name;
                classBuilder.AppendLine($"    /** {new T(typeName)} */");
                classBuilder.AppendLine($"    public static {typeName} = {JsonConvert.SerializeObject(typeName)};");
            }
            classBuilder.AppendLine("}");
            return classBuilder.ToString();
        }

        /// <summary>
        /// 生成权限列表的脚本
        /// </summary>
        /// <returns></returns>
        public virtual string GeneratePrivilegesScript()
        {
            // 获取系统中注册的所有权限
            var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
            var privilegeProviders = ZKWeb.Application.Ioc.ResolveMany<IPrivilegesProvider>();
            var privilegeTranslator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
            var allPrivileges = privilegeProviders.SelectMany(p => p.GetPrivileges()).Distinct();
            // 生成脚本代码
            var classBuilder = new StringBuilder();
            classBuilder.AppendLine($"export class Privileges {{");
            foreach (var privilege in allPrivileges)
            {
                var name = privilegeTranslator.Translate(privilege);
                var variableName = pathConfig.NormalizeVariableName(privilege);
                classBuilder.AppendLine($"    /** {name} */");
                classBuilder.AppendLine($"    public static {variableName} = {JsonConvert.SerializeObject(privilege)};");
            }
            classBuilder.AppendLine("}");
            return classBuilder.ToString();
        }
        private const string classTemplate = @"
export class {{templateName}} {

    /** 属性 */
{{props}}
    /** 数据字段 */
    dataFields = {
{{dataFields}}
    };

    /** 功能 */
    actions = {
{{actions}}
    };

    /** 过滤器 */
    filters = {
{{filters}}
    };
}";
        private const string propTemplate = "    {{propName}} = {{propValue}}; \n";
        private const string actionTemplate = "{{actionName}}: {\n" + @"            enable: {{enableValue}}, text: {{TextValue}}, default: {{defaultValue}} " + "\n        }";

        private const string dataFieldTemp = "{{dataFieldName}}: {\n" + @"            name: {{nameValue}}, queryable: {{queryableValue}}, required: {{requiredValue}}, visible: {{visibleValue}}, editable: {{editableValue}}, text: {{textValue}},
            default: {{defaultValue}}, dataType: {{dataTypeValue}}, componentType: {{compTypeValue}}" + "\n        }";

        /// <summary>
        /// 生成模板对象字典
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GenerateComponentObjects()
        {
            var injector = ZKWeb.Application.Ioc;
            var tempDict = new Dictionary<string, string>();
            //获取模板对象
            var templateProviders = injector.ResolveMany<IPrivilegesProvider>();
            var templateTranslator = injector.Resolve<IPrivilegeTranslator>();

            var strBuilder = new StringBuilder();
            foreach (var templateProvider in templateProviders)
            {
                var temp = templateProvider.GetModuleComponentClassInfos();
                foreach (var tpl in temp)
                {
                    //模板名称
                    var tempName = tpl.TempClassType.Name.Replace("Service", "Template");
                    //基本属性处理
                    var tempProps = propTemplate.Replace("{{propName}}", "TemplateId")
                        .Replace("{{propValue}}", tpl.TempId.AutoDoubleQuotes());

                    tempProps += propTemplate.Replace("{{propName}}", "TempName")
                        .Replace("{{propValue}}", tpl.TempName.AutoDoubleQuotes());

                    tempProps += propTemplate.Replace("{{propName}}", "ModuleId")
                            .Replace("{{propValue}}", tpl.ModuleId.AutoDoubleQuotes());

                    tempProps += propTemplate.Replace("{{propName}}", "ModuleName")
                            .Replace("{{propValue}}", tpl.ModuleName.AutoDoubleQuotes());
                    //功能处理
                    strBuilder.Clear();
                    var tempActions = tpl.TempActions;
                    foreach (var action in tempActions)
                    {
                        var act = actionTemplate.Replace("{{actionName}}", action.Name.Trim())
                            .Replace("{{enableValue}}", action.Enable.ToString().ToLower())
                            .Replace("{{TextValue}}", action.Text.AutoDoubleQuotes())
                            .Replace("{{defaultValue}}", action.Default.ToString().ToLower());
                        strBuilder.Append("        " + act);
                        if (action != tempActions.Last())
                        {
                            strBuilder.AppendLine(",");
                        }
                    }
                    var actions = strBuilder.ToString();

                    //数据字段处理
                    strBuilder.Clear();
                    foreach (var dataField in tpl.TempDataFields)
                    {
                        var dataFieldStr = dataFieldTemp.Replace("{{dataFieldName}}", dataField.Alias)
                            .Replace("{{nameValue}}", dataField.Name.AutoDoubleQuotes())
                            .Replace("{{queryableValue}}", dataField.Queryable.ToString().ToLower())
                            .Replace("{{requiredValue}}", dataField.required.ToString().ToLower())
                            .Replace("{{visibleValue}}", dataField.Visible.ToString().ToLower())
                            .Replace("{{editableValue}}", dataField.Editable.ToString().ToLower())
                            .Replace("{{textValue}}", dataField.Text.AutoDoubleQuotes())
                            .Replace("{{defaultValue}}", dataField.Default.AutoDoubleQuotes())
                            .Replace("{{dataTypeValue}}", dataField.DataType.AutoDoubleQuotes())
                            .Replace("{{compTypeValue}}", dataField.ComponentType.AutoDoubleQuotes());
                        strBuilder.Append("        " + dataFieldStr);
                        if (dataField != tpl.TempDataFields.Last())
                        {
                            strBuilder.AppendLine(",");
                        }
                    }
                    var dataFields = strBuilder.ToString();

                    //过滤器处理
                    strBuilder.Clear();
                    foreach (var action in tpl.TempFilters)
                    {

                    }
                    var filters = strBuilder.ToString();

                    tempDict[tempName] = classTemplate.Replace("{{templateName}}", tempName)
                        .Replace("{{props}}", tempProps)
                        .Replace("{{dataFields}}", dataFields)
                        .Replace("{{actions}}", actions)
                        .Replace("{{filters}}", filters);
                }
            }
            return tempDict;
        }
    }
}
