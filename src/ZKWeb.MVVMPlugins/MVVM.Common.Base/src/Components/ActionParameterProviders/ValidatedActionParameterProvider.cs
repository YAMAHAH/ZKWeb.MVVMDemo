using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Extensions;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;
using Newtonsoft.Json;
using SimpleEasy.Core.lib;
using SimpleEasy.Core.lib.Utils;

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
            var x = HttpManager.CurrentContext.Request.Get<object>(name);
            var xx = HttpManager.CurrentContext.Request.GetJsonBodyDictionary();
            dynamic result;
            //如果启用加密，则取出
            if (xx.Count > 0)
            {
                var json = HttpManager.CurrentContext.Request.GetJsonBody();
                var encryptObject = JsonConvert.DeserializeObject<EncryptInput>(json);
                //typeof(IEncryptInput).GetTypeInfo().IsAssignableFrom(encryptObject.GetType())
                if (encryptObject != null)
                {
                    //解密
                    result = AESUtils.DecryptToModel<object>("99b3ad6e", encryptObject.data).Result;
                    //把解密的对象放入上下文中
                    HttpManager.CurrentContext.PutData<object>(name, (result as object));
                }
            }

            result = _originalProvider.GetParameter<T>(name, method, parameterInfo);

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
