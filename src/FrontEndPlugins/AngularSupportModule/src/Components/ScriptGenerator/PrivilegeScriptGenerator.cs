using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Localize;
using InfrastructurePlugins.BaseModule.Components.Extensions;
using InfrastructurePlugins.BaseModule.Template;
using BusinessPlugins.OrganizationModule.Components.PrivilegeTranslators.Interfaces;
using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using ZKWeb.Plugins.OrganizationModule.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace FrontEndPlugins.AngularSupportModule.Components.ScriptGenerator
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
        private string propTemplate = " ".Repeat(4) + "{{propName}} = {{propValue}};";
        private string actionTemplate = " ".Repeat(8) + "{{actionName}}: {\n" +
                             " ".Repeat(12) + "enable: {{enableValue}}, text: {{TextValue}}, default: {{defaultValue}} \n" +
                            " ".Repeat(8) + "}";

        private string dataFieldTemp = " ".Repeat(8) + "{{0}}: {\n" +
                              " ".Repeat(12) + "name: {{1}}, queryable: {{2}}, required: {{3}}, visible: {{4}}, editable: {{5}}, text: {{6}},\n" +
                              " ".Repeat(12) + "default: {{7}}, dataType: {{8}}, componentType: {{9}}" + "\n" +
                              " ".Repeat(8) + "}";

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
                    StringTemplateBuilder propBuilder = new StringTemplateBuilder() { NeedJoinSymbol = false };
                    //模板名称
                    var tempName = tpl.TempClassType.Name.Replace("Service", "Template");
                    //基本属性处理
                    Dictionary<string, string> propValues = new Dictionary<string, string>()
                    {
                        {"propName","TemplateId" },
                        {"propValue",tpl.TempId.AutoDoubleQuotes() }
                    };
                    propBuilder.Clear();
                    propBuilder.AddGroupValues(propValues);

                    propValues = new Dictionary<string, string>()
                    {
                        {"propName","TempName" },
                        {"propValue",tpl.TempName.AutoDoubleQuotes() }
                    };
                    propBuilder.AddGroupValues(propValues);

                    propValues = new Dictionary<string, string>()
                    {
                        {"propName","ModuleId" },
                        {"propValue",tpl.ModuleId.AutoDoubleQuotes() }
                    };
                    propBuilder.AddGroupValues(propValues);

                    propValues = new Dictionary<string, string>()
                    {
                        {"propName","ModuleName" },
                        {"propValue",tpl.ModuleName.AutoDoubleQuotes() }
                    };
                    propBuilder.AddGroupValues(propValues);
                    var tempProps = propBuilder.TransformText(propTemplate);

                    //功能处理
                    strBuilder.Clear();
                    var tempActions = tpl.TempActions;
                    StringTemplateBuilder stringTemplate = new StringTemplateBuilder() { };
                    foreach (var action in tempActions)
                    {
                        Dictionary<string, string> ActionGroupValues = new Dictionary<string, string>()
                        {
                            { "actionName", action.Name.Trim() },
                            { "enableValue", action.Enable.ToString().ToLower() },
                            { "TextValue",  action.Text.AutoDoubleQuotes() },
                            { "defaultValue", action.Default.ToString().ToLower() }
                        };
                        stringTemplate.AddGroupValues(ActionGroupValues);
                    }
                    var actions = stringTemplate.TransformText(actionTemplate);

                    //数据字段处理                
                    stringTemplate.Clear();
                    foreach (var dataField in tpl.TempDataFields)
                    {
                        Dictionary<string, string> values2 = new Dictionary<string, string>()
                        {
                            { "0", dataField.Alias },
                            { "1", dataField.Name.AutoDoubleQuotes() },
                            { "2", dataField.Queryable.ToString().ToLower() },
                            { "3", dataField.required.ToString().ToLower() },
                            { "4", dataField.Visible.ToString().ToLower() },
                            { "5", dataField.Editable.ToString().ToLower() },
                            { "6", dataField.Text.AutoDoubleQuotes() },
                            { "7", dataField.Default.AutoDoubleQuotes()},
                            { "8", dataField.DataType.AutoDoubleQuotes()},
                            { "9", dataField.ComponentType.AutoDoubleQuotes()}
                        };
                        stringTemplate.AddGroupValues(values2);
                    }
                    var dataFields = stringTemplate.TransformText(dataFieldTemp);

                    //过滤器处理
                    strBuilder.Clear();
                    foreach (var action in tpl.TempFilters)
                    {

                    }
                    var filters = strBuilder.ToString();
                    StringTemplateBuilder strTemp = new StringTemplateBuilder() { Template = classTemplate };
                    Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "templateName", tempName },
                        { "props", tempProps },
                        { "dataFields", dataFields },
                        { "actions", actions },
                        { "filters", filters }
                    };
                    strTemp.AddGroupValues(values);
                    tempDict[tempName] = strTemp.TransformText();
                }
            }
            return tempDict;
        }
    }
}
