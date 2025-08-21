using System;
using Newtonsoft.Json;
namespace OpenBLive.Runtime.Data
{
    /// <summary>
    /// 互动玩法心跳 https://open-live.bilibili.com/doc/2/1/3
    /// </summary>
    public struct GameIds
    {
        /// <summary>
        /// 玩法场次
        /// </summary>
        [JsonProperty("game_ids")]
        public string[] gameIds;
    }
}