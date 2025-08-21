namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 服务器返回的Websocket长链接信息 https://open-live.bilibili.com/document/657d8e34-f926-a133-16c0-300c1afc6e6b
    /// </summary>
    public struct WebsocketInfo
    {
        public int code;
        public string message;
        public WebsocketInfoData data;
    }
}