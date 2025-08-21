using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 弹幕数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct Dm
    {
        /// <summary>
        /// 用户UID(即将废弃)
        /// </summary>
        [JsonProperty("uid")] public long uid;

        /// <summary>
        /// 用户open_id
        /// </summary>
        [JsonProperty("open_id")] public string open_id;

        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonProperty("uname")] public string userName;

        /// <summary>
        /// 用户头像
        /// </summary>
        [JsonProperty("uface")] public string userFace;

        /// <summary>
        /// 弹幕发送时间秒级时间戳
        /// </summary>
        [JsonProperty("timestamp")] public long timestamp;


        /// <summary>
        /// 弹幕内容
        /// </summary>
        [JsonProperty("msg")] public string msg;

        /// <summary>
        /// 粉丝勋章等级
        /// </summary>
        [JsonProperty("fans_medal_level")] public long fansMedalLevel;

        /// <summary>
        /// 粉丝勋章名
        /// </summary>
        [JsonProperty("fans_medal_name")] public string fansMedalName;

        /// <summary>
        /// 佩戴的粉丝勋章佩戴状态
        /// </summary>
        [JsonProperty("fans_medal_wearing_status")]
        public bool fansMedalWearingStatus;

        /// <summary>
        /// 大航海等级
        /// </summary>
        [JsonProperty("guard_level")] public long guardLevel;

        /// <summary>
        /// 弹幕接收的直播间
        /// </summary>
        [JsonProperty("room_id")] public long roomId;
    }
}