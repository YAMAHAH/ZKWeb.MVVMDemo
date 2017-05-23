using Newtonsoft.Json;
using SimpleEasy.Core.lib;
using SimpleEasy.Core.lib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Global;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ActionParameterProviders
{
    /// <summary>
    /// 获取参数后自动进行检查
    /// </summary>
    public class ValidatedActionParameterProvider : IActionParameterProvider
    {
        private IActionParameterProvider _originalProvider;

        public ValidatedActionParameterProvider(IActionParameterProvider originalProvider)
        {
            _originalProvider = originalProvider;
        }

        public T GetParameter<T>(string name, MethodInfo method, ParameterInfo parameterInfo)
        {
            var httpContext = HttpManager.CurrentContext;
            dynamic result = default(T);
            //如果启用加密，则取出并缓存
            var bodyDict = httpContext.GetOrCreateData("bodyDict", () =>
            {
                var jsonBody = httpContext.Request.GetJsonBody();
                var encryptObj = JsonConvert.DeserializeObject<EncryptInput>(jsonBody);
                if (encryptObj != null && encryptObj is IEncryptInput && ((IEncryptInput)encryptObj).data != null)
                {
                    //从会话中取出客户端密钥 上下文->会话ID->密钥
                    //使用密钥解密
                    var sessionId = httpContext.Request.GetHeader(AppConsts.SessionHeaderIn);
                    if (sessionId == null) throw new BadRequestException("会话不充许为空");
                    IClientDataManager clientDataMan = ZKWeb.Application.Ioc.Resolve<IClientDataManager>();
                    var secretKey = clientDataMan.GetData(sessionId)?.SecretKey;

                    try
                    {
                        jsonBody = AESUtils.DecryptToUtf8String(secretKey, ((IEncryptInput)encryptObj).data).Result;
                    }
                    catch (Exception)
                    {
                        throw new BadRequestException("解密错误：前端与后端的加密密钥不一致.");
                    }
                    return JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonBody);
                }
                return new Dictionary<string, object>();
            });
            if (bodyDict.ContainsKey(name)) result = bodyDict[name].ConvertOrDefault<T>();

            if (result == null)
            {
                result = _originalProvider.GetParameter<T>(name, method, parameterInfo);
            }
            // 如果结果是IInputDto并且函数未标记不验证的属性则执行验证
            if (result is IInputDto &&
                method.GetCustomAttribute<NoParameterValidationAttribute>() == null)
            {
                ((IInputDto)result).Validate();
            }
            return result;
        }
    }
}
