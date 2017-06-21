using Newtonsoft.Json;

namespace InfrastructurePlugins.BaseModule.Utils
{
    public static class JsonUtils
    {
        public static JsonSerializerSettings SelfLoopJsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        public static string SerializeObject(object serializeObject, JsonSerializerSettings jsonSerializerSetting = null)
        {
            if (jsonSerializerSetting == null)
            {
                return JsonConvert.SerializeObject(serializeObject, SelfLoopJsonSettings);
            }
            return JsonConvert.SerializeObject(serializeObject, jsonSerializerSetting);
        }
        public static string SerializeObject(object serializeObject)
        {
            return JsonConvert.SerializeObject(serializeObject);
        }
        public static T DeserializeObject<T>(string serializeString, JsonSerializerSettings jsonSerializerSetting = null)
        {
            if (jsonSerializerSetting == null)
            {
                return (T)JsonConvert.DeserializeObject(serializeString, typeof(T), SelfLoopJsonSettings);
            }
            return (T)JsonConvert.DeserializeObject(serializeString, typeof(T), SelfLoopJsonSettings);
        }
        public static T DeserializeObject<T>(string serializeString)
        {
            return (T)JsonConvert.DeserializeObject(serializeString, typeof(T));
        }
    }
}
