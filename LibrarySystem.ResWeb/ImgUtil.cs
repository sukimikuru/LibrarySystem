using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LibrarySystem.ResWeb
{
    public class ImgUtil
    {
        #region 正方形裁剪并缩放
        /// <summary>
        /// 正方形裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <param name="sourcePath">原图路径</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutAndZoom(string sourcePath, string savePath, int side, long quality = 100)
        {
            if (!Init(sourcePath, savePath))
                return;

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            using (Image sourceImage = Image.FromFile(sourcePath, true))
            {
                //原图宽高均小于模版，不作处理，直接保存
                if (sourceImage.Width <= side && sourceImage.Height <= side)
                {
                    sourceImage.Save(savePath, ImageFormat.Jpeg);
                }
                else
                {
                    //原始图片的宽、高
                    int initWidth = sourceImage.Width;
                    int initHeight = sourceImage.Height;
                    Image parsedImage = sourceImage;
                    int scaleSide = initWidth;
                    //非正方型先裁剪为正方型
                    if (initWidth != initHeight)
                    {
                        Rectangle fromRect;
                        //宽大于高的横图
                        if (initWidth > initHeight)
                        {
                            scaleSide = initHeight;
                            fromRect = new Rectangle((initWidth - initHeight) / 2, 0, scaleSide, scaleSide);
                        }
                        //高大于宽的竖图
                        else
                        {
                            scaleSide = initHeight;
                            fromRect = new Rectangle(0, (initHeight - initWidth) / 2, scaleSide, scaleSide);
                        }
                        Rectangle toRect = new Rectangle(0, 0, scaleSide, scaleSide);
                        parsedImage = GetScaleImage(sourceImage, toRect, fromRect);
                    }
                    using (parsedImage)
                    {
                        parsedImage.Save(savePath, ImageFormat.Jpeg);
                        sourceImage.Save(savePath, ImageFormat.Jpeg);
                        using (Image saveImage = GetScaleImage(parsedImage, new Rectangle(0, 0, side, side),
                           new Rectangle(0, 0, scaleSide, scaleSide)))
                        {
                            SaveImage(saveImage, savePath, quality);
                        }
                    }
                }
            }            
        }
        #endregion

        #region 自定义裁剪并缩放
        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="sourcePath">原图路径</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutAndZoom(string sourcePath, string savePath, int maxWidth, int maxHeight, long quality = 100)
        {
            if (!Init(sourcePath, savePath))
                return;
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            using (Image sourceImage = Image.FromFile(sourcePath, true))
            {
                //原图宽高均小于模版，不作处理，直接保存
                if (sourceImage.Width <= maxWidth && sourceImage.Height <= maxHeight)
                {
                    SaveImage(sourceImage, savePath, quality);
                }
                else
                {
                    //模版的宽高比例
                    double templateRate = (double)maxWidth / maxHeight;
                    //原图片的宽高比例
                    double initRate = (double)sourceImage.Width / sourceImage.Height;
                    //原图与模版比例相等，直接缩放
                    Image parsedImage = sourceImage;
                    //原图与模版比例不等，裁剪后缩放
                    if (templateRate != initRate)
                    {
                        //定位
                        Rectangle fromRect = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                        Rectangle toRect = new Rectangle(0, 0, 0, 0);//目标定位
                        //宽为标准进行裁剪
                        if (templateRate > initRate)
                        {
                            //裁剪源定位
                            fromRect.X = 0;
                            fromRect.Y = (int)Math.Floor((sourceImage.Height - sourceImage.Width / templateRate) / 2);
                            fromRect.Width = sourceImage.Width;
                            fromRect.Height = (int)Math.Floor(sourceImage.Width / templateRate);
                            //裁剪目标定位
                            toRect.X = 0;
                            toRect.Y = 0;
                            toRect.Width = sourceImage.Width;
                            toRect.Height = (int)Math.Floor(sourceImage.Width / templateRate);
                        }
                        //高为标准进行裁剪
                        else
                        {
                            fromRect.X = (int)Math.Floor((sourceImage.Width - sourceImage.Height * templateRate) / 2);
                            fromRect.Y = 0;
                            fromRect.Width = (int)Math.Floor(sourceImage.Height * templateRate);
                            fromRect.Height = sourceImage.Height;
                            toRect.X = 0;
                            toRect.Y = 0;
                            toRect.Width = (int)Math.Floor(sourceImage.Height * templateRate);
                            toRect.Height = sourceImage.Height;
                        }
                        parsedImage = GetScaleImage(sourceImage, toRect, fromRect);
                    }
                    using (parsedImage)
                    {
                        using(Image saveImage = GetScaleImage(sourceImage, new Rectangle(0, 0, maxWidth, maxHeight), 
                            new Rectangle(0, 0, parsedImage.Width, parsedImage.Height)))
                        {
                            SaveImage(saveImage, savePath, quality);
                        }
                    }   
                }
            }
        }
        #endregion

        #region 等比缩放
        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <param name="sourcePath">原图路径</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="watermarkText">水印文字(为""表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为""表示不使用水印)</param>
        public static void ZoomAuto(string sourcePath, string savePath, Double targetWidth, Double targetHeight, string watermarkText, string watermarkImage, long quality = 100)
        {
            if (!Init(sourcePath, savePath))
                return;
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            using (Image sourceImage = Image.FromFile(sourcePath, true))
            {
                //原图宽高均小于模版，不作处理，直接保存
                if (sourceImage.Width <= targetWidth && sourceImage.Height <= targetHeight)
                {
                    AddWaterMark(sourceImage, watermarkText, watermarkImage, new Point(sourceImage.Width / 2, sourceImage.Height / 2));
                    //保存
                    SaveImage(sourceImage, savePath, quality);
                }
                else
                {
                    //缩略图宽、高计算
                    double newWidth = sourceImage.Width;
                    double newHeight = sourceImage.Height;
                    //宽大于高或宽等于高（横图或正方）
                    if (sourceImage.Width > sourceImage.Height || sourceImage.Width == sourceImage.Height)
                    {
                        //如果宽大于模版
                        if (sourceImage.Width > targetWidth)
                        {
                            //宽按模版，高按比例缩放
                            newWidth = targetWidth;
                            newHeight = sourceImage.Height * (targetWidth / sourceImage.Width);
                        }
                    }
                    //高大于宽（竖图）
                    else
                    {
                        //如果高大于模版
                        if (sourceImage.Height > targetHeight)
                        {
                            //高按模版，宽按比例缩放
                            newHeight = targetHeight;
                            newWidth = sourceImage.Width * (targetHeight / sourceImage.Height);
                        }
                    }
                    using (Image parsedImage = GetScaleImage(sourceImage, new Rectangle(0, 0, (int)newWidth, (int)newHeight),
                        new Rectangle(0, 0, sourceImage.Width, sourceImage.Height)))
                    {
                        AddWaterMark(parsedImage, watermarkText, watermarkImage, new Point(sourceImage.Width / 2, sourceImage.Height / 2));
                        //保存
                        SaveImage(parsedImage, savePath, quality);
                    }
                }
            }
        }
        #endregion

        #region 自定义裁剪
        /// <summary>
        /// 图片自定义裁剪
        /// </summary>
        /// <param name="sourcePath">原图路径</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的裁剪位置及大小</param>
        public static void CutCustom(string sourcePath, string savePath, Rectangle fromRect, long quality = 100)
        {
            if (!Init(sourcePath, savePath))
                return;
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            using (Image sourceImage = Image.FromFile(sourcePath, true))
            {
                //直接裁剪
                using(Image saveImage = GetScaleImage(sourceImage, new Rectangle(0, 0, fromRect.Width, fromRect.Height), fromRect))
                {
                    //保存
                    SaveImage(saveImage, savePath, quality);
                }
            }
        }
        #endregion

        #region 其它
        /// <summary>
        /// 判断文件类型是否为WEB格式图片:JPG,GIF,BMP,PNG
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType)
        {
            if (contentType == "image/pjpeg" || contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/bmp" || contentType == "image/png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 裁剪图片的指定位置\大小到新图
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="toRect"></param>
        /// <param name="fromRect"></param>
        /// <returns></returns>
        private static Image GetScaleImage(Image sourceImage, Rectangle toRect, Rectangle fromRect)
        {
            Image scaleImage = new Bitmap(toRect.Width, toRect.Height);
            using (Graphics pickedG = Graphics.FromImage(scaleImage))
            {
                //设置质量
                pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                pickedG.SmoothingMode = SmoothingMode.HighQuality;
                //用指定背景色清空画布
                pickedG.Clear(Color.White);
                //画图
                pickedG.DrawImage(sourceImage, toRect, fromRect, GraphicsUnit.Pixel);
            }
            return scaleImage;
        }
        /// <summary>
        /// 用指定算法保存图片到新的位置
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="toRect"></param>
        /// <param name="fromRect"></param>
        /// <param name="savePath"></param>
        /// <param name="quality"></param>
        private static void SaveImage(Image saveImage, string savePath, long quality)
        {
            //关键质量控制
            //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo i in icis)
            {
                if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                {
                    ici = i;
                }
            }
            using (EncoderParameters ep = new EncoderParameters(1))
            {
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                saveImage.Save(savePath, ici, ep);
            }
        }
        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="watermarkText"></param>
        /// <param name="watermarkImage"></param>
        /// <param name="point"></param>
        private static void AddWaterMark(Image sourceImage, string watermarkText, string watermarkImage, Point point)
        {
            //文字水印
            if (!string.IsNullOrEmpty(watermarkText))
            {
                using (Graphics gWater = Graphics.FromImage(sourceImage))
                {
                    Font fontWater = new Font("黑体", 10);
                    Brush brushWater = new SolidBrush(Color.White);
                    gWater.DrawString(watermarkText, fontWater, brushWater, point.X, point.Y);
                }
            }

            //透明图片水印
            if (!string.IsNullOrEmpty(watermarkImage))
            {
                if (File.Exists(watermarkImage))
                {
                    //获取水印图片
                    using (Image wrImage = Image.FromFile(watermarkImage))
                    {
                        //水印绘制条件：原始图片宽高均大于或等于水印图片
                        if (sourceImage.Width >= wrImage.Width && sourceImage.Height >= wrImage.Height)
                        {
                            using (Graphics gWater = Graphics.FromImage(sourceImage))
                            {
                                //透明属性
                                ImageAttributes imgAttributes = new ImageAttributes();
                                ColorMap colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = { colorMap };
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements = { 
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage, new Rectangle(point.X, point.Y, wrImage.Width, wrImage.Height),
                                    0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 图片操作前的初始化
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private static bool Init(string sourcePath, string savePath)
        {
            bool flag = false;
            try
            {
                if (!File.Exists(sourcePath))
                {
                    //创建目录
                    string dir = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    if (File.Exists(savePath))
                        File.Delete(savePath);
                }
                flag = true;
            }
            catch
            {
            }
            return flag;
        }
        #endregion

    }
}