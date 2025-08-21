using OpenBLive.Client;
using OpenBLive.Client.Data;
using OpenBLive.Runtime.Utilities;
using OpenBLive.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenBLive.Runtime.Data;
using SmallProject.Logger;
using System.Threading;

namespace SmallProject.BiliBili
{
    internal class BStationLive
    {
        //初始化于测试的参数
        public const string AccessKeyId = "";//填入你的accessKeyId，可以在直播创作者服务中心【个人资料】页面获取(https://open-live.bilibili.com/open-manage)
        public const string AccessKeySecret = "";//填入你的accessKeySecret，可以在直播创作者服务中心【个人资料】页面获取(https://open-live.bilibili.com/open-manage)
        public const string AppId = "";//填入你的appId，可以在直播创作者服务中心【我的项目】页面创建应用后获取(https://open-live.bilibili.com/open-manage)
        public const string Code = "";//填入你的主播身份码Code，可以在互动玩法首页，右下角【身份码】处获取(互玩首页：https://play-live.bilibili.com/)

        public static IBApiClient bApiClient = new BApiClient();
        public static string game_id = string.Empty;
        public static bool IsLive = false;

        public async Task Init()
        {
            //是否为测试环境（一般用户可无视，给专业对接测试使用）
            BApi.isTestEnv = false;

            SignUtility.accessKeyId = AccessKeyId;
            SignUtility.accessKeySecret = AccessKeySecret;
            var appId = AppId;
            var code = Code;


            var startInfoLive = new AppStartInfo();
            if (!string.IsNullOrEmpty(appId))
            {
                startInfoLive = await bApiClient.StartInteractivePlay(code, appId);
                if (startInfoLive?.Code != 0)
                {
                    return;
                }

                var gameId = startInfoLive?.Data?.GameInfo?.GameId;
                if (gameId != null)
                {
                    game_id = gameId;
                    IsLive = true;
                    //JLog.InfoLive("成功开启，开始心跳，场次ID: " + gameId);

                    //心跳API（用于保持在线）
                    InteractivePlayHeartBeat m_PlayHeartBeat = new InteractivePlayHeartBeat(gameId);
                    m_PlayHeartBeat.HeartBeatError += M_PlayHeartBeat_HeartBeatError;
                    m_PlayHeartBeat.HeartBeatSucceed += M_PlayHeartBeat_HeartBeatSucceed;
                    m_PlayHeartBeat.Start();

                    //长链接（用户持续接收服务器推送消息）
                    WebSocketBLiveClient m_WebSocketBLiveClient;
                    m_WebSocketBLiveClient = new WebSocketBLiveClient(startInfoLive.GetWssLink(), startInfoLive.GetAuthBody());
                    m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;//弹幕事件
                    m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;//礼物事件
                    m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;//大航海事件
                    m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;//SC事件
                    m_WebSocketBLiveClient.OnLike += M_WebSocketBLiveClient_OnLike;//点赞事件(点赞需要直播间开播才会触发推送)
                    m_WebSocketBLiveClient.OnEnter += M_WebSocketBLiveClient_OnEnter;//观众进入房间事件
                    m_WebSocketBLiveClient.OnLiveStart += M_WebSocketBLiveClient_OnLiveStart;//直播间开始直播事件
                    m_WebSocketBLiveClient.OnLiveEnd += M_WebSocketBLiveClient_OnLiveEnd;//直播间停止直播事件
                    //m_WebSocketBLiveClient.Connect();//正常连接
                    m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(10));//失败后30秒重连
                }
                else
                {
                    JLog.InfoLive("开启玩法错误: " + startInfoLive.ToString());
                }
                //await Task.Run(async () =>
                //{
                //    var closeTime = int.Parse(closeTimeStr);
                //    await Task.Delay(closeTime * 1000);
                //    var ret = await bApiClient.EndInteractivePlay(appId, gameId);
                //    IsLive = false;
                //    Console.WriteLine("关闭玩法: " + ret.ToString());
                //    return;
                //});
            }


        }

        private static void M_WebSocketBLiveClient_OnLiveEnd(LiveEnd liveEnd)
        {
            StringBuilder sb = new StringBuilder($"直播间[{liveEnd.room_id}]直播结束，分区ID：【{liveEnd.area_id}】,标题为【{liveEnd.title}】");
            JLog.InfoLive(sb.ToString());
        }

        private static void M_WebSocketBLiveClient_OnLiveStart(LiveStart liveStart)
        {
            StringBuilder sb = new StringBuilder($"直播间[{liveStart.room_id}]开始直播，分区ID：【{liveStart.area_id}】,标题为【{liveStart.title}】");
            JLog.InfoLive(sb.ToString());
        }

        private static void M_WebSocketBLiveClient_OnEnter(Enter enter)
        {
            StringBuilder sb = new StringBuilder($"用户[{enter.uname}]进入房间");
            JLog.InfoLive(sb.ToString());
        }

        private static void M_WebSocketBLiveClient_OnLike(Like like)
        {
            StringBuilder sb = new StringBuilder($"用户[{like.uname}]点赞了{like.unamelike_count}次");
            JLog.InfoLive(sb.ToString());
        }

        private static void M_PlayHeartBeat_HeartBeatSucceed()
        {
            //JLog.InfoLive("心跳成功");
        }

        private static void M_PlayHeartBeat_HeartBeatError(string json)
        {
            JsonConvert.DeserializeObject<EmptyInfo>(json);
            //JLog.InfoLive("心跳失败" + json);
        }

        private static void WebSocketBLiveClientOnSuperChat(SuperChat superChat)
        {
            StringBuilder sb = new StringBuilder($"用户[{superChat.userName}]发送了{superChat.rmb}元的醒目留言内容：{superChat.message}");
            JLog.InfoLive(sb.ToString());
        }

        private static void WebSocketBLiveClientOnGuardBuy(Guard guard)
        {
            StringBuilder sb = new StringBuilder($"用户[{guard.userInfo.userName}]充值了{(guard.guardUnit == "月" ? (guard.guardNum + "个月") : guard.guardUnit.TrimStart('*'))}[{(guard.guardLevel == 1 ? "总督" : guard.guardLevel == 2 ? "提督" : "舰长")}]大航海");
            JLog.InfoLive(sb.ToString());
        }

        private static void WebSocketBLiveClientOnGift(SendGift sendGift)
        {
            StringBuilder sb = new StringBuilder($"用户[{sendGift.userName}]赠送了{sendGift.giftNum}个[{sendGift.giftName}]");
            JLog.InfoLive(sb.ToString());
            
        }

        private static void WebSocketBLiveClientOnDanmaku(Dm dm)
        {
            StringBuilder sb = new StringBuilder($"用户[{dm.userName}]发送弹幕:{dm.msg}");
            JLog.InfoLive(sb.ToString());
            var curr = App.Core.Jyker.currentJoints;
            var joint1 = curr.a[0];
            var joint2 = curr.a[1];
            var joint3 = curr.a[2];
            switch (dm.msg)
            {
                case "111":
                    if (joint1 > 30)
                    {
                        JLog.InfoLive("左不了了");
                        return;
                    }
                    App.Core.Jyker.Move(new double[]{joint1+20, curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "222":
                    if (joint1 < -90)
                    {
                        JLog.InfoLive("右不了了");
                        return;
                    }
                    App.Core.Jyker.Move(new double[]{ joint1-20, curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "333":
                    if (joint2 > 150)
                    {
                        JLog.InfoLive("前不了了");
                        return;
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], joint2+10, joint3-10
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "444":
                    if (joint2 - 1 < -90)
                    {
                        JLog.InfoLive("退不了了");
                        return;
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], joint2-10, joint3+10
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "555":
                    if (curr.a[4] - 10 < -90)
                    {
                        JLog.InfoLive("脖子没那么长啦");
                        return;
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4]-20, curr.a[5] });
                    break;
                case "666":
                    if (curr.a[4] + 10 > 90)
                    {
                        JLog.InfoLive("脖子没那么长啦");
                        return;
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4]+20, curr.a[5] });
                    break;
                case "777":
                    App.Core.Jyker.Move(new double[]{curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], 120 });
                    Thread.Sleep(1000);
                    App.Core.Jyker.GetStatus(6);
                    Thread.Sleep(200);
                    JLog.Info($"Current {App.Core.Jyker.motorJ[5].Current}");
                    if (App.Core.Jyker.motorJ[5].Current < 0.5)
                    {
                        JLog.Info("没抓到东西");
                        return;
                    }
                    else
                    {
                        JLog.Info("抓住了");
                        return;
                    }
                case "888":
                    App.Core.Jyker.Move(new double[]{curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], 0 });
                    break;
                default:
                    break;
            }
        
        }
    }
}
