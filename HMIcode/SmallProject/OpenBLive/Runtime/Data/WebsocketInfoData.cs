using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 服务器返回的Websocket长链接信息 https://open-live.bilibili.com/document/657d8e34-f926-a133-16c0-300c1afc6e6b
    /// </summary>
    public struct WebsocketInfoData
    {
        /// <summary>
        /// ip地址
        /// </summary>
        [JsonProperty("ip")]
        public List<string> ip;
        /// <summary>
        /// host地址 可能是ip 也可能是域名
        /// </summary>
        [JsonProperty("host")]
        public List<string> host;
        /// <summary>
        /// 长连使用的请求json体 第三方无需关注内容,建立长连时使用即可
        /// </summary>
        [JsonProperty("auth_body")]
        public string authBody;
        /// <summary>
        /// tcp 端口号
        /// </summary>
        [JsonProperty("tcp_port")]
        public List<int> tcpPort;
        /// <summary>
        /// ws 端口号
        /// </summary>
        [JsonProperty("ws_port")]
        public List<int> wsPort;
        /// <summary>
        /// wss 端口号
        /// </summary>
        [JsonProperty("wss_port")]
        public List<int> wssPort;
    }
}