using Newtonsoft.Json;
using OpenBLive.Client.Data;
using OpenBLive.Runtime;

namespace OpenBLive.Client
{
    public class BApiClient : IBApiClient
    {
        /// <summary>
        /// 开启互动玩法
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<AppStartInfo> StartInteractivePlay(string code, string appId)
        {
            var respStr = await BApi.StartInteractivePlay(code, appId);
            return JsonConvert.DeserializeObject<AppStartInfo>(respStr);
        }

        /// <summary>
        /// 关闭应用
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="gameId">开启应用 返回的gameId</param>
        /// <returns></returns>
        public async Task<EmptyInfo> EndInteractivePlay(string appId, string gameId)
        {
            var respStr = await BApi.EndInteractivePlay(appId, gameId);
            return JsonConvert.DeserializeObject<EmptyInfo>(respStr);
        }

        /// <summary>
        /// 批量应用心跳
        /// </summary>
        /// <param name="gameIds">开启应用 返回的gameId</param>
        /// <returns></returns>
        public async Task<EmptyInfo> HeartBeatInteractivePlay(string gameId)
        {
            var respStr = await BApi.HeartBeatInteractivePlay(gameId);
            return JsonConvert.DeserializeObject<EmptyInfo>(respStr);
        }

        /// <summary>
        /// 批量应用心跳
        /// </summary>
        /// <param name="gameIds">开启应用 返回的gameId</param>
        /// <returns></returns>
        public async Task<EmptyInfo> BatchHeartBeatInteractivePlay(string[] gameIds)
        {
            var respStr = await BApi.BatchHeartBeatInteractivePlay(gameIds);
            return JsonConvert.DeserializeObject<EmptyInfo>(respStr);
        }
    }
}
