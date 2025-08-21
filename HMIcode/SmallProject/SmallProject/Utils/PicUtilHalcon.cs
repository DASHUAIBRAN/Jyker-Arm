using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmallProject.Utils
{
    public partial class PicUtil
    {
        public static HObject BitmapToHobject(Bitmap bitmap)
        {
            int height = bitmap.Height;//图像的高度
            int width = bitmap.Width;//图像的宽度


            Rectangle imgRect = new Rectangle(0, 0, width, height);
            BitmapData bitData = bitmap.LockBits(imgRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            HObject image;

            //由于Bitmap图像每行的字节数必须保持为4的倍数，因此在行的字节数不满足这个条件时，会对行进行补充，步幅数Stride表示的就是补充过后的每行的字节数，也成为扫描宽度
            int stride = bitData.Stride;
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    {
                        unsafe
                        {
                            int count = height * width;
                            byte[] data = new byte[count];
                            byte* bptr = (byte*)bitData.Scan0;
                            fixed (byte* pData = data)
                            {
                                for (int i = 0; i < height; i++)
                                {
                                    for (int j = 0; j < width; j++)
                                    {
                                        /*
                                         *
                                         如果直接使用GenImage1，传入BitData的Scan0（图像首元素的指针）作为内存指针的话，如果图像不满足行为4的倍数，那么填充的部分也会参与进来，从而导致图像扭曲
                                         *
                                         *
                                         */


                                        //舍去填充的部分
                                        data[i * width + j] = bptr[i * stride + j];
                                    }

                                }
                                HOperatorSet.GenImage1(out image, "byte", width, height, new IntPtr(pData));
                            }
                        }

                    }
                    break;
                case PixelFormat.Format24bppRgb:
                    {
                        unsafe
                        {
                            int count = height * width * 3;//24位的BitMap每个像素三个字节
                            byte[] data = new byte[count];
                            byte* bptr = (byte*)bitData.Scan0;
                            fixed (byte* pData = data)
                            {
                                for (int i = 0; i < height; i++)
                                {
                                    for (int j = 0; j < width * 3; j++)
                                    {
                                        //每个通道的像素需一一对应
                                        data[i * width * 3 + j] = bptr[i * stride + j];
                                    }
                                }
                                HOperatorSet.GenImageInterleaved(out image, new IntPtr(pData), "bgr", bitmap.Width, bitmap.Height, 0, "byte", bitmap.Width, bitmap.Height, 0, 0, -1, 0);
                            }
                        }
                    }
                    break;
                default:
                    {
                        unsafe
                        {
                            int count = height * width;
                            byte[] data = new byte[count];
                            byte* bptr = (byte*)bitData.Scan0;
                            fixed (byte* pData = data)
                            {
                                for (int i = 0; i < height; i++)
                                {
                                    for (int j = 0; j < width; j++)
                                    {
                                        data[i * width + j] = bptr[i * stride + j];
                                    }

                                }
                                HOperatorSet.GenImage1(out image, "byte", width, height, new IntPtr(pData));
                            }
                        }

                    }
                    break;
            }
            bitmap.UnlockBits(bitData);
            return image;
        }
        public static Bitmap HObject2Bitmap8(HObject image)
        {
            Bitmap res;
            HTuple hpoint, type, width, height;
            const int Alpha = 255;
            HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);
            res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = res.Palette;
            for (int i = 0; i <= 255; i++)
            { pal.Entries[i] = Color.FromArgb(Alpha, i, i, i); }

            res.Palette = pal; Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            IntPtr ptr1 = bitmapData.Scan0;
            IntPtr ptr2 = hpoint; int bytes = width * height;
            byte[] rgbvalues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, rgbvalues, 0, bytes);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues, 0, ptr1, bytes);
            res.UnlockBits(bitmapData);

            return res;
        }

        public static Bitmap HObject2Bitmap24(HObject hObject)
        {

            try
            {
                HTuple pointer, w, h;
                //下面这个语句的PNG即为HObject类型
                HOperatorSet.GetImagePointer3(hObject, out HTuple r, out HTuple g, out HTuple b, out pointer, out w, out h);

                byte[] red = new byte[w * h];
                byte[] green = new byte[w * h];
                byte[] blue = new byte[w * h];
                // 将指针指向地址的值取出来放到byte数组中
                Marshal.Copy(r, red, 0, w * h);
                Marshal.Copy(g, green, 0, w * h);
                Marshal.Copy(b, blue, 0, w * h);

                Bitmap bitmap = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Rectangle rect2 = new Rectangle(0, 0, w, h);
                BitmapData bitmapData2 = bitmap.LockBits(rect2, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                unsafe
                {
                    byte* bptr2 = (byte*)bitmapData2.Scan0;

                    Parallel.For(0, w * h, i => {
                        bptr2[i * 4] = blue[i];
                        bptr2[i * 4 + 1] = green[i];
                        bptr2[i * 4 + 2] = red[i];
                        bptr2[i * 4 + 3] = 255;
                    });
                }
                bitmap.UnlockBits(bitmapData2);
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static HObject Bitmap2HObjectBpp24(Bitmap bmp)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                HOperatorSet.GenImageInterleaved(out HObject image, srcBmpData.Scan0, "bgrx", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
