using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;

namespace OpenBLive.Runtime
{
    public delegate void ReceiveDanmakuEvent(Dm dm);

    public delegate void ReceiveGiftEvent(SendGift sendGift);

    public delegate void ReceiveGuardBuyEvent(Guard guard);

    public delegate void ReceiveSuperChatEvent(SuperChat e);

    public delegate void ReceiveSuperChatDelEvent(SuperChatDel e);

    public delegate void ReceiveLikeEvent(Like like);

    public delegate void ReceiveEnterEvent(Enter enter);
    
    public delegate void ReceiveLiveStartEvent(LiveStart liveStart);
    
    public delegate void ReceiveLiveEndEvent(LiveEnd liveEnd);

    public delegate void ReceiveRawNotice(string raw, JObject jObject);

    public abstract class BLiveClient : IDisposable
    {
        private Timer m_Timer;
        protected string token;

        /// <summary>
        /// 弹幕数据
        /// </summary>
        public event ReceiveDanmakuEvent OnDanmaku;

        /// <summary>
        /// 赠送礼物
        /// </summary>
        public event ReceiveGiftEvent OnGift;

        public event ReceiveGuardBuyEvent OnGuardBuy;

        /// <summary>
        /// SC赠送
        /// </summary>
        public event ReceiveSuperChatEvent OnSuperChat;

        /// <summary>
        /// SC删除
        /// </summary>
        public event ReceiveSuperChatDelEvent OnSuperChatDel;

        /// <summary>
        /// 点赞信息
        /// </summary>
        public event ReceiveLikeEvent OnLike;

        /// <summary>
        /// 进入房间
        /// </summary>
        public event ReceiveEnterEvent OnEnter;

        /// <summary>
        /// 开始直播
        /// </summary>
        public event ReceiveLiveStartEvent OnLiveStart;

        /// <summary>
        /// 停止直播
        /// </summary>
        public event ReceiveLiveEndEvent OnLiveEnd;

        /// <summary>
        /// 原始数据包
        /// </summary>
        public event ReceiveRawNotice ReceiveNotice;

        /// <summary>
        /// 更新人气
        /// </summary>
        public event EventHandler<int> UpdatePopularity;

        public event EventHandler Open;
        public abstract void Connect();
        /// <summary>
        /// 带有重连
        /// </summary>
        public abstract void Connect(TimeSpan timeout);
        public abstract void Disconnect();
        public abstract void Dispose();
        public abstract void Send(byte[] packet);
        public abstract Task SendAsync(byte[] packet);
        public abstract void Send(Packet packet);
        protected abstract Task SendAsync(Packet packet);

        protected virtual void OnOpen()
        {
            SendAsync(Packet.Authority(token));

            m_Timer?.Dispose();
            m_Timer = new Timer((e) => (
                (BLiveClient)e)?.SendAsync(Packet.HeartBeat()), this, 0, 30 * 1000);
        }
#if UNITY_2021_2_OR_NEWER || NET5_0_OR_GREATER
        protected void ProcessPacket(ReadOnlySpan<byte> bytes) =>
            ProcessPacketAsync(new Packet(bytes));
#else
        protected void ProcessPacket(byte[] bytes) =>
            ProcessPacketAsync(new Packet(bytes));
#endif


        private void ProcessPacketAsync(Packet packet)
        {
            var header = packet.Header;
            switch (header.ProtocolVersion)
            {
                case ProtocolVersion.UnCompressed:
                case ProtocolVersion.HeartBeat:
                    break;
                case ProtocolVersion.Zlib:
                    //no Zlib compress in OpenBLive wss
                    //await foreach (var packet1 in ZlibDeCompressAsync(packet.PacketBody))
                    //ProcessPacketAsync(packet1);
                    return;
                case ProtocolVersion.Brotli:
                    //no Brotli compress in OpenBLive wss
                    //await foreach (var packet1 in BrotliDecompressAsync(packet.PacketBody))
                    //ProcessPacketAsync(packet1);
                    return;
                default:
                    throw new NotSupportedException(
                        "New bilibili danmaku protocol appears, please contact the author if you see this Exception.");
            }

            switch (header.Operation)
            {
                case Operation.AuthorityResponse:
                    Open?.Invoke(this, null);
                    break;
                case Operation.HeartBeatResponse:
                    Array.Reverse(packet.PacketBody);

#if UNITY_2021_2_OR_NEWER || NET5_0_OR_GREATER
                    var popularity = BitConverter.ToInt32(packet.PacketBody);
#else
                    var popularity = BitConverter.ToInt32(packet.PacketBody,0);
#endif

                    UpdatePopularity?.Invoke(this, popularity);
                    break;
                case Operation.ServerNotify:
                    try
                    {
                        ProcessNotice(Encoding.UTF8.GetString(packet.PacketBody));
                    }
                    catch (Exception)
                    {
                    }
                    break;
                // HeartBeat packet request, only send by client
                case Operation.HeartBeat:
                // This operation key only used for sending authority packet by client
                case Operation.Authority:
                default:
                    break;
            }
        }


        private void ProcessNotice(string rawMessage)
        {
            var json = JObject.Parse(rawMessage);
            ReceiveNotice?.Invoke(rawMessage, json);
            var data = json["data"]?.ToString();
            if (String.IsNullOrWhiteSpace(data))
                return;
            Logger.Log($"收到长连发来数据：\n{json?.ToString()}");
            try
            {
                switch (json["cmd"]?.ToString())
                {
                    case "LIVE_OPEN_PLATFORM_DM":
                        var dm = JsonConvert.DeserializeObject<Dm>(data);
                        OnDanmaku?.Invoke(dm);
                        break;
                    case "LIVE_OPEN_PLATFORM_SUPER_CHAT":
                        var superChat = JsonConvert.DeserializeObject<SuperChat>(data);
                        OnSuperChat?.Invoke(superChat);
                        break;
                    case "LIVE_OPEN_PLATFORM_SUPER_CHAT_DEL":
                        var superChatDel = JsonConvert.DeserializeObject<SuperChatDel>(data);
                        OnSuperChatDel?.Invoke(superChatDel);
                        break;
                    case "LIVE_OPEN_PLATFORM_SEND_GIFT":
                        var gift = JsonConvert.DeserializeObject<SendGift>(data);
                        OnGift?.Invoke(gift);
                        break;
                    case "LIVE_OPEN_PLATFORM_GUARD":
                        var guard = JsonConvert.DeserializeObject<Guard>(data);
                        OnGuardBuy?.Invoke(guard);
                        break;
                    case "LIVE_OPEN_PLATFORM_LIKE":
                        var like = JsonConvert.DeserializeObject<Like>(data);
                        OnLike?.Invoke(like);
                        break;
                    case "LIVE_OPEN_PLATFORM_LIVE_ROOM_ENTER":
                        var enter = JsonConvert.DeserializeObject<Enter>(data);
                        OnEnter?.Invoke(enter);
                        break;
                    case "LIVE_OPEN_PLATFORM_LIVE_START":
                        var live_start = JsonConvert.DeserializeObject<LiveStart>(data);
                        OnLiveStart?.Invoke(live_start);
                        break;
                    case "LIVE_OPEN_PLATFORM_LIVE_END":
                        var live_end = JsonConvert.DeserializeObject<LiveEnd>(data);
                        OnLiveEnd?.Invoke(live_end);
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.LogError("json数据解析异常 rawMessage: " + rawMessage + e.Message);
            }
        }
    }
}