using OpenBLive.Client.Data;

namespace OpenBLive.Client
{
    /// <summary>
    /// BapiClient 相关文档 https://open-live.bilibili.com/document/doc&tool/auth.html
    /// </summary>
    public interface IBApiClient
    {
        /// <summary>
        /// 开启应用
        /// </summary>
        /// <param name="code">
        /// 身份码
        /// 获取地址 :https://link.bilibili.com/p/center/index#/my-room/start-live
        /// </param>
        /// <param name="appId">
        /// AppId 
        /// 申请的应用Id
        /// </param>
        /// <returns></returns>
        Task<AppStartInfo> StartInteractivePlay(string code, string appId);
        /// <summary>
        /// 关闭应用
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="gameId">开启玩法 返回的gameId</param>
        /// <returns></returns>
        Task<EmptyInfo> EndInteractivePlay(string appId, string gameId);
        /// <summary>
        /// 应用心跳
        /// </summary>
        /// <param name="gameId">开启玩法 返回的gameId</param>
        /// <returns></returns>
        Task<EmptyInfo> HeartBeatInteractivePlay(string gameId);
        /// <summary>
        /// 批量应用心跳
        /// </summary>
        /// <param name="gameIds">开启玩法 返回的gameId</param>
        /// <returns></returns>
        Task<EmptyInfo> BatchHeartBeatInteractivePlay(string[] gameIds);
    }
}
