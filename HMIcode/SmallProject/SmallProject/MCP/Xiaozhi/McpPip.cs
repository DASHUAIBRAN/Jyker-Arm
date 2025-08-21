using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using SmallProject.Logger;
using SmallProject.MCP.Models;
using SmallProject.MCP.Serializers;
using SmallProject.MCP.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SmallProject.MCP.Xiaozhi
{

    internal class McpPip
    {

        private const string INITIAL_BACKOFF_ENV = "INITIAL_BACKOFF";
        private const string MAX_BACKOFF_ENV = "MAX_BACKOFF";

        private static int reconnectAttempt = 0;
        private static double backoff = 1.0;
        private static readonly Random Random = new();

        private static readonly string InitialBackoffStr = Environment.GetEnvironmentVariable(INITIAL_BACKOFF_ENV) ?? "1";
        private static readonly string MaxBackoffStr = Environment.GetEnvironmentVariable(MAX_BACKOFF_ENV) ?? "600";

        private static readonly double INITIAL_BACKOFF = double.TryParse(InitialBackoffStr, out var val) ? val : 1;
        private static readonly double MAX_BACKOFF = double.TryParse(MaxBackoffStr, out var val) ? val : 600;

        private static Dictionary<string, Object> ToolsDictionary = new Dictionary<string, Object>();

        private static ResultBody toolsListBody = new ResultBody();

        public async Task Init()
        {
            await AddTools<JykerControl>();


            var endpointUrl = App.JConfig.EndpointUrl;

            if (string.IsNullOrEmpty(endpointUrl))
            {
                JLog.Info("小智接入点为空。。");
                return;
            }

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                //JLog.Info("Received interrupt signal, shutting down...");
                eventArgs.Cancel = true;
                return;
            };

            while (true)
            {
                try
                {
                    await ConnectWithRetry(endpointUrl);
                }
                catch (Exception ex)
                {
                    JLog.Error(ex);
                    await Task.Delay(TimeSpan.FromSeconds(backoff));
                    backoff = Math.Min(backoff * 2, MAX_BACKOFF);
                    reconnectAttempt++;
                }
                Thread.Sleep(50);
            }
        }

        private async Task ConnectWithRetry(string uri)
        {
            using var cts = new CancellationTokenSource();
            using var client = new ClientWebSocket();

            JLog.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Connecting to WebSocket server...");

            await client.ConnectAsync(new Uri(uri), cts.Token);

            JLog.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Successfully connected to WebSocket server");

            reconnectAttempt = 0;
            backoff = INITIAL_BACKOFF;

            var receiveTask = PipeWebSocketReceive(client, cts.Token);
            var sendTask = PipeWebSocketSend(client, cts.Token);

            await Task.WhenAll(receiveTask, sendTask);

        }
        private async Task PipeWebSocketReceive(ClientWebSocket ws, CancellationToken token)
        {
            var buffer = new byte[4096];

            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer, token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                JLog.Info(message);
                var res =JsonConvert.DeserializeObject<RequestBody>(message);
                switch (res.Method)
                {
                    case "initialize":
                        //JLog.Info("initialize");
                        var b = @"{""jsonrpc"":""2.0"",""id"":0,""result"":{""protocolVersion"":""2024-11-05"",""capabilities"":{""experimental"":{},""prompts"":{""listChanged"":false},""resources"":{""subscribe"":false,""listChanged"":false},""tools"":{""listChanged"":false}},""serverInfo"":{""name"":""JykerContrl"",""version"":""1.12.0""}}}";
                        await ws.SendAsync(Encoding.UTF8.GetBytes(b), WebSocketMessageType.Text, true, token);
                        break;
                    case "notifications/initialized":
                        //JLog.Info("notifications/initialized");
                        break;
                    case "tools/list":
                        //JLog.Info("tools/list");
                        //var a = @"{""jsonrpc"":""2.0"",""id"":"+res.Id+@",""result"":{""tools"":[{""name"":""calculator"",""description"":""For mathamatical calculation, always use this tool to calculate the result of a add b. You can use 'math' or 'random' directly, without 'import'."",""inputSchema"":{""properties"":{""a"":{""title"":""A"",""type"":""integer""},""b"":{""title"":""B"",""type"":""integer""}},""required"":[""a"",""b""],""title"":""calculatorArguments"",""type"":""object""}}]}}";
                        toolsListBody.id = res.Id;
                        var body = JsonConvert.SerializeObject(toolsListBody).ToLower();
                        await ws.SendAsync(Encoding.UTF8.GetBytes(body), WebSocketMessageType.Text, true, token);
                        break;
                    case "tools/call":
                        var oneTool = ToolsDictionary[res.Params.Name];
                        if(oneTool!=null)
                        {
                            Type type = oneTool.GetType();

                            // 获取并调用公共方法
                            MethodInfo publicMethod = type.GetMethod(res.Params.Name, BindingFlags.Public | BindingFlags.Instance);
                            if (publicMethod != null)
                            {
                                var MethodResult = publicMethod
                                    .Invoke(oneTool, res.Params.Arguments.Select(t=>t.Value).ToArray());

                                var resbody = new ResultBody();
                                resbody.id = res.Id;
                                resbody.result = new ContentRoot { content = new List<ResultText>() };
                                var ResultText = new ResultText();
                                ResultText.text = JsonConvert.SerializeObject(new ResultContent
                                {
                                    result = MethodResult
                                }, Formatting.Indented).Replace("\r","");
                                resbody.result.content.Add(ResultText);
                                var json = JsonConvert.SerializeObject(resbody);
                                await ws.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, token);
                                //JLog.Info(json);
                            }
                        }
                        break;
                    case "ping":
                        //JLog.Info("ping");
                        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ResultBody() { id = res.Id}).ToLower());
                        await ws.SendAsync(bytes, WebSocketMessageType.Text, true, token);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task PipeWebSocketSend(ClientWebSocket ws, CancellationToken token)
        {
            string line = "";
            var bytes = Encoding.UTF8.GetBytes(line);
            await ws.SendAsync(bytes, WebSocketMessageType.Text, true, token);
        }
        /// <summary>
        /// 添加工具
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task AddTools<T>() where T :new ()
        {
            var tools = ToolsSerializer.SerializeTool<T>();
            foreach (var tool in tools)
            {
                T t = new T();
                ToolsDictionary.Add(tool.Name.ToLower(), t);
            }
            if(toolsListBody.result==null)
            {
                toolsListBody.result = new ToolRoot { Tools = tools };
            }
            else
            {
                toolsListBody.result.tools.AddRange(tools);
            }
            
        }
    }

}
