using HalconDotNet;
using NAudio.CoreAudioApi;
using SmallProject.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallProject.MCP.Tools
{
    public class JykerControl
    {
        //[Description("For mathamatical calculation, always use this tool to calculate the result of a add b")]
        //public long calculator(long a, long b)
        //{
        //    return a + b;
        //}

        [Description("用 前 后 左 右 上 下 六个命令控制方向，如果返回移动了就是移动成功了，没有就是没有成功。")]
        public string jyker_move(string direction)
        {
            var curr = App.Core.Jyker.currentJoints;
            JLog.Info(direction);
            var joint1 = curr.a[0];
            var joint2 = curr.a[1];
            var joint3 = curr.a[2];

            switch (direction)
            {
                case "前":
                    
                    App.Core.Jyker.Move(new double[]{ curr.a[0], joint2+10, joint3-10
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "后":
                    if(joint2-1 < -90)
                    {
                        return "退不了了";
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], joint2-10, joint3+10
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "左":
                    if(joint1>30)
                    {
                        return "左不了了";
                    }
                    App.Core.Jyker.Move(new double[]{joint1+20, curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "右":
                    if(joint1<-90)
                    {
                        return "右不了了";
                    }
                    App.Core.Jyker.Move(new double[]{ joint1-20, curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], curr.a[5] });
                    break;
                case "上":
                    if (curr.a[4] - 10 < -90)
                    {
                        return "脖子没那么长啦";
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4]-20, curr.a[5] });
                    break;
                case "下":
                    if (curr.a[4] + 10 > 90)
                    {
                        return "脖子没那么长啦";
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4]+20, curr.a[5] });
                    break;
                default:
                    break;
            }
            return "移动了";
        }

        [Description("这个方法会返回看到了什么,传入一个direction 的参数，下 代表往下看 上 代表往上看")]
        public string jyker_view(string direction)
        {
            var curr = App.Core.Jyker.currentJoints;
            switch (direction)
            {
                case "上":
                    if(curr.a[4] - 10<-90)
                    {
                        return "脖子没那么长啦";
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4]-20, curr.a[5] });
                    break;
                case "下":
                    if (curr.a[4]+10>90)
                    {
                        return "脖子没那么长啦";
                    }
                    App.Core.Jyker.Move(new double[]{ curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4]+20, curr.a[5] });
                    break;
                default:
                    break;
            }
            JLog.Info("我看到了");
            return "苹果";
        }

        [Description("这个方法传入一个参数state ,如果是抓，就是抓住，如果是放，就是放开")]
        public string jyker_clumporopen(string state)
        {
            var curr = App.Core.Jyker.currentJoints;
            var joint6 = curr.a[5];
            JLog.Info(state);
            switch (state)
            {
                case "抓":
                    App.Core.Jyker.Move(new double[]{curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], 100 });
                    Thread.Sleep(1000);
                    App.Core.Jyker.GetStatus(6);
                    Thread.Sleep(200);
                    JLog.Info($"Current {App.Core.Jyker.motorJ[5].Current}");
                    if (App.Core.Jyker.motorJ[5].Current<0.5)
                    {
                        JLog.Info("没抓到东西");
                        return "没抓到东西";
                    }
                    else
                    {
                        JLog.Info("抓住了");
                        return "抓住了";
                    }
                case "放":
                    App.Core.Jyker.Move(new double[]{curr.a[0], curr.a[1], curr.a[2]
                        , curr.a[3], curr.a[4], 0 });
                    break;
                default:
                    break;
            }

            return "ok";
        }

        [Description("当有人跟你说这个东西是什么的时候，麻烦告诉一下系统，让系统知道这个东西是什么 传入的参数就是物体的名字")]
        public string jyker_thisis(string thing)
        {
            JLog.Info($"jyker_thisis {thing}");
            App.Core.ViewName = thing;
            //创建shapemodel
            
            return "知道了";
        }

        [Description("当阿然叫你富贵的时候麻烦告诉一下系统 ")]
        public string jyker_wakeup()
        {
            JLog.Info("唤醒了");
            return "ok";
        }

    }
}
