using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 结束直播数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct LiveEnd
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [JsonProperty("open_id")] public string open_id;


        /// <summary>
        /// 时间秒级时间戳
        /// </summary>
        [JsonProperty("timestamp")] public long timestamp;

        /// <summary>
        /// 发生的直播间
        /// </summary>
        [JsonProperty("room_id")] public long room_id;

        /// <summary>
        /// 开播时的标题
        /// </summary>
        [JsonProperty("title")] public string title;

        /// <summary>
        /// 开播的分区ID
        /// </summary>
        [JsonProperty("area_id")] public long area_id;
    }
}