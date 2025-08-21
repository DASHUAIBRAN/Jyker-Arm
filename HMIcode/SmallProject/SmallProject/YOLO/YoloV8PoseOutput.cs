using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace SmallProject.YOLO
{
    public class YoloV8PoseOutput
    {
        public class Prediction
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public float Confidence { get; set; }
            public int Class { get; set; }
            public Keypoint[] Keypoints { get; set; }
        }

        public class Keypoint
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Confidence { get; set; }
        }

        //加载模型并运算
        public static List<Prediction> GetPredictions(System.Drawing.Bitmap bitmap, out System.Drawing.Bitmap outImage)
        {
            // 设置目标尺寸 (如 YOLOv8 需要 640x640)
            int targetWidth = 640;
            int targetHeight = 640;
            outImage = bitmap;
            // 预处理图像
            DenseTensor<float> inputTensor = Preprocess(bitmap, targetWidth, targetHeight
                , out float scalarR, out float paddingWidth, out float paddingHeight);

            // 加载 ONNX 模型
            string modelPath = "yolov8n-pose.onnx";
            using (var session = new InferenceSession(modelPath))
            {
                // 创建模型输入
                var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("images", inputTensor)
            };

                // 运行推理
                using (var results = session.Run(inputs))
                {
                    // 获取输出
                    var outputName = session.OutputMetadata.Keys.First();
                    var outputTensor = results.First(r => r.Name == outputName).AsTensor<float>();
                    var outputArray = outputTensor.ToArray();
                    // 解析输出 (假设 numPredictions = 1 和 numKeypoints = 17)
                    int numPredictions = outputTensor.Dimensions[2];
                    int numKeypoints = (outputTensor.Dimensions[1] - 5) / 3; // 根据实际模型输出调整
                    Prediction[] predictions = ParseOutput(outputArray, numPredictions, numKeypoints, scalarR, paddingWidth, paddingHeight);

                    // 在图像上绘制检测结果
                    DrawPredictions(bitmap, predictions);

                    // 保存或显示结果图像
                    //bitmap.Save("annotated_image.jpg");

                    return predictions.ToList();
                }
            }
        }

        //原图缩放成 640*640
        private static Mat Letterbox(Mat image, out float scalarR, out float paddingWidth, out float paddingHeight
            , Size newShape = default(Size), Scalar color = default(Scalar), bool scaleup = true)
        {
            if (newShape == default(Size))
            {
                newShape = new Size(640, 640);
            }
            if (color == default(Scalar))
            {
                color = new Scalar(114, 114, 114); // 默认灰色填充
            }

            // 原图像大小
            int originalHeight = image.Height;
            int originalWidth = image.Width;

            // 缩放比例
            scalarR = Math.Min((float)newShape.Width / originalWidth, (float)newShape.Height / originalHeight);

            // 只进行下采样, 因为上采样会让图片模糊
            if (!scaleup)
            {
                scalarR = (float)Math.Min(scalarR, 1.0);
            }

            // 计算调整后的图像大小
            int newUnpadWidth = (int)Math.Round(originalWidth * scalarR);
            int newUnpadHeight = (int)Math.Round(originalHeight * scalarR);

            // 计算填充量
            paddingWidth = (newShape.Width - newUnpadWidth) / 2.0f; // width padding
            paddingHeight = (newShape.Height - newUnpadHeight) / 2.0f; // height padding

            // 调整图像大小
            Mat resizedImage = new Mat();
            Cv2.Resize(image, resizedImage, new Size(newUnpadWidth, newUnpadHeight));

            // 计算四周的填充
            int top = (int)Math.Round(paddingHeight - 0.1);
            int bottom = (int)Math.Round(paddingHeight + 0.1);
            int left = (int)Math.Round(paddingWidth - 0.1);
            int right = (int)Math.Round(paddingWidth + 0.1);

            // 添加填充
            Mat paddedImage = new Mat();
            Cv2.CopyMakeBorder(resizedImage, paddedImage, top, bottom, left, right, BorderTypes.Constant, color);

            return paddedImage;
        }

        //过滤output数据
        private static Prediction[] ParseOutput(float[] output, int numPredictions, int numKeypoints, float scalarR, float paddingWidth, float paddingHeight)
        {
            int numAttributes = 4 + 1 + (3 * numKeypoints); // 4 for bbox, 1 for confidence, 1 for class, 3 * numKeypoints for keypoints
            List<Prediction> predictions = new List<Prediction>();

            for (int i = 0; i < numPredictions; i++)
            {

                Prediction prediction = new Prediction
                {
                    X = (output[i] - paddingWidth) / scalarR,
                    Y = (output[i + 1 * numPredictions] - paddingHeight) / scalarR,
                    Width = (output[i + 2 * numPredictions]) / scalarR,
                    Height = (output[i + 3 * numPredictions]) / scalarR,
                    Confidence = output[i + 4 * numPredictions],
                    Keypoints = new Keypoint[numKeypoints]
                };

                //只要 conf>0.7 的
                if (prediction.Confidence < 0.7) continue;

                for (int j = 0; j < numKeypoints; j++)
                {
                    int keypointOffset = i + (5 + (j * 3)) * numPredictions;
                    prediction.Keypoints[j] = new Keypoint
                    {
                        X = (output[keypointOffset] - paddingWidth) / scalarR,
                        Y = (output[keypointOffset + 1 * numPredictions] - paddingHeight) / scalarR,
                        Confidence = output[keypointOffset + 2 * numPredictions]
                    };
                }

                predictions.Add(prediction);
            }
            //极大值抑制
            predictions = NonMaximumSuppression(predictions);
            return predictions.ToArray();
        }

        //在图上画上检测结果
        private static void DrawPredictions(System.Drawing.Bitmap bitmap, Prediction[] predictions)
        {
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            {
                using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 2))
                {
                    using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
                    {
                        foreach (var pred in predictions)
                        {
                            // Draw bounding box
                            g.DrawRectangle(pen, pred.X - pred.Width / 2, pred.Y - pred.Height / 2, pred.Width, pred.Height);

                            // Draw keypoints
                            foreach (var keypoint in pred.Keypoints)
                            {
                                if (keypoint.Confidence > 0.5) // Optional: Draw only confident keypoints
                                {
                                    g.FillEllipse(brush, keypoint.X - 3, keypoint.Y - 3, 6, 6);
                                }
                            }

                            // 绘制骨架连接
                            int[][] skeleton = new int[][]
                            {
                                new int[]{0, 1}, new int[]{0, 2}, new int[]{1, 3}, new int[]{2, 4},
                                new int[]{5, 6}, new int[]{5, 7}, new int[]{7, 9}, new int[]{6, 8}, new int[]{8, 10},
                                new int[]{11, 12}, new int[]{11, 13}, new int[]{13, 15},
                                new int[]{12, 14}, new int[]{ 14, 16}
                            };

                            foreach (var bone in skeleton)
                            {
                                int kpt1 = bone[0], kpt2 = bone[1];
                                float x1 = pred.Keypoints[kpt1].X;
                                float y1 = pred.Keypoints[kpt1].Y;
                                float x2 = pred.Keypoints[kpt2].X;
                                float y2 = pred.Keypoints[kpt2].Y;

                                g.DrawLine(pen, x1, y1, x2, y2);
                            }
                        }
                    }
                }
            }
        }

        //图像归一化
        private static DenseTensor<float> Preprocess(System.Drawing.Bitmap bitmap, int targetWidth, int targetHeight
            , out float scalarR, out float paddingWidth, out float paddingHeight)
        {
            // 1. 调整图像大小
            var mat = Letterbox(BitmapConverter.ToMat(bitmap), out scalarR, out paddingWidth, out paddingHeight);
            var resizedBitmap = BitmapConverter.ToBitmap(mat);
            // 2. 创建浮点数组存储图像数据 (channels, height, width)
            int channels = 3; // RGB
            var tensor = new DenseTensor<float>(new[] { 1, channels, targetHeight, targetWidth });

            // 3. 逐像素填充数组，归一化为 [0, 1]
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    System.Drawing.Color color = resizedBitmap.GetPixel(x, y);
                    tensor[0, 0, y, x] = color.B / 255f; // Blue channel
                    tensor[0, 1, y, x] = color.R / 255f; // Red channel
                    tensor[0, 2, y, x] = color.G / 255f; // Green channel

                }
            }

            return tensor;
        }

        //极大值抑制处理图像框
        private static List<Prediction> NonMaximumSuppression(List<Prediction> predictions, float iouThreshold = 0.5f)
        {
            // 按置信度降序排序
            var sortedPredictions = predictions.OrderByDescending(p => p.Confidence).ToList();
            List<Prediction> result = new List<Prediction>();

            while (sortedPredictions.Count > 0)
            {
                // 取出置信度最高的检测框
                var bestPrediction = sortedPredictions[0];
                result.Add(bestPrediction);
                sortedPredictions.RemoveAt(0);

                // 移除与当前检测框重叠度大于阈值的检测框
                sortedPredictions = sortedPredictions.Where(p => IoU(bestPrediction, p) < iouThreshold).ToList();
            }

            return result;
        }

        //计算图像相交区域
        private static float IoU(Prediction boxA, Prediction boxB)
        {
            // 计算重叠区域的坐标
            float x1 = Math.Max(boxA.X, boxB.X);
            float y1 = Math.Max(boxA.Y, boxB.Y);
            float x2 = Math.Min(boxA.X + boxA.Width, boxB.X + boxB.Width);
            float y2 = Math.Min(boxA.Y + boxA.Height, boxB.Y + boxB.Height);

            // 计算重叠区域的面积
            float interArea = Math.Max(0, x2 - x1) * Math.Max(0, y2 - y1);

            // 计算两个检测框的面积
            float boxAArea = boxA.Width * boxA.Height;
            float boxBArea = boxB.Width * boxB.Height;

            // 计算交并比（IoU）
            return interArea / (boxAArea + boxBArea - interArea);
        }


    }
}
