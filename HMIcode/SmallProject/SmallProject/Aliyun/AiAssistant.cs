using NAudio.Wave;
using Newtonsoft.Json;
using SmallProject.Aliyun.Models;
using SmallProject.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallProject.Aliyun
{
    internal class AiAssistant
    {
        private static readonly string workspace_id = "llm-zfcwus2axt2ik8ya";
        private static readonly string apiKey = "sk-87479f81a3494310b1baf77aa72c21d9"; // 替换为你的API Key
        private static readonly string app_id = "mm_2f63ea3eca2b48e3a7f1e65fffc4";
        private static readonly string webSocketUrl = "wss://dashscope.aliyuncs.com/api-ws/v1/inference";
        private static readonly int sampleRate = 16000; // 设置采样率为16kHz
        private static ClientWebSocket socket;
        private static WaveInEvent waveInEvent;

        private AssistantState state = AssistantState.None;
        public async Task Init()
        {
            string outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "output.wav");

            //var recorder = new AudioRecorder(outputFilePath);
            //recorder.StartRecording();
            //JLog.Info("开始录音");
            //Thread.Sleep(2000);
            //recorder.StopRecording();
            //JLog.Info($"录音完成，文件已保存至: {outputFilePath}");

            // 初始化WebSocket连接
            socket = new ClientWebSocket();
            socket.Options.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            await socket.ConnectAsync(new Uri(webSocketUrl), CancellationToken.None);
            JLog.Info("已连接到WebSocket服务器...");

            // 发送Start指令
            var msg = new SocketMessage();
            msg.payload.input.workspace_id = workspace_id;
            msg.payload.input.app_id = app_id;
            string startMessage = JsonConvert.SerializeObject(msg);
            await SendMessage(socket, startMessage);

            // 开始录音并实时发送音频数据
            StartRecordingAndSendData();

            // 接收服务端返回的消息
            await ReceiveMessages(socket);
        }

        void StartRecordingAndSendData()
        {
            waveInEvent = new WaveInEvent
            {
                WaveFormat = new WaveFormat(sampleRate, 1), // 单声道，16kHz
                BufferMilliseconds = 100 // 每次捕获100ms的数据
            };

            waveInEvent.DataAvailable += WaveInEvent_DataAvailable;

            waveInEvent.StartRecording();
            JLog.Info("开始录音...");
        }

        private void WaveInEvent_DataAvailable(object? sender, WaveInEventArgs e)
        {
            //JLog.Info("here");
            if (socket.State == WebSocketState.Open&& state == AssistantState.Listening)
            {
                if (e.Buffer.Length == 0) return;
                // 直接发送PCM格式的音频数据
                socket.SendAsync(new ArraySegment<byte>(e.Buffer, 0, e.BytesRecorded),
                    WebSocketMessageType.Binary, e.BytesRecorded < waveInEvent.WaveFormat.AverageBytesPerSecond / 10, CancellationToken.None);
            }
        }

        async Task SendMessage(ClientWebSocket socket, string message)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            JLog.Info($"发送消息: {message}");
        }

        async Task ReceiveMessages(ClientWebSocket socket)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }
                else
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var res = JsonConvert.DeserializeObject<SocketReceive>(receivedText);
                    if(res.payload.output.state.Equals(AssistantState.Listening.ToString()))
                    {
                        state = AssistantState.Listening;

                    }
                    JLog.Info($"收到服务端消息: {receivedText}");
                }
            }
        }


       
    }


    public class AudioRecorder
    {
        private readonly string _outputFilePath;
        private WaveInEvent _waveIn;
        private WaveFileWriter _writer;

        public AudioRecorder(string outputFilePath)
        {
            _outputFilePath = outputFilePath;
        }

        public void StartRecording()
        {
            if (_waveIn != null) return;

            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1) // 16kHz, 单声道（适合语音识别）
            };

            _writer = new WaveFileWriter(_outputFilePath, _waveIn.WaveFormat);

            _waveIn.DataAvailable += (s, e) =>
            {
                _writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            _waveIn.RecordingStopped += (s, e) =>
            {
                _writer.Dispose();
                _writer = null;
                _waveIn.Dispose();
                _waveIn = null;
            };

            _waveIn.StartRecording();
        }

        public void StopRecording()
        {
            _waveIn?.StopRecording();
        }
    }
}
