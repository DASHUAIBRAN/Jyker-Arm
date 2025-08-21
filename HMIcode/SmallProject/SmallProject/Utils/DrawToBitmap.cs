using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Utils
{
    public class DrawToBitmap
    {
        public static void DrawRectangle(Bitmap b, float X, float Y, float width, float height
            , int thickness = 10, string text = "", float fontSize = 50.0f)
        {
            using (Graphics grapPic = Graphics.FromImage(b))
            {

                using (Pen pen = new Pen(Color.FromArgb(255, Color.Red), thickness))
                {
                    Brush whiteBrush = new SolidBrush(Color.FromArgb(220, Color.Red)); // 画文字用

                    System.Drawing.Font font = new System.Drawing.Font("微软雅黑", fontSize, System.Drawing.FontStyle.Bold);
                    grapPic.DrawRectangle(pen, X, Y, width, height);
                    //定义字体
                    grapPic.DrawString(text, font, whiteBrush, (X + width / 2) * 1f, (Y + height / 2) * 1f);
                }

            }
        }
    }
}
