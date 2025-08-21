using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 进入房间数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct Enter
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonProperty("uname")] public string uname;

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [JsonProperty("open_id")] public string open_id;

        /// <summary>
        /// 用户头像
        /// </summary>
        [JsonProperty("uface")] public string uface;

        /// <summary>
        /// 时间秒级时间戳
        /// </summary>
        [JsonProperty("timestamp")] public long timestamp;

        /// <summary>
        /// 发生的直播间
        /// </summary>
        [JsonProperty("room_id")] public long room_id;
    }
}