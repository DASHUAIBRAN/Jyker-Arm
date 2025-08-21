using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov5Net.Scorer.Models.Abstract;

namespace Yolov5Net.Scorer.Models
{
    public class Yolov8Model : YoloModel
    {
        public override int Width { get; set; } = 640;
        public override int Height { get; set; } = 640;
        public override int Depth { get; set; } = 3;

        public override int Dimensions { get; set; } = 10;
        //public override int Dimensions { get; set; } = 9;

        public override int[] Strides { get; set; } = new int[] { 8, 16, 32 };

        public override int[][][] Anchors { get; set; } = new int[][][]
        {
            new int[][] { new int[] { 010, 13 }, new int[] { 016, 030 }, new int[] { 033, 023 } },
            new int[][] { new int[] { 030, 61 }, new int[] { 062, 045 }, new int[] { 059, 119 } },
            new int[][] { new int[] { 116, 90 }, new int[] { 156, 198 }, new int[] { 373, 326 } }
        };

        public override int[] Shapes { get; set; } = new int[] { 80, 40, 20 };

        public override float Confidence { get; set; } = 0.0f;
        public override float MulConfidence { get; set; } = 0.25f;
        public override float Overlap { get; set; } = 0.45f;

        //public override string[] Outputs { get; set; } = new[] { "output" };
        public override string[] Outputs { get; set; } = new[] { "output0" };

        public override List<YoloLabel> Labels { get; set; } = new List<YoloLabel>()
        {
            new YoloLabel { Id = 0, Name = "0", Title = "人" },
        new YoloLabel { Id = 1, Name = "1", Title = "自行车" },
        new YoloLabel { Id = 2, Name = "2", Title = "汽车" },
        new YoloLabel { Id = 3, Name = "3", Title = "摩托车" },
        new YoloLabel { Id = 4, Name = "4", Title = "飞机" },
        new YoloLabel { Id = 5, Name = "5", Title = "公交车" },
        new YoloLabel { Id = 6, Name = "6", Title = "火车" },
        new YoloLabel { Id = 7, Name = "7", Title = "卡车" },
        new YoloLabel { Id = 8, Name = "8", Title = "船" },
        new YoloLabel { Id = 9, Name = "9", Title = "交通灯" },
        new YoloLabel { Id = 10, Name = "10", Title = "消防栓" },
        new YoloLabel { Id = 11, Name = "11", Title = "停车标志" },
        new YoloLabel { Id = 12, Name = "12", Title = "停车计时器" },
        new YoloLabel { Id = 13, Name = "13", Title = "长椅" },
        new YoloLabel { Id = 14, Name = "14", Title = "鸟" },
        new YoloLabel { Id = 15, Name = "15", Title = "猫" },
        new YoloLabel { Id = 16, Name = "16", Title = "狗" },
        new YoloLabel { Id = 17, Name = "17", Title = "马" },
        new YoloLabel { Id = 18, Name = "18", Title = "羊" },
        new YoloLabel { Id = 19, Name = "19", Title = "牛" },
        new YoloLabel { Id = 20, Name = "20", Title = "大象" },
        new YoloLabel { Id = 21, Name = "21", Title = "熊" },
        new YoloLabel { Id = 22, Name = "22", Title = "斑马" },
        new YoloLabel { Id = 23, Name = "23", Title = "长颈鹿" },
        new YoloLabel { Id = 24, Name = "24", Title = "背包" },
        new YoloLabel { Id = 25, Name = "25", Title = "雨伞" },
        new YoloLabel { Id = 26, Name = "26", Title = "手提包" },
        new YoloLabel { Id = 27, Name = "27", Title = "领带" },
        new YoloLabel { Id = 28, Name = "28", Title = "行李箱" },
        new YoloLabel { Id = 29, Name = "29", Title = "飞盘" },
        new YoloLabel { Id = 30, Name = "30", Title = "滑雪板" },
        new YoloLabel { Id = 31, Name = "31", Title = "单板滑雪板" },
        new YoloLabel { Id = 32, Name = "32", Title = "运动球" },
        new YoloLabel { Id = 33, Name = "33", Title = "风筝" },
        new YoloLabel { Id = 34, Name = "34", Title = "棒球棒" },
        new YoloLabel { Id = 35, Name = "35", Title = "棒球手套" },
        new YoloLabel { Id = 36, Name = "36", Title = "滑板" },
        new YoloLabel { Id = 37, Name = "37", Title = "冲浪板" },
        new YoloLabel { Id = 38, Name = "38", Title = "网球拍" },
        new YoloLabel { Id = 39, Name = "39", Title = "瓶子" },
        new YoloLabel { Id = 40, Name = "40", Title = "酒杯" },
        new YoloLabel { Id = 41, Name = "41", Title = "杯子" },
        new YoloLabel { Id = 42, Name = "42", Title = "叉子" },
        new YoloLabel { Id = 43, Name = "43", Title = "刀" },
        new YoloLabel { Id = 44, Name = "44", Title = "勺子" },
        new YoloLabel { Id = 45, Name = "45", Title = "碗" },
        new YoloLabel { Id = 46, Name = "46", Title = "香蕉" },
        new YoloLabel { Id = 47, Name = "47", Title = "苹果" },
        new YoloLabel { Id = 48, Name = "48", Title = "三明治" },
        new YoloLabel { Id = 49, Name = "49", Title = "橙子" },
        new YoloLabel { Id = 50, Name = "50", Title = "西兰花" },
        new YoloLabel { Id = 51, Name = "51", Title = "胡萝卜" },
        new YoloLabel { Id = 52, Name = "52", Title = "热狗" },
        new YoloLabel { Id = 53, Name = "53", Title = "披萨" },
        new YoloLabel { Id = 54, Name = "54", Title = "甜甜圈" },
        new YoloLabel { Id = 55, Name = "55", Title = "蛋糕" },
        new YoloLabel { Id = 56, Name = "56", Title = "椅子" },
        new YoloLabel { Id = 57, Name = "57", Title = "沙发" },
        new YoloLabel { Id = 58, Name = "58", Title = "盆栽植物" },
        new YoloLabel { Id = 59, Name = "59", Title = "床" },
        new YoloLabel { Id = 60, Name = "60", Title = "餐桌" },
        new YoloLabel { Id = 61, Name = "61", Title = "马桶" },
        new YoloLabel { Id = 62, Name = "62", Title = "电视" },
        new YoloLabel { Id = 63, Name = "63", Title = "笔记本电脑" },
        new YoloLabel { Id = 64, Name = "64", Title = "鼠标" },
        new YoloLabel { Id = 65, Name = "65", Title = "遥控器" },
        new YoloLabel { Id = 66, Name = "66", Title = "键盘" },
        new YoloLabel { Id = 67, Name = "67", Title = "手机" },
        new YoloLabel { Id = 68, Name = "68", Title = "微波炉" },
        new YoloLabel { Id = 69, Name = "69", Title = "烤箱" },
        new YoloLabel { Id = 70, Name = "70", Title = "烤面包机" },
        new YoloLabel { Id = 71, Name = "71", Title = "水槽" },
        new YoloLabel { Id = 72, Name = "72", Title = "冰箱" },
        new YoloLabel { Id = 73, Name = "73", Title = "书" },
        new YoloLabel { Id = 74, Name = "74", Title = "时钟" },
        new YoloLabel { Id = 75, Name = "75", Title = "花瓶" },
        new YoloLabel { Id = 76, Name = "76", Title = "剪刀" },
        new YoloLabel { Id = 77, Name = "77", Title = "泰迪熊" },
        new YoloLabel { Id = 78, Name = "78", Title = "吹风机" },
        new YoloLabel { Id = 79, Name = "79", Title = "牙刷" }
        };

        public override bool UseDetect { get; set; } = true;


    }
}
