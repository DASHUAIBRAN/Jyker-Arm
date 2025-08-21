using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 点赞数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct Like
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonProperty("uname")] public string uname;

        /// <summary>
        /// 用户UID(即将废弃)
        /// </summary>
        [JsonProperty("uid")] public long uid;

        /// <summary>
        /// 用户唯一标识(2024-03-11后上线)
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

        /// <summary>
        /// 点赞文案
        /// </summary>
        [JsonProperty("like_text")] public string like_text;

        /// <summary>
        /// 对单个用户最近2秒的点赞次数聚合
        /// </summary>
        [JsonProperty("like_count")] public long unamelike_count;

        /// <summary>
        /// 该房间粉丝勋章佩戴情况
        /// </summary>
        [JsonProperty("fans_medal_wearing_status")] public bool fans_medal_wearing_status;

        /// <summary>
        /// 粉丝勋章名
        /// </summary>
        [JsonProperty("fans_medal_name")] public string fans_medal_name;

        /// <summary>
        /// 对应房间勋章信息
        /// </summary>
        [JsonProperty("fans_medal_level")] public long fans_medal_level;
    }
}