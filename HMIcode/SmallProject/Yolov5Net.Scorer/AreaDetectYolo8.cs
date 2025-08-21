using Emgu.CV;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yolov5Net.Scorer.Models;

namespace Yolov5Net.Scorer
{

    public class AreaDetectYolo8
    {

        private YoloScorer<Yolov8Model> scorer = null;

        /// <summary>
        /// 初始化并加载网络文件
        /// </summary>
        /// <param name="onnxName">weights 文件名</param>
        /// <param name="type">0: cuda, -1: cpu</param>
        public AreaDetectYolo8(string onnxName, AreaDetectType type = AreaDetectType.CPU)
        {
            string fileName = string.Format("Assets/Weights/{0}.onnx", onnxName);

            if (type == AreaDetectType.CPU)
            {
                //SessionOptions options = new SessionOptions();
                //options.AppendExecutionProvider_CPU();
                scorer = new YoloScorer<Yolov8Model>(fileName);
            }
            else if (type == AreaDetectType.GPU)
            {
                scorer = new YoloScorer<Yolov8Model>(fileName,
                    Microsoft.ML.OnnxRuntime.SessionOptions.MakeSessionOptionWithCudaProvider(0));
            }

        }


        ///// <summary>
        ///// 获取目标和坐标
        ///// </summary>
        ///// <param name="captureImg"></param>
        ///// <returns></returns>
        //public List<YoloPrediction> getPrediction(Mat captureImg)
        //{
        //    List<YoloPrediction> predictions = scorer.Predict(captureImg);
        //    return predictions;
        //}

        /// <summary>
        /// 获取目标和坐标
        /// </summary>
        /// <param name="captureImg"></param>
        /// <returns></returns>
        public List<YoloPrediction> getPrediction(Image captureImg)
        {
            List<YoloPrediction> predictions = scorer.Predictyolo8(captureImg);
            return predictions;
        }

    }

}
