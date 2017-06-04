using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Template
{
    public class ComponentObjectTemplate : IAngularTemplate
    {
        //        private const string classTemplate = @"
        //export class {{templateName}} {

        //    /** 属性 */
        //{{props}}
        //    /** 数据字段 */
        //    dataFields = {
        //{{dataFields}}
        //    };

        //    /** 功能 */
        //    actions = {
        //{{actions}}
        //    };

        //    /** 过滤器 */
        //    filters = {
        //{{filters}}
        //    };
        //}";
        //        private const string propTemplate = "    {{propName}} = {{propValue}}; \n";
        //        private const string actionTemplate = "{{actionName}}: {\n" + @"            enable: {{enableValue}}, text: {{TextValue}}, default: {{defaultValue}} " + "\n        }";

        //        private const string dataFieldTemp = "{{dataFieldName}}: {\n" + @"            name: {{nameValue}}, queryable: {{queryableValue}}, required: {{requiredValue}}, visible: {{visibleValue}}, editable: {{editableValue}}, text: {{textValue}},
        //            default: {{defaultValue}}, dataType: {{dataTypeValue}}, componentType: {{compTypeValue}}" + "\n        }";

        //        /// <summary>
        //        /// 生成模板对象字典
        //        /// </summary>
        //        /// <returns></returns>
        //        public virtual Dictionary<string, string> GenerateTemplateObjects()
        //        {
        //            var injector = ZKWeb.Application.Ioc;
        //            var tempDict = new Dictionary<string, string>();
        //            //获取模板对象
        //            var templateProviders = injector.ResolveMany<IPrivilegesProvider>();
        //            var templateTranslator = injector.Resolve<IPrivilegeTranslator>();

        //            var strBuilder = new StringBuilder();
        //            foreach (var templateProvider in templateProviders)
        //            {
        //                var temp = templateProvider.GetModuleTemplateObjects();
        //                foreach (var tpl in temp)
        //                {
        //                    //模板名称
        //                    var tempName = tpl.TempClassType.Name.Replace("Service", "Template");
        //                    //基本属性处理
        //                    var tempProps = propTemplate.Replace("{{propName}}", "TemplateId")
        //                        .Replace("{{propValue}}", tpl.TempId.AutoDoubleQuotes());

        //                    tempProps += propTemplate.Replace("{{propName}}", "TempName")
        //                        .Replace("{{propValue}}", tpl.TempName.AutoDoubleQuotes());

        //                    tempProps += propTemplate.Replace("{{propName}}", "ModuleId")
        //                            .Replace("{{propValue}}", tpl.ModuleId.AutoDoubleQuotes());

        //                    tempProps += propTemplate.Replace("{{propName}}", "ModuleName")
        //                            .Replace("{{propValue}}", tpl.ModuleName.AutoDoubleQuotes());
        //                    //功能处理
        //                    strBuilder.Clear();
        //                    var tempActions = tpl.TempActions;
        //                    foreach (var action in tempActions)
        //                    {
        //                        var act = actionTemplate.Replace("{{actionName}}", action.Name.Trim())
        //                            .Replace("{{enableValue}}", action.Enable.ToString().ToLower())
        //                            .Replace("{{TextValue}}", action.Text.AutoDoubleQuotes())
        //                            .Replace("{{defaultValue}}", action.Default.ToString().ToLower());
        //                        strBuilder.Append("        " + act);
        //                        if (action != tempActions.Last())
        //                        {
        //                            strBuilder.AppendLine(",");
        //                        }
        //                    }
        //                    var actions = strBuilder.ToString();

        //                    //数据字段处理
        //                    strBuilder.Clear();
        //                    foreach (var dataField in tpl.TempDataFields)
        //                    {
        //                        var dataFieldStr = dataFieldTemp.Replace("{{dataFieldName}}", dataField.Alias)
        //                            .Replace("{{nameValue}}", dataField.Name.AutoDoubleQuotes())
        //                            .Replace("{{queryableValue}}", dataField.Queryable.ToString().ToLower())
        //                            .Replace("{{requiredValue}}", dataField.required.ToString().ToLower())
        //                            .Replace("{{visibleValue}}", dataField.Visible.ToString().ToLower())
        //                            .Replace("{{editableValue}}", dataField.Editable.ToString().ToLower())
        //                            .Replace("{{textValue}}", dataField.Text.AutoDoubleQuotes())
        //                            .Replace("{{defaultValue}}", dataField.Default.AutoDoubleQuotes())
        //                            .Replace("{{dataTypeValue}}", dataField.DataType.AutoDoubleQuotes())
        //                            .Replace("{{compTypeValue}}", dataField.ComponentType.AutoDoubleQuotes());
        //                        strBuilder.Append("        " + dataFieldStr);
        //                        if (dataField != tpl.TempDataFields.Last())
        //                        {
        //                            strBuilder.AppendLine(",");
        //                        }
        //                    }
        //                    var dataFields = strBuilder.ToString();

        //                    //过滤器处理
        //                    strBuilder.Clear();
        //                    foreach (var action in tpl.TempFilters)
        //                    {

        //                    }
        //                    var filters = strBuilder.ToString();

        //                    tempDict[tempName] = classTemplate.Replace("{{templateName}}", tempName)
        //                        .Replace("{{props}}", tempProps)
        //                        .Replace("{{dataFields}}", dataFields)
        //                        .Replace("{{actions}}", actions)
        //                        .Replace("{{filters}}", filters);
        //                }
        //            }
        //            return tempDict;
        //        }
    }
}
