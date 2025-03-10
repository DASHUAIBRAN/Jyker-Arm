﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Logger
{
    internal class Log
    {
        internal static event Action<string> MessageEvent;

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="msg"></param>
        internal static void Error(Exception msg)
        {
            Masuit.Tools.Logging.LogManager.Error(msg);
            //MessageEvent?.Invoke(msg.Message);
        }

        /// <summary>
        /// 打印普通信息
        /// </summary>
        /// <param name="msg"></param>
        internal static void Info(string msg)
        {
            Masuit.Tools.Logging.LogManager.Info(msg);
            MessageEvent?.Invoke(msg);
        }
    }
}
