using Emgu.CV;
using HalconDotNet;
using Masuit.Tools.Strings;
using Microsoft.VisualBasic.Logging;
using NAudio.CoreAudioApi;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using SmallProject.Aliyun.Models;
using SmallProject.Logger;
using SmallProject.Utils;
using SmallProject.YOLO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Yolov5Net.Scorer;
using static System.Formats.Asn1.AsnWriter;

namespace SmallProject.WindowController
{
    class JykerViewContrl
    {
        private MainWindow M;
        bool IsVideo = false;
        Bitmap bitmap;
        Dictionary<HTuple,string> shapeModels = new Dictionary<HTuple,string>();
        public JykerViewContrl(MainWindow m)
        {
            M = m;
            M.bt_StartDetect.Click += Bt_StartDetect_Click;
            //加载model 
            LoadModels();
        }

        private void LoadModels()
        {
            string path = App.JConfig.FindModelPath;
            string searchPattern = "*.model";
            string[] files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    HOperatorSet.ReadShapeModel(file, out HTuple modelID);
                    shapeModels.Add(modelID, file.Replace(".model",""));
                }
            }
            
        }

        private void Bt_StartDetect_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (!IsVideo)
            {
                IsVideo = true;
                Task.Run(() => {
                    try
                    {
                        // 创建视频捕获对象并打开默认摄像头（0）
                        using (var capture = new OpenCvSharp.VideoCapture(1))
                        {
                            if (!capture.IsOpened())
                            {
                                JLog.Info("好像没有找到摄像头哦");
                                return;
                            }

                            while (true)
                            {
                                if (!IsVideo)
                                {
                                    M.Dispatcher.Invoke(() =>
                                    {
                                        M.bt_StartDetect.Content = "开启视频";
                                    });
                                    break;
                                }
                                M.Dispatcher.Invoke(() =>
                                {
                                    M.bt_StartDetect.Content = "运行中";
                                });

                                OpenCvSharp.Mat frame = new OpenCvSharp.Mat();

                                // 从摄像头中读取当前帧图像
                                bool success = capture.Read(frame);

                                if (!success || frame.Empty())
                                    continue;

                                // 在这里进行处理或显示图像
                                bitmap = BitmapConverter.ToBitmap(frame);
                                M.Dispatcher.Invoke(() =>
                                {

                                    if(shapeModels.Count>0)
                                    {
                                        var hv_ModelIDs = new HTuple();
                                        var NumMatches = new HTuple();
                                        foreach (var mod in shapeModels)
                                        {
                                            hv_ModelIDs = hv_ModelIDs.TupleConcat(mod.Key);
                                            NumMatches = NumMatches.TupleConcat(1);
                                        }
                                        var Hobj = PicUtil.BitmapToHobject(bitmap);
                                        //Image, ModelIDs, -0.39, 0.78, 0.9, 1.1, 0.5, 1, \
                                        //0.5, 'least_squares', 0, 0.9, Row, Column, Angle,\
                                        //Scale, Score, Model
                                        HOperatorSet.FindScaledShapeModels(Hobj, hv_ModelIDs, -0.39
              , 0.78, 0.2, 0.9, 1.1, NumMatches, 0.5, "interpolation", 0, 0.9, out HTuple hv_Row,
              out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Scale, out HTuple hv_Score, out HTuple modelID);
                                        if(hv_Scale.Length>0)
                                        {

                                            //border.findShapeModelEntity.Angle = Math.Abs(((double)hv_Angle) / Math.PI * 180);
                                            //border.findShapeModelEntity.Score = (double)hv_Score;
                                            //border.findShapeModelEntity.X = hv_Column - width / 2;
                                            //border.findShapeModelEntity.Y = hv_Row - height / 2;
                                            //border.findShapeModelEntity.Width = width;
                                            //border.findShapeModelEntity.Height = height;
                                            var name = shapeModels[modelID];
                                            HOperatorSet.GetImageSize(Hobj, out HTuple width, out HTuple height);
                                            float x, y, w, h;
                                            x = hv_Column - width / 2;
                                            y = hv_Row - height / 2;
                                            w = width;
                                            h = height;
                                            DrawToBitmap.DrawRectangle(bitmap, x, y, w, h,
                                                text: $"{name}({hv_Score})");
                                        }
                                    }

                                    
                                    

                                    //AreaDetectYolo8 areaDetectBarError = new AreaDetectYolo8("yolov8n");
                                    //var resList = areaDetectBarError.getPrediction(bitmap);
                                    //if (resList.Count() > 0)
                                    //{
                                    //    foreach (var item in resList)
                                    //    {
                                    //        //if (item.Label.Id != 0) continue;
                                    //        DrawToBitmap.DrawRectangle(bitmap, item.Rectangle.X, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height,
                                    //            text: $"{item.Label.Title}({item.Score})");
                                    //    }
                                    //}

                                    M.ImageBig.Source = PicUtil.ToBitmapSource(bitmap);

                                    if (!string.IsNullOrEmpty(App.Core.ViewName))
                                    {
                                        string dir = App.JConfig.FindModelPath;
                                        if(!Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                        PicUtil.SaveImageToFile(PicUtil.ToBitmapSource(bitmap), $"{dir}/{App.Core.ViewName}.jpg");
                                        HOperatorSet.ReadImage(out HObject part, $"{dir}/{App.Core.ViewName}.jpg");
                                        HOperatorSet.CreateScaledShapeModel(part, 5
                                            , (new HTuple(-45)).TupleRad()
                                          , (new HTuple(90)).TupleRad(), "auto"
                                          , 0.8,1.0, "auto", "none", "ignore_global_polarity", 40, 10, out HTuple hv_ModelID);
                                        HOperatorSet.WriteShapeModel(hv_ModelID, $"{dir}/{App.Core.ViewName}.model");
                                        shapeModels.Add(hv_ModelID, App.Core.ViewName);
                                        App.Core.ViewName = "";
                                    }
                                });

                                Thread.Sleep(50);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        M.Dispatcher.Invoke(() =>
                        {
                            M.bt_StartDetect.Content = "开启视频";
                        });
                    }

                });
            }
            else
            {
                IsVideo = false;
                M.Dispatcher.Invoke(() => {
                    M.bt_StartDetect.Content = "开启视频";
                });
            }
        }


      
    }
}
