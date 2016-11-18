using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;
using System.IO;

namespace LibrarySystem.Client
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
        public LibraryService.UserEntity LoginUserInfo { set; get; }

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
            LoginUserInfo = new LibraryService.UserEntity();
        }



        #endregion
    }
}
