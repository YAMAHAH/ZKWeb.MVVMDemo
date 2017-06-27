using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructurePlugins.BaseModule.Application.Dtos
{
    public interface IHandshakeRequestInput
    {
        string SecretKey { get; set; }
        string PublicKey { get; set; }
    }
}
