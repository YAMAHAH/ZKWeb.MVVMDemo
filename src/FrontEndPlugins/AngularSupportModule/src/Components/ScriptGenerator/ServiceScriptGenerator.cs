﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace FrontEndPlugins.AngularSupportModule.Components.ScriptGenerator
{
    /// <summary>
    /// 应用服务的脚本生成器
    /// </summary>
    [ExportMany]
    public class ServiceScriptGenerator
    {
        /// <summary>
        /// 根据应用服务生成脚本
        /// </summary>
        public virtual string GenerateScript(IApplicationService service)
        {
            // 参考: https://angular.io/docs/ts/latest/guide/server-communication.html
            // 这里调用api的操作都封装到了AppApiService
            // 生成的代码中不分别处理返回结果和错误
            var dtoScriptGenerator = ZKWeb.Application.Ioc.Resolve<DtoScriptGenerator>();
            var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
            var includeBuilder = new StringBuilder();
            var classBuilder = new StringBuilder();
            var includedTypes = new HashSet<Type>();
            var className = pathConfig.NormalizeClassName(service.GetType().Name);
            var classDescription = service.GetType().GetTypeInfo()
                .GetCustomAttribute<DescriptionAttribute>()?.Description ?? className;
            includeBuilder.AppendLine("import { Injectable } from '@angular/core';");
            includeBuilder.AppendLine("import { Observable } from 'rxjs/Observable';");
            includeBuilder.AppendLine("import { AppApiService } from '@global_module/services/app-api-service';");
            includeBuilder.AppendLine("import { ApiCallExtra } from '@global_module/models/api-call-extra';");
            classBuilder.AppendLine("@Injectable()");
            classBuilder.AppendLine($"/** {classDescription} */");
            classBuilder.AppendLine($"export class {className} {{");
            classBuilder.AppendLine("    constructor(private appApiService: AppApiService) { }");
            classBuilder.AppendLine();
            var methods = service.GetApiMethods().ToList();
            var apiCallParamStr = "extra"; // "resultConverter, errorConverter";
            var actionParamStr = "extra?: ApiCallExtra"; //"resultConverter?: rxResultConverter, errorConverter?: rxErrorConverter";
            foreach (var method in methods)
            {
                // 获取方法信息
                var newDiscoveredTypes = new List<Type>();
                var methodName = method.Name;
                var methodDescription = method.Attributes
                    .OfType<DescriptionAttribute>()
                    .FirstOrDefault()?.Description ?? methodName;
                var methodReturnType = dtoScriptGenerator.GetScriptType(
                    method.ReturnType, newDiscoveredTypes);
                var methodParameters = method.Parameters
                    .Select(p => new
                    {
                        name = p.Name,
                        type = dtoScriptGenerator.GetScriptType(p.Type, newDiscoveredTypes)
                    }).ToList();
                var methodUrl = method.Url;
                // 必要时引用其他类型的脚本
                foreach (var newDiscoveredType in newDiscoveredTypes)
                {
                    if (!includedTypes.Contains(newDiscoveredType))
                    {
                        var importName = pathConfig.NormalizeClassName(newDiscoveredType);
                        var importFile = pathConfig.NormalizeFilename(importName);
                        includeBuilder.AppendLine($"import {{ {importName} }} from '@generated_module/dtos/{importFile}';");
                        includedTypes.Add(newDiscoveredType);
                    }
                }
                // 添加方法
                classBuilder.AppendLine($"    /** {methodDescription} */");
                classBuilder.Append($"    {methodName}(");
                foreach (var parameter in methodParameters)
                {
                    classBuilder.Append($"{parameter.name}: {parameter.type}");
                    // if (parameter != methodParameters.Last())
                    //  {
                    classBuilder.Append(", ");
                    //  }
                }
                classBuilder.AppendLine($"{actionParamStr}): Observable<{methodReturnType}> {{");
                //序列化参数
                string[] requireBodyMethods = { "post", "put", "patch" }; //head,options,get
                string requestMethod = method.Method.ToLower();

                string requestBody = string.Empty;
                string requestParams = string.Empty;

                requestBody = "{ ";
                requestParams = "[";
                foreach (var parameter in methodParameters)
                {
                    requestBody += parameter.name;
                    if (typeof(IInputDto).GetTypeInfo().IsAssignableFrom(parameter.GetType()))
                    {
                        requestParams += parameter.name;
                    }
                    else
                    {
                        requestParams += "{ " + parameter.name + " }";
                    }
                    if (parameter != methodParameters.Last())
                    {
                        requestBody += ",";
                        requestParams += ", ";
                    }
                }
                requestBody += " }";
                requestParams += "]";

                if (!requireBodyMethods.Contains(requestMethod))
                {
                    var queryString = "        let urlParams = this.appApiService.getRequestQueryString(" + requestParams + ");";
                    if (methodParameters.Count > 0) classBuilder.AppendLine(queryString);
                }
                classBuilder.AppendLine($"        return this.appApiService.call<{methodReturnType}>(");
                classBuilder.AppendLine($"            {JsonConvert.SerializeObject(methodUrl)},");

                //添加BODY
                //body:{ inputdtoA, inputdtoB } //按照约定只有一个输入参数，封装所有的信息

                //根据API的method,生成相应的请求选项
                // classBuilder.AppendLine($"            }},");

                if (requireBodyMethods.Contains(requestMethod) || methodParameters.Count == 0)
                {
                    if (methodParameters.Count > 0)
                    {
                        classBuilder.AppendLine("            {\n                method: \"" + $"{method.Method}\"," +
                                                             "\n                body: " + $"{requestBody}" +
                                                             "\n            }" +
                                                              $", {apiCallParamStr}" + ");");
                    }
                    else
                    {
                        classBuilder.AppendLine("            {\n                method: \"" + $"{method.Method}" + "\"\n            }"
                                                                                + $", {apiCallParamStr}" + ");");
                    }
                }
                else
                {
                    classBuilder.AppendLine("            {\n                method: \"" + $"{method.Method}\"," +
                                                         "\n                params: urlParams" +
                                           "\n            }" + $", {apiCallParamStr}" + ");");
                }

                classBuilder.AppendLine($"    }}");
                if (method != methods.Last())
                {
                    classBuilder.AppendLine();
                }
            }
            classBuilder.AppendLine("}");
            includeBuilder.AppendLine();
            return includeBuilder.ToString() + classBuilder.ToString();
        }
    }
}
