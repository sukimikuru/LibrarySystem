using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSeage.Common.WCF;
using System.IO;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Configuration;

namespace LibrarySystem.ResWeb
{
    /// <summary>
    /// 资源操作类
    /// </summary>
    public class RWUtility
    {
        /// <summary>
        /// 获取html字符串中的第一个图片 
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string GetFirstImages(string htmlText, ResFileType rft)
        {
            string img = string.Empty;
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(htmlText);
            if (matches.Count > 0)
            {
                img = matches[0].Groups["imgUrl"].Value;
            }
            if (string.IsNullOrEmpty(img))
            {
                return ResConfig.Current.ResUrlDefault + ResConfig.Current.Icons[rft].Default;
            }
            return img;
        }
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="strHtml">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string StripHTML(string strHtml)
        {
            string[] aryReg ={
                              @"<script[^>]*?>.*?</script>",
                              @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                              @"([\r\n])[\s]+",
                              @"&(quot|#34);",
                              @"&(amp|#38);",
                              @"&(lt|#60);",
                              @"&(gt|#62);",
                              @"&(nbsp|#160);",
                              @"&(iexcl|#161);",
                              @"&(cent|#162);",
                              @"&(pound|#163);",
                              @"&(copy|#169);",
                              @"&#(\d+);",
                              @"-->",
                              @"<!--.*\n"
                             };

            string[] aryRep = {
                               "",
                               "",
                               "",
                               "\"",
                               "&",
                               "<",
                               ">",
                               " ",
                               "\xa1",//chr(161),
                               "\xa2",//chr(162),
                               "\xa3",//chr(163),
                               "\xa9",//chr(169),
                               "",
                               "\r\n",
                               ""
                              };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }
        /// <summary>
        /// 根据资源相对路径，获取资源的URL数据，带文件是否存在判断以及不存在时返回缺省值
        /// </summary>
        /// <returns></returns>
        public static string FormatResUrl(string resFile, ResFileType rft)
        {
            string url = "";
            try
            {
                if (!string.IsNullOrEmpty(resFile))
                {
                    if (ResConfig.Current.Icons.ContainsKey(rft))
                    {
                        string path = resFile.Split('/')[0] + string.Format("\\{0}x{1}\\",
                        ResConfig.Current.Icons[rft].Width, ResConfig.Current.Icons[rft].Height) +
                        resFile.Split('/')[1];
                        if (IsFileExists(path))
                        {
                            url = GetResUrl(path);
                        }
                    }
                    else
                    {
                        if (IsFileExists(resFile))
                        {
                            url = GetResUrl(resFile);
                        }
                    }
                }
                if (url.Length == 0)
                {
                    if (ResConfig.Current.Icons.ContainsKey(rft))
                    {
                        return ResConfig.Current.ResUrlDefault + ResConfig.Current.Icons[rft].Default;
                    }
                }
            }
            catch (Exception ex)
            {
                OSeage.Common.Log.Logger.Error(string.Format("[{0}]FormatResUrl Error:{1}", resFile, ex.ToString()));
            }
            return url;
        }
        /// <summary>
        /// 根据资源相对路径，获取资源的全路径
        /// </summary>
        /// <returns></returns>
        public static string GetResPath(string path)
        {
            if (path.IndexOf(":") == -1)
            {
                return ResConfig.Current.UploadDir + path.Replace("/", "\\");
            }
            else
            {
                return path;
            }
        }
        /// <summary>
        /// 根据模板相对路径，获取资模板的全路径
        /// </summary>
        /// <returns></returns>
        public static string GetTemplatePath(string path)
        {
            if (path.IndexOf(":") == -1)
            {
                return AppDomain.CurrentDomain.BaseDirectory + "_LAYOUTS\\" + path.Replace("/", "\\");
            }
            else
            {
                return path;
            }
        }
        /// <summary>
        /// 根据资源相对路径，获取资源的全路径
        /// </summary>
        /// <returns></returns>
        public static string GetAttrPath(string path)
        {
            return ResConfig.Current.UploadDir + path.Replace("/", "\\");
        }
        /// <summary>
        /// 根据类型获取缺省时显示的网址
        /// </summary>
        /// <param name="rft"></param>
        /// <returns></returns>
        public static string GetDefaultUrlByResFileType(ResFileType rft)
        {
            if (ResConfig.Current.Icons.ContainsKey(rft))
            {
                return ResConfig.Current.ResUrlDefault + ResConfig.Current.Icons[rft].Default;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 文件后缀对应的显示图片
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetFileIconWithExtension(string extension)
        {
            if (extension == ".docx") extension = ".doc";
            string path = string.Format("extension/{0}.png", extension.Substring(1));
            if (!File.Exists(ResConfig.Current.DefaultDir + path))
            {
                return ResConfig.Current.ResUrlDefault + "extension/default.png";
            }
            else
            {
                return ResConfig.Current.ResUrlDefault + path;
            }
        }
        /// <summary>
        /// 根据资源相对路径，获取资源的文件URL(不带文件是否存在判断以及缺省值）
        /// </summary>
        /// <returns></returns>
        public static string GetResUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("\\", "/").ToLower();
                if (url.StartsWith("/"))
                {
                    url = url.Substring(1);
                }
                int index = url.LastIndexOf("/") + 1;
                if (index > 0)
                {
                    url = url.Substring(0, index) + HttpUtility.UrlEncode(url.Substring(index)).Replace("+", "%20");
                }
                else
                {
                    url = HttpUtility.UrlEncode(url).Replace("+", "%20");
                }
                return ResConfig.Current.ResUrlPrefix + url;
            }
            else
            {
                return ResConfig.Current.ResUrlPrefix;
            }

        }
        /// <summary>
        /// 获取默认主机访问时的资源前缀URL
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultHostResUrl()
        {
            return ResConfig.Current.UrlDict["_prefix"];
        }

        public static List<string> GetAllPrefixResUrl()
        {
            List<string> list = new List<string>();
            foreach (string key in ResConfig.Current.UrlDict.Keys)
            {
                if (key.EndsWith("_prefix"))
                {
                    list.Add(ResConfig.Current.UrlDict[key]);
                }
            }
            return list;
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="file">含文件内容的HttpPostedFileBase</param>
        /// <param name="lengthName">是否以文件大小作为文件名</param>
        /// <returns>返回yyyyMMddHHmmssSSS+fileName</returns>
        public static string UploadToServer(HttpPostedFileBase file, bool lengthName)
        {
            if (file.ContentLength > 0)
            {
                DateTime time = DateTime.Now;
                string filePath = time.ToString("yyyyMMddHHmmss") + time.Millisecond.ToString("000") + "/";
                string path = RWUtility.GetResPath(filePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Path.GetFileName(file.FileName.Replace(" ", ""));
                if (lengthName)
                {
                    fileName = file.ContentLength + Path.GetExtension(file.FileName);
                }
                file.SaveAs(path + fileName);
                return filePath + fileName;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="file">含文件内容的HttpPostedFileBase</param>
        /// <param name="savePath">相对路径</param>
        /// <returns>相对路径(不含ResourceDir) + 文件名</returns>
        public static string CopyImageToResFiles(string file)
        {
            DateTime time = DateTime.Now;
            string filePath = time.ToString("yyyyMMddHHmmss") + time.Millisecond.ToString("000") + "/";
            string path = RWUtility.GetResPath(filePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.GetFileName(file.Replace(" ", ""));
            Bitmap map = new Bitmap(file);
            map.Save(path + fileName);
            return filePath + fileName;
        }
        /// <summary>
        /// 删除文件及文件夹
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteToFiles(string filePath)
        {
            bool flag = false;
            string path = RWUtility.GetResPath(Path.GetFileName(Path.GetDirectoryName(filePath)));
            if (Directory.Exists(path))
            {
                try {
                    Directory.Delete(path, true);
                    flag = true;
                }
                catch { }
            }
            return flag;
        }
        /// <summary>
        /// 将整个文件夹复制到目标文件夹中。
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                if (Directory.Exists(srcPath))
                {
                    // 检查目标目录是否以目录分割字符结束如果不是则添加之
                    if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                        aimPath += Path.DirectorySeparatorChar;
                    // 判断目标目录是否存在如果不存在则新建之
                    if (!Directory.Exists(aimPath))
                        Directory.CreateDirectory(aimPath);
                    // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                    // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                    string[] fileList = Directory.GetFileSystemEntries(srcPath);
                    // 遍历所有的文件和目录
                    foreach (string file in fileList)
                    {
                        // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                        if (Directory.Exists(file))
                            CopyDir(file, aimPath + Path.GetFileName(file));
                        // 否则直接Copy文件
                        else
                            File.Copy(file, aimPath + Path.GetFileName(file), true);
                    }
                }
            }
            catch
            {
               
            }
        }
        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rft"></param>
        /// <returns></returns>
        public static string PicZoomAuto(string filePath, ResFileType rft)
        {
            filePath = RWUtility.GetResPath(filePath);
            string savePath = string.Empty;
            if (ResConfig.Current.Icons.ContainsKey(rft))
            {
                IconInfo size = ResConfig.Current.Icons[rft];
                string dirPath = Path.GetDirectoryName(filePath) + string.Format("\\{0}x{1}\\", size.Width, size.Height);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                ImgUtil.ZoomAuto(filePath, dirPath + Path.GetFileName(filePath), size.Width, size.Height, null, null, 100);
                savePath = dirPath + Path.GetFileName(filePath);
            }
            return savePath;
        }
        /// <summary>
        /// 创建缩略图
        /// </summary>
        public static string CutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, ResFileType rft, int t)
        {
            try
            {
                imgSrc = RWUtility.GetResPath(imgSrc);
                string dirPath = string.Empty;
                if (ResConfig.Current.Icons.ContainsKey(rft))
                {
                    IconInfo size = ResConfig.Current.Icons[rft];
                    dirPath = Path.GetDirectoryName(imgSrc) + string.Format("\\{0}x{1}\\", size.Width, size.Height);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    Image original = Image.FromFile(imgSrc);
                    Bitmap img = new Bitmap(t, t, PixelFormat.Format24bppRgb);
                    img.MakeTransparent(img.GetPixel(0, 0));
                    img.SetResolution(72, 72);
                    using (Graphics gr = Graphics.FromImage(img))
                    {
                        if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) || original.RawFormat.Equals(ImageFormat.Bmp))
                        {
                            gr.Clear(Color.Transparent);
                        }
                        if (original.RawFormat.Equals(ImageFormat.Gif))
                        {
                            gr.Clear(Color.White);
                        }

                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.SmoothingMode = SmoothingMode.AntiAlias;
                        gr.CompositingQuality = CompositingQuality.HighQuality;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        using (var attribute = new System.Drawing.Imaging.ImageAttributes())
                        {
                            attribute.SetWrapMode(WrapMode.TileFlipXY);
                            gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                        }
                    }
                    ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    if (original.RawFormat.Equals(ImageFormat.Jpeg))
                    {
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    }
                    else
                        if (original.RawFormat.Equals(ImageFormat.Png))
                        {
                            myImageCodecInfo = GetEncoderInfo("image/png");
                        }
                        else
                            if (original.RawFormat.Equals(ImageFormat.Gif))
                            {
                                myImageCodecInfo = GetEncoderInfo("image/gif");
                            }
                            else
                                if (original.RawFormat.Equals(ImageFormat.Bmp))
                                {
                                    myImageCodecInfo = GetEncoderInfo("image/bmp");
                                }

                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, Quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    img.Save(dirPath + Path.GetFileName(imgSrc), myImageCodecInfo, myEncoderParameters);
                }
                return dirPath + Path.GetFileName(imgSrc);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 读取XPS缩略图
        /// </summary>
        /// <param name="res_key"></param>
        /// <param name="rft"></param>
        /// <returns></returns>
        public static string XpsImage(string resFile, ResFileType rft)
        {
            string url = "";
            try
            {
                if (!string.IsNullOrEmpty(resFile))
                {
                    string filePath = RWUtility.GetResPath(resFile);
                    if (System.IO.File.Exists(filePath))
                    {
                        string path = resFile.Split('/')[0] + "/thumbnail.jpg";
                        if (IsFileExists(path))
                        {
                            url = GetResUrl(path);
                        }
                    }
                }
                if (url.Length == 0)
                {
                    if (ResConfig.Current.Icons.ContainsKey(rft))
                    {
                        return ResConfig.Current.ResUrlDefault + ResConfig.Current.Icons[rft].Default;
                    }
                }
            }
            catch (Exception ex)
            {
                OSeage.Common.Log.Logger.Error(string.Format("[{0}]XpsImage Error:{1}", resFile, ex.ToString()));
            }
            return url;
        }
        /// <summary>
        /// 根据长宽自适应 按原图比例缩放 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <returns></returns>
        private static Size GetThumbnailSize(System.Drawing.Image original, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / original.Width;
            var heightScale = (double)desiredHeight / original.Height;
            var scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
            {
                Width = (int)(scale * original.Width),
                Height = (int)(scale * original.Height)
            };
        }
        /// <summary>
        /// 根据mimeType获取相应的ImageCodecInfo（对应映射）
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        /// <summary>
        /// 判断指定路径的文件是否存在
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        public static bool IsFileExists(string sourceFilePath)
        {
            return File.Exists(GetResPath(sourceFilePath));
        }
        /// <summary>
        /// 为指定网址生成两维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MemoryStream BuildQrCode(string url, string qrPath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (GetQRCode(url, ms))
                {
                    if (!string.IsNullOrEmpty(qrPath))
                    {
                        //qrPath = "~/theme/images/qrLogo.png";
                        if (qrPath.StartsWith("~/"))
                        {
                            qrPath = HttpContext.Current.Request.MapPath(qrPath);
                        }
                        return CombinImage(ms, qrPath);
                    }
                    else
                    {
                        MemoryStream result = new MemoryStream();
                        ms.Position = 0;
                        ms.CopyTo(result);
                        result.Position = 0;
                        return result;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="strContent">待编码的字符</param>
        /// <param name="stream">输出流</param>
        ///<returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>
        private static bool GetQRCode(string strContent, Stream stream)
        {
            ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M; //误差校正水平 
            string Content = strContent;//待编码内容
            QuietZoneModules QuietZones = QuietZoneModules.Two;  //空白区域 
            //实际大小为32*ModuleSize
            int ModuleSize = 12;//大小
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (encoder.TryEncode(Content, out qr))//对内容进行编码，并保存生成的矩阵
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(qr.Matrix, ImageFormat.Png, stream);
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 在生成的二维码上加图片
        /// </summary>
        /// <param name="qrPath"></param>
        /// <param name="logoPath"></param>
        private static MemoryStream CombinImage(MemoryStream ms, string logoPath)
        {
            if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
            {
                using (Image imgForce = Image.FromFile(logoPath))
                {
                    ms.Position = 0;
                    using (Image imgBack = Image.FromStream(ms))
                    {
                        if (imgBack.Width > imgForce.Width && imgBack.Height > imgForce.Height)
                        {
                            Graphics g = Graphics.FromImage(imgBack);
                            g.DrawImage(imgForce, (imgBack.Width - imgForce.Width) / 2, (imgBack.Height - imgForce.Height) / 2, imgForce.Width, imgForce.Height);
                            MemoryStream msNew = new MemoryStream();
                            imgBack.Save(msNew, ImageFormat.Png);
                            msNew.Position = 0;
                            return msNew;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// PlayOrDownloadVideo 用到
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        private static string GetEtag(FileInfo fi)
        {
            return fi.FullName;
        }
        /// <summary>
        /// 播放或者下载视频都可以调这个方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Request"></param>
        /// <param name="Response"></param>
        public static void PlayOrDownloadVideo(string path, HttpRequestBase Request, HttpResponseBase Response)
        {
            FileStream stm = null;
            try
            {
                Response.CacheControl = "public";
                FileInfo fi = new FileInfo(path);
                stm = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                var vary = HttpHeaderVariable.Init(Request);
                vary.Range.Total = fi.Length;
                if (vary.Range.End == 0 || vary.Range.End > vary.Range.Total - 1)
                {
                    vary.Range.End = vary.Range.Total - 1;
                }
                if (vary.IsContainsModify)
                {
                    if (fi.LastWriteTimeUtc <= vary.SinceModify)
                    {
                        Response.StatusCode = 304;
                        Response.Headers.Add("Etag", GetEtag(fi));
                        Response.End();
                    }
                }
                if (vary.IsContanisIfRange)
                {
                    if (GetEtag(fi) == vary.IfRangeString)
                    {
                        Response.StatusCode = 206;
                        Response.Headers.Add("Etag", GetEtag(fi));
                    }//get all
                    else
                    {
                        vary.Range.Start = 0;
                        vary.Range.End = vary.Range.Total - 1;
                    }
                }
                if (vary.IsContainsRange) Response.StatusCode = 206;
                else Response.StatusCode = 200;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AddHeader("Connection", "keep-alive");
                Response.AddHeader("Content-Length", vary.Range.Length.ToString());
                Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", vary.Range.Start, vary.Range.End, vary.Range.Total));
                Response.ContentType = "video/mp4";

                Response.Cache.SetMaxAge(TimeSpan.FromHours(1));
                Response.Cache.SetETag(GetEtag(fi));
                Response.Cache.SetExpires(DateTime.Now.AddDays(3));
                Response.Cache.SetLastModified(fi.LastWriteTime);
                stm.Position = vary.Range.Start;
                long totalLength = vary.Range.Length;
                var buffer = new byte[1024];
                int n = 1;
                while (totalLength > 0 && n > 0)
                {
                    n = stm.Read(buffer, 0, (int)Math.Min(totalLength, buffer.Length));
                    Response.OutputStream.Write(buffer, 0, n);
                    totalLength -= n;
                }
            }
            finally
            {
                if (stm != null)
                {
                    stm.Dispose();
                }
                Response.End();
            }
        }
        /// <summary>
        /// 必须要有Content-Length和Content-Disposition以及ContentType
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contentType"></param>
        /// <param name="Response"></param>
        public static void DownLoad(string filePath, string fileName, string contentType, HttpResponse Response)
        {
            //指定块大小
            long chunkSize = 204800;
            //建立一个200K的缓冲区
            byte[] buffer = new byte[chunkSize];
            //已读的字节数
            long dataToRead = 0;
            FileStream stream = null;
            try
            {
                //打开文件
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                dataToRead = stream.Length;
                //添加Http头
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20"));
                Response.AddHeader("Content-Length", dataToRead.ToString());
                while (dataToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        //防止client失去连接
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                Response.Close();
            }
        }
        /// <summary>
        /// 必须要有Content-Length和Content-Disposition以及ContentType
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contentType"></param>
        /// <param name="Response"></param>
        public static void DownLoad(string filePath, string fileName, string contentType, HttpResponseBase Response, bool deleteAfterDownload = false)
        {
            //指定块大小
            long chunkSize = 204800;
            //建立一个200K的缓冲区
            byte[] buffer = new byte[chunkSize];
            //已读的字节数
            long dataToRead = 0;
            FileStream stream = null;
            try
            {
                //打开文件
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                dataToRead = stream.Length;
                //添加Http头
                Response.ContentType = contentType;
                //解决,火狐浏览器下载时，中文名乱码
                if (HttpContext.Current.Request.Browser.Browser == "Firefox")
                {
                    //Response.AddHeader("Content-Disposition", "attachement;filename*=utf-8'zh_cn'" + fileName);
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                }
                else
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20"));
                Response.AddHeader("Content-Length", dataToRead.ToString());
                while (dataToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        //防止client失去连接
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    if (deleteAfterDownload && File.Exists(filePath))
                    {
                        string path = Path.GetDirectoryName(filePath);
                        if (path.Length > RWUtility.GetResPath("").Length)
                        {
                            try { Directory.Delete(Path.GetDirectoryName(filePath), true); }
                            catch { }
                        }
                    }
                }
                Response.Close();
            }
        }
    }
}
