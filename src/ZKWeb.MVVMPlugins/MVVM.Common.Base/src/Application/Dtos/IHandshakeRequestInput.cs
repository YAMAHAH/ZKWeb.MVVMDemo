using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos
{
    public interface IHandshakeRequestInput
    {
        string SecretKey { get; set; }
        string PublicKey { get; set; }
    }
}
