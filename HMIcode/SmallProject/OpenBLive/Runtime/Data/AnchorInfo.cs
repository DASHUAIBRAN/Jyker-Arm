using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 礼物数据中的主播数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct AnchorInfo
    {
        /// <summary>
        /// 主播UID(即将废弃)
        /// </summary>
        [JsonProperty("uid")] public long uid;

        /// <summary>
        /// 主播open_id
        /// </summary>
        [JsonProperty("open_id")] public string open_id;

        /// <summary>
        /// 收礼主播昵称
        /// </summary>
        [JsonProperty("uname")] public string userName;

        /// <summary>
        /// 收礼主播头像
        /// </summary>
        [JsonProperty("uface")] public string userFace;
    }
}