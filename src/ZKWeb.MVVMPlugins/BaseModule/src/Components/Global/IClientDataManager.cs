using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructurePlugins.BaseModule.Components.Global
{
    public interface IClientDataManager
    {
        ClientData GetData(string key);
        void SetData(string key, ClientData value);
        void RemoveData(string key);
        bool HasKey(string key);
    }
}
