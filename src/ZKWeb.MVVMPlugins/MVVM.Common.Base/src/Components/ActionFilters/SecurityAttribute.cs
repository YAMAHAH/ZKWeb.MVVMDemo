using SimpleEasy.Core.lib.Utils;
using System;
using System.Data;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes
{
    /// <summary>
    /// 控制应用服务如何使用加密，标记在应用服务的函数上
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SecurityAttribute : ActionFilterAttribute
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
                dynamic content;
                if (actionResult is PlainResult)
                {
                    content = ((PlainResult)actionResult).Text;
                    string chiperText = AESUtils.EncryptToBase64String("99b3ad6e", content);
                    ((PlainResult)actionResult).Text = chiperText;
                }
                else if (actionResult is JsonResult)
                {
                    content = ((JsonResult)actionResult).Object;
                    string contentString = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                    //加密
                    string chiperText = AESUtils.EncryptToBase64String("99b3ad6e", contentString);
                    ((JsonResult)actionResult).Object = new { data = chiperText, encrypt = true };
                }
                return actionResult;
            });
        }
    }
}
