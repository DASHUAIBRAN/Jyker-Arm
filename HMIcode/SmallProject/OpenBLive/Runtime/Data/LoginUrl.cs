using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    public struct LoginUrl
    {
        [JsonProperty("data")]
        public LoginUrlData data;
        [JsonProperty("status")]
        public bool status;
    }
}