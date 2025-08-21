using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 登录成功
    /// </summary>
    public struct LoginStatusReady
    {
        [JsonProperty("code")] public int code;
        [JsonProperty("status")] public bool status;
        [JsonProperty("data")] public LoginStatusData data;
    }
}