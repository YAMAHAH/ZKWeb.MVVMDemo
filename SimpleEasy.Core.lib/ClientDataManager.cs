using System.Collections.Generic;

namespace SimpleEasy.Core.lib
{
    public class ClientDataManager
    {
        private static Dictionary<string, ClientData> clientDatas = new Dictionary<string, ClientData>();

        public static ClientData GetData(string key)
        {
            if (!clientDatas.ContainsKey(key)) return null;
            return clientDatas[key];
        }

        public static void SetData(string key,ClientData value)
        {
            clientDatas[key] = value;
        }

        public static void RemoveData(string key)
        {
            if (clientDatas.ContainsKey(key)) clientDatas.Remove(key);
        }
    }
}

public class ClientData
{
    public string SecretKey { get; set; }
    public string PublickKey { get; set; }
}
