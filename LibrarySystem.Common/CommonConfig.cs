using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;
using System.IO;

namespace LibrarySystem.Common
{
    public class CommonConfig
    {
        #region 单一实例相关定义
        private static object _locker = new object();
        private static CommonConfig _current = null;
        public static CommonConfig Current
        {
            get
            {
                lock (_locker)
                {
                    if (_current == null)
                    {
                        _current = new CommonConfig();
                    }
                    return _current;
                }
            }
        }

        #endregion

        #region 全局配置文件中的定义

        public LibrarySystem.DAL.LibraryDC db { get; private set; }


        #region cookie和登陆相关

        /// <summary>
        /// <summary>
        /// 最大同时在线人数
        /// </summary>
        public int MaxOnlineUser { get; private set; }
        /// <summary>
        /// <summary>
        /// 登录COOKIE有效时长（分钟）
        /// </summary>
        public int LoginCookieVaildTime { get; private set; }
        /// <summary>
        /// <summary>
        /// 记住我COOKIE有效时长（天）
        /// </summary>
        public int RemeberMeCookieVaildTime { get; private set; }
        /// <summary>
        /// <summary>
        /// 登录COOKIE名称
        /// </summary>
        public string LoginCookieName { get; private set; }
        /// <summary>
        /// <summary>
        /// 记住我COOKIE名称
        /// </summary>
        public string RemeberMeCookieName { get; private set; }

        #endregion



        /// <summary>
        /// 是否开启单点登陆
        /// </summary>
        public int EnableSSO { get; private set; }

        /// <summary>
        /// 用户默认密码
        /// </summary>
        public string UserDefaultPwd { get; private set; }

        #endregion


        #region 初始化
        private CommonConfig()
        {
            Init();
        }


        private void Init()
        {
            InitFromConfig();
        }

        private void InitFromConfig()
        {
            UserDefaultPwd = Utility.SHA256Encrypt(ConfigurationManager.AppSettings["defaultPwd"]);
            LoginCookieVaildTime = int.Parse(ConfigurationManager.AppSettings["login_cookie_vaild_time"]);
            RemeberMeCookieVaildTime = int.Parse(ConfigurationManager.AppSettings["remeber_me_cookie_vaild_time"]);
            LoginCookieName = ConfigurationManager.AppSettings["login_cookie_name"];
            RemeberMeCookieName = ConfigurationManager.AppSettings["remeber_me_cookie_name"];


            db = new DAL.LibraryDC(Utility.GetConn());
        }



        #endregion
    }
}
