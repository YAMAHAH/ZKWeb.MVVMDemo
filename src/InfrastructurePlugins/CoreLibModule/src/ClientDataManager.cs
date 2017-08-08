using InfrastructurePlugins.BaseModule.Components.Global;
using InfrastructurePlugins.SessionStateModule.Domain.Services;
using System.Collections.Concurrent;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace CoreLibModule
{
    [ExportMany, SingletonReuse]
    public class ClientDataManager : IClientDataManager
    {
        private ConcurrentDictionary<string, ClientData> clientDatas = new ConcurrentDictionary<string, ClientData>();

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
            ClientData cdValue;
            if (clientDatas.ContainsKey(key)) clientDatas.TryRemove(key, out cdValue);
        }
        public bool HasKey(string key)
        {
            return clientDatas.ContainsKey(key);
        }
    }
}


