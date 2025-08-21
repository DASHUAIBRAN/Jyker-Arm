using Emgu.CV;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov5Net.Scorer.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Yolov5Net.Scorer
{
    public enum AreaDetectType
    {
        CPU = 0,
        GPU = 1
    }

    public class AreaDetect
    {

        private YoloScorer<YoloCocoP5Model> scorer = null;

        /// <summary>
        /// 初始化并加载网络文件
        /// </summary>
        /// <param name="onnxName">weights 文件名</param>
        /// <param name="type">0: cuda, -1: cpu</param>
        public AreaDetect(string onnxName, AreaDetectType type = AreaDetectType.CPU)
        {
            string fileName = string.Format("Assets/Weights/{0}.onnx", onnxName);

            if (type == AreaDetectType.CPU)
            {
                //SessionOptions options = new SessionOptions();
                //options.AppendExecutionProvider_CPU();
                scorer = new YoloScorer<YoloCocoP5Model>(fileName);
            }
            else if (type == AreaDetectType.GPU)
            {
                scorer = new YoloScorer<YoloCocoP5Model>(fileName,
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
        public List<YoloPrediction> getPrediction(System.Drawing.Image captureImg)
        {
            List<YoloPrediction> predictions = scorer.Predict(captureImg);
            return predictions;
        }

       
    }

}
