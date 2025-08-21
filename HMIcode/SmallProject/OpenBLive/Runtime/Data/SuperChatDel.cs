using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 付费留言数据下线数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct SuperChatDel
    {
        /// <summary>
        /// 直播间ID
        /// </summary>
        [JsonProperty("room_id")] public long roomId;

        /// <summary>
        /// 留言id
        /// </summary>
        [JsonProperty("message_ids")] public long[] messageIds;
    }
}