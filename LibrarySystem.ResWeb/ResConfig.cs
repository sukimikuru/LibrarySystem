using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Drawing;
using System.Web;

namespace LibrarySystem.ResWeb
{
    internal class IconInfo
    {
        public ResFileType Type { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string Default { get; set; }
    }
    internal class ResConfig
    {
        private ResConfig()
        {
            Init();
        }

        static ResConfig()
        {
        }

        private static object _locker = new object();
        private static ResConfig _current = null;
        public static ResConfig Current
        {
            get
            {
                lock (_locker)
                {
                    if (_current == null)
                    {
                        _current = new ResConfig();
                    }
                    return _current;
                }
            }
        }
        /// <summary>
        /// 上传路径
        /// </summary>
        public string UploadDir { get; private set; }
        /// <summary>
        /// 缺省相关图片所在路径
        /// </summary>
        public string DefaultDir { get; private set; }
        /// <summary>
        /// URL访问资源前缀（带“/”结束）
        /// </summary>
        public string ResUrlPrefix
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host.ToLower() + "_prefix";
                if (UrlDict.ContainsKey(host))
                {
                    return UrlDict[host];
                }
                else
                {
                    return UrlDict["_prefix"];
                }
            }
        }
        /// <summary>
        /// 缺省类型的图片URL（带“/”结束）
        /// </summary>
        public string ResUrlDefault
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host.ToLower() + "_default";
                if (UrlDict.ContainsKey(host))
                {
                    return UrlDict[host];
                }
                else
                {
                    return UrlDict["_default"];
                }
            }
        }

        internal Dictionary<string, string> UrlDict = new Dictionary<string, string>();

        public Dictionary<ResFileType, IconInfo> Icons { get; private set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Icons = new Dictionary<ResFileType, IconInfo>();
            UrlDict.Clear();
            XmlDocument xdoc = new XmlDocument();
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "res.config";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ResConfigPath"]))
            {
                xmlPath = ConfigurationManager.AppSettings["ResConfigPath"];
                if (!xmlPath.Contains(":"))
                {
                    xmlPath = xmlPath.Replace("/", "\\").Replace("~", "");
                    if (xmlPath.StartsWith("\\"))
                    {
                        xmlPath = xmlPath.Substring(1);
                    }
                    xmlPath = AppDomain.CurrentDomain.BaseDirectory + xmlPath;
                }
            }
            xdoc.Load(xmlPath);
            UploadDir = xdoc.SelectSingleNode("//configuration/UploadDir").InnerText;
            if (!UploadDir.EndsWith("\\"))
            {
                UploadDir += "\\";
            }
            DefaultDir = xdoc.SelectSingleNode("//configuration/DefaultDir").InnerText;
            if (!DefaultDir.EndsWith("\\"))
            {
                DefaultDir += "\\";
            }
            foreach (XmlNode node in xdoc.SelectNodes("//configuration/ResUrl/Url"))
            {
                string host = node.Attributes["host"].InnerText;
                string type = node.Attributes["type"].InnerText;
                string url = node.InnerText;
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }
                UrlDict.Add(host + "_" + type, url);
            }
            foreach (XmlNode node in xdoc.SelectNodes("//configuration/Icon"))
            {
                IconInfo ii = new IconInfo();
                ii.Type = (ResFileType)Enum.Parse(typeof(ResFileType), node.Attributes["name"].InnerText, true);
                ii.Width = double.Parse(node.Attributes["width"].InnerText);
                ii.Height = double.Parse(node.Attributes["height"].InnerText);
                ii.Default = node.Attributes["default"].InnerText;
                Icons.Add(ii.Type, ii);
            }
        }
    }
}
