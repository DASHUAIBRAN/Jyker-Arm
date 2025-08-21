using System;
using Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 赠送大航海的用户数据 https://open-live.bilibili.com/document/f9ce25be-312e-1f4a-85fd-fef21f1637f8
    /// </summary>
    [Serializable]
    public struct UserInfo
    {
        /// <summary>
        /// 购买大航海的用户UID(即将废弃)
        /// </summary>
        [JsonProperty("uid")] public long uid;

        /// <summary>
        /// 购买大航海的用户open_id
        /// </summary>
        [JsonProperty("open_id")] public string open_id;

        /// <summary>
        /// 购买大航海的用户昵称
        /// </summary>
        [JsonProperty("uname")] public string userName;

        /// <summary>
        /// 购买大航海的用户头像
        /// </summary>
        [JsonProperty("uface")] public string userFace;
    }
}