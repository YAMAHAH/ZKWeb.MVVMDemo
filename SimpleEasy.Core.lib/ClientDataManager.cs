using System.Collections.Generic;

namespace SimpleEasy.Core.lib
{
    public class ClientDataManager
    {
        private static Dictionary<string, ClientData> clientDatas = new Dictionary<string, ClientData>();

        public static ClientData GetData(string key)
        {
            if (!clientDatas.ContainsKey(key))
            {
                //从会话中取
            }
            return clientDatas[key];
        }

        public static void SetData(string key, ClientData value)
        {
            //同时存储到会话中
            clientDatas[key] = value;
        }

        public static void RemoveData(string key)
        {
            if (clientDatas.ContainsKey(key)) clientDatas.Remove(key);
        }
        public static bool HasKey(string key)
        {
            return clientDatas.ContainsKey(key);
        }
    }
}

public class ClientData
{
    public string SecretKey { get; set; }
    public string PublickKey { get; set; }
}
