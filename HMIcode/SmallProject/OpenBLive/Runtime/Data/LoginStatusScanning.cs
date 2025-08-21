using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 扫码中未登录
    /// </summary>
    public struct LoginStatusScanning
    {
        [JsonProperty("status")] public bool status;
        [JsonProperty("data")] public int data;
        [JsonProperty("message")] public string message;
    }
}