using Newtonsoft.Json;
namespace OpenBLive.Runtime.Data
{
    public struct LoginUrlData
    {
        [JsonProperty("oauthKey")]
        public string oauthKey;
        [JsonProperty("url")]
        public string url;
    }
}