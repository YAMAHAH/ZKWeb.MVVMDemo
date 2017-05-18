using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.RequestHandlers
{
    //[ExportMany, SingletonReuse]
    public class SecurityRequestHandlerWrapper : IHttpRequestHandlerWrapper
    {
        public Action WrapHandlerAction(Action action)
        {
            throw new NotImplementedException();
        }
    }
}
