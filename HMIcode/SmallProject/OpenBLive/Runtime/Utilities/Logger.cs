#if NET5_0_OR_GREATER
using System;
#else
using UnityEngine;
#endif



namespace OpenBLive.Runtime.Utilities
{
    public static class Logger
    {
        public static void LogError(string logInfo)
        {
#if NET5_0_OR_GREATER
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：{logInfo}");
#else
            Debug.LogError(logInfo);
#endif
        }

        public static void Log(string logInfo)
        {
#if NET5_0_OR_GREATER
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：{logInfo}");
#else
            Debug.Log(logInfo);
#endif
        }
    }
}