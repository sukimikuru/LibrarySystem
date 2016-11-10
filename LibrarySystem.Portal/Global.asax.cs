using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LibrarySystem.Entities;
using System.Threading;

namespace LibrarySystem.Portal
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected ManualResetEvent _stopEvent = null;

        protected void Application_Start()
        {
            #region 开启登录用户记录
            Application["CookieCache"] = new Dictionary<long, CookieInfo>();
            _stopEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(AutoClearAvalidCookie));
            #endregion

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }

        /// <summary>
        /// 60秒来一次扫描，去掉过期缓存
        /// </summary>
        /// <param name="state"></param>
        protected void AutoClearAvalidCookie(object state)
        {
            while (true)
            {
                if (_stopEvent.WaitOne(6000))
                {
                    break;
                }
                Application.Lock();
                Dictionary<long, CookieInfo> dict = Application["CookieCache"] as Dictionary<long, CookieInfo>;
                List<long> clearList = new List<long>();
                DateTime dt = DateTime.Now;
                foreach (CookieInfo ci in dict.Values)
                {
                    if (dt.Subtract(ci.LastTime).TotalMinutes >= 30)
                    {
                        clearList.Add(ci.UserKey);
                    }
                }
                foreach (long userKey in clearList)
                {
                    dict.Remove(userKey);
                }
                if (clearList.Count > 0)
                {
                    //Utility.SetLoginStatus(Utility.FormatListToString<long>(clearList), "0");
                }
                Application.UnLock();

            }
        }
    }
}