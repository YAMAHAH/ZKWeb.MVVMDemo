using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Global
{
    public interface IClientDataManager
    {
        ClientData GetData(string key);
        void SetData(string key, ClientData value);
        void RemoveData(string key);
        bool HasKey(string key);
    }
}
