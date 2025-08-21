using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using Logger = OpenBLive.Runtime.Utilities.Logger;
#if NET5_0_OR_GREATER
using System.Net;
#elif UNITY_2020_3_OR_NEWER
using UnityEngine.Networking;
#endif

namespace OpenBLive.Runtime
{
    /// <summary>
    /// 各类b站api
    /// </summary>
    public static class BApi
    {
        /// <summary>
        /// 是否为测试环境的api
        /// </summary>
        public static bool isTestEnv;

        /// <summary>
        /// 开放平台域名
        /// </summary>
        private static string OpenLiveDomain =>
            isTestEnv ? "http://test-live-open.biliapi.net" : "https://live-open.biliapi.com";

        /// <summary>
        /// 应用开启
        /// </summary>
        private const string k_InteractivePlayStart = "/v2/app/start";

        /// <summary>
        /// 应用关闭
        /// </summary>
        private const string k_InteractivePlayEnd = "/v2/app/end";

        /// <summary>
        /// 应用心跳
        /// </summary>
        private const string k_InteractivePlayHeartBeat = "/v2/app/heartbeat";

        /// <summary>
        /// 应用批量心跳
        /// </summary>
        private const string k_InteractivePlayBatchHeartBeat = "/v2/app/batchHeartbeat";


        private const string k_Post = "POST";

 

        public static async Task<string> StartInteractivePlay(string code, string appId)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayStart;
            var param = $"{{\"code\":\"{code}\",\"app_id\":{appId}}}";

            var result = await RequestWebUTF8(postUrl, k_Post, param);

            return result;
        }

        public static async Task<string> EndInteractivePlay(string appId, string gameId)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayEnd;
            var param = $"{{\"app_id\":{appId},\"game_id\":\"{gameId}\"}}";

            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }

        public static async Task<string> HeartBeatInteractivePlay(string gameId)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayHeartBeat;
            string param = "";
            if (gameId != null)
            {
                param = $"{{\"game_id\":\"{gameId}\"}}";

            }

            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }

        public static async Task<string> BatchHeartBeatInteractivePlay(string[] gameIds)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayBatchHeartBeat;
            GameIds games = new GameIds()
            {
                gameIds = gameIds
            };
            var param = JsonConvert.SerializeObject(games);
            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }

        private static async Task<string> RequestWebUTF8(string url, string method, string param,
            string cookie = null)
        {
#if NET5_0_OR_GREATER
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;

            if (param != null)
            {
                SignUtility.SetReqHeader(req, param, cookie);
            }

            HttpWebResponse httpResponse = (HttpWebResponse)(await req.GetResponseAsync());
            Stream stream = httpResponse.GetResponseStream();

            if (stream != null)
            {
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                result = await reader.ReadToEndAsync();
            }

            return result;

#elif UNITY_2020_3_OR_NEWER
            UnityWebRequest webRequest = new UnityWebRequest(url);
            webRequest.method = method;
            if (param != null)
            {
                SignUtility.SetReqHeader(webRequest, param, cookie);
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeUploadHandlerOnDispose = true;
            webRequest.disposeDownloadHandlerOnDispose = true;
            await webRequest.SendWebRequest();
            var text = webRequest.downloadHandler.text;

            webRequest.Dispose();
            return text;
#endif
        }
#if UNITY_2020_3_OR_NEWER
        private static TaskAwaiter GetAwaiter(this UnityEngine.AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += _ => { tcs.SetResult(null); };
            return ((Task) tcs.Task).GetAwaiter();
        }
#endif
    }
}