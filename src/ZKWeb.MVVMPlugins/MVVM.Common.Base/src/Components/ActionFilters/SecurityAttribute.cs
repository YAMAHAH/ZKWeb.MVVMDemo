using Newtonsoft.Json;
using SimpleEasy.Core.lib;
using SimpleEasy.Core.lib.Utils;
using System;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes
{
    /// <summary>
    /// 控制应用服务如何使用加密，标记在应用服务的函数上
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DataSecurityAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 是否禁用加密，默认是false
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 开启加密
        /// </summary>
        public override Func<IActionResult> Filter(Func<IActionResult> action)
        {
            if (IsDisabled)
            {
                // 不使用加密
                return action;
            }
            return new Func<IActionResult>(() =>
            {
                var actionResult = action();
                var httpContext = HttpManager.CurrentContext;
                var seesionId = httpContext.Request.GetHeader(AppConts.SessionHeaderIn);
                var secretKey = ClientDataManager.GetData(seesionId)?.SecretKey ?? AppConts.DefaultSecretKey;
                dynamic content;
                if (actionResult is PlainResult)
                {
                    content = ((PlainResult)actionResult).Text;
                    string chiperText = AESUtils.EncryptToBase64String(secretKey, content);
                    var encryptObj = new { requestId = Guid.NewGuid().ToString(), data = chiperText, encrypt = true, signature = "" };
                    ((PlainResult)actionResult).Text = JsonConvert.SerializeObject(encryptObj);
                }
                else if (actionResult is JsonResult)
                {
                    content = ((JsonResult)actionResult).Object;
                    string contentString = JsonConvert.SerializeObject(content);
                    //加密
                    string chiperText = AESUtils.EncryptToBase64String(secretKey, contentString);
                    ((JsonResult)actionResult).Object = new { requestId = Guid.NewGuid().ToString(), data = chiperText, encrypt = true, signature = "" };
                }
                return actionResult;
            });
        }
    }
}
