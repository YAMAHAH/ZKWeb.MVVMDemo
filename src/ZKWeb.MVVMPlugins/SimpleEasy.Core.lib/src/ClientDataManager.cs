using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Global;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace SimpleEasy.Core.lib
{
    [ExportMany, SingletonReuse]
    public class ClientDataManager : IClientDataManager
    {
        private Dictionary<string, ClientData> clientDatas = new Dictionary<string, ClientData>();

        public ClientData GetData(string key)
        {
            if (!clientDatas.ContainsKey(key))
            {
                ////从会话中取
                var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
                var session = sessionManager.GetSession();
                ClientData clientData = session[AppConsts.ClientDataKey].ConvertOrDefault<ClientData>();
                if (clientData != null) SetData(session.Id.ToString(), clientData);
                return clientData;
            }
            return clientDatas[key];
        }

        public void SetData(string key, ClientData value)
        {
            //同时存储到会话中
            clientDatas[key] = value;
        }

        public void RemoveData(string key)
        {
            if (clientDatas.ContainsKey(key)) clientDatas.Remove(key);
        }
        public bool HasKey(string key)
        {
            return clientDatas.ContainsKey(key);
        }
    }
}


