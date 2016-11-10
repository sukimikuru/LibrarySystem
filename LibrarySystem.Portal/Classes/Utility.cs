using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Configuration;
using System.Reflection;
using LibrarySystem.Entities;
using System.Web.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using LibrarySystem.DAL;


namespace LibrarySystem.Portal.Classes
{
    public class Utility
    {

        private static Dictionary<long, int> _rkCache = new Dictionary<long, int>();
        private static object _locker = new object();

        /// <summary>
        /// 分页最大返回行数
        /// </summary>
        public static int PageMaxCount = 50000;




        public static SQLiteConnection GetConn()
        {
            string constr = "";
            constr = "Data Source=" + ConfigurationManager.AppSettings["DataBasePath"] + ";Version=3";
            return new SQLiteConnection(constr);
        }

        public static void LogError(string position, string input, string msg)
        { }


        /// <summary>        
        /// DateTime时间格式转换为Unix时间戳格式        
        /// </summary>        
        /// <param name=”time”></param>        
        /// <returns></returns>       
        public static long CreateRowKey()
        {
            lock (_locker)
            {
                long row_key = DateTime.Now.Ticks;
                while (_rkCache.ContainsKey(row_key))
                {
                    row_key = DateTime.Now.Ticks;
                }
                _rkCache.Add(row_key, 0);
                int maxCount = 100000;
                if (_rkCache.Count >= maxCount)
                {
                    List<long> keyList = _rkCache.Keys.ToList();
                    for (int i = keyList.Count - 1; i >= keyList.Count / 2; --i)
                    {
                        _rkCache.Remove(keyList[i]);
                    }
                    if (!_rkCache.ContainsKey(row_key))
                    {
                        _rkCache.Add(row_key, 0);
                    }
                }
                return row_key;
            }
        }



        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        public static void Redirect(HttpResponse response, string url)
        {
            try
            {
                if (!response.IsRequestBeingRedirected)
                {
                    response.Redirect(url, true);
                }
            }
            catch { }
        }
        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        public static void Redirect(HttpResponseBase response, string url)
        {
            try
            {
                if (!response.IsRequestBeingRedirected)
                {
                    response.Redirect(url, true);
                }
            }
            catch { }
        }


        #region COOKIE相关
        /// <summary>
        /// 登录COOKIE名称
        /// </summary>
        public static string LoginCookieName
        {
            get { return CommonConfig.Current.LoginCookieName; }
        }
        /// <summary>
        /// 登录COOKIE名称
        /// </summary>
        public static string RemeberMeCookieName
        {
            get { return CommonConfig.Current.RemeberMeCookieName; }
        }
        /// <summary>
        /// 获取指定COOKIE
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName];
        }
        /// <summary>
        /// 获取当前登录用户的COOKIE ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetCookieKey()
        {
            Guid g = Guid.Empty;
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                Guid.TryParse(cookie["cookie_key"], out g);
            }
            return g;
        }
        /// <summary>
        /// 删除指定COOKIE
        /// </summary>
        /// <param name="cookieName"></param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = GetCookie(cookieName);
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddHours(-1);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
        /// <summary>
        /// 创建指定COOKIE
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie CreateCookie(string cookieName)
        {
            return new HttpCookie(cookieName);
        }
        /// <summary>
        /// 延长COOKIE有效时间
        /// </summary>
        /// <param name="cookieName"></param>
        public static void UpdateLoginCookieTime()
        {
            HttpCookie cookie = GetCookie(Utility.LoginCookieName);
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(CommonConfig.Current.LoginCookieVaildTime);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
        /// <summary>
        /// 更新指定COOKIE中的指定值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cookieName"></param>
        public static void UpdateCookie(string key, string value, string cookieName)
        {
            HttpCookie cookie = GetCookie(cookieName);
            if (cookie != null)
            {
                cookie.Values.Set(key, HttpUtility.UrlEncode(value));
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
        /// <summary>
        /// 设置登录
        /// </summary>
        /// <param name="user"></param>
        public static int SetLogin(UserEntity user)
        {
            //Utility.SetUserRight(user, user.RoleKey);
            //Utility.SetUserObjKey(user);
            int result = 1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BufferOutput = true;
            //先删除Cookie
            ClearCookie(LoginCookieName);
            //再新建Cookie
            HttpCookie cookie = CreateCookie(LoginCookieName);
            cookie.Expires = DateTime.Now.AddMinutes(CommonConfig.Current.LoginCookieVaildTime);
            cookie.Values.Add("user_key", user.RowKey.ToString());
            //cookie.Values.Add("role_key", user.RoleKey.ToString());
            //cookie.Values.Add("role_kind", user.RoleKind);
            //cookie.Values.Add("areas", user.AttachPropDict["areas"]);
            //cookie.Values.Add("menus", user.AttachPropDict["menus"]);
            //cookie.Values.Add("obj_key", user.AttachPropDict["obj_key"]);
            cookie.Values.Add("nick_name", HttpUtility.UrlEncode(user.NickName));
            cookie.Values.Add("head_img", HttpUtility.UrlEncode(user.HeadImg));
            cookie.Values.Add("cookie_key", Guid.NewGuid().ToString());
            HttpContext.Current.Response.AppendCookie(cookie);

            Dictionary<long, CookieInfo> dict = HttpContext.Current.Application["CookieCache"] as Dictionary<long, CookieInfo>;
            if (!dict.ContainsKey(user.RowKey))
            {
                CookieInfo ci = new CookieInfo();
                ci.UserKey = user.RowKey;
                ci.ValidKey = Utility.GetCookieKey();
                ci.LastTime = DateTime.Now;

                HttpContext.Current.Application.Lock();
                dict[user.RowKey] = ci;
                HttpContext.Current.Application.UnLock();

                //if (dict.Count < CommonConfig.Current.MaxOnlineUser)
                //{
                //    HttpContext.Current.Application.Lock();
                //    dict[user.RowKey] = ci;
                //    HttpContext.Current.Application.UnLock();
                //}
                //else
                //{
                //    ClearCookie(LoginCookieName);
                //    result = 1006;
                //}
            }
            else
            {
                HttpContext.Current.Application.Lock();
                CookieInfo ci = dict[user.RowKey];
                ci.ValidKey = Utility.GetCookieKey();
                ci.LastTime = DateTime.Now;
                dict[user.RowKey] = ci;
                HttpContext.Current.Application.UnLock();
            }

            return result;
        }
        /// <summary>
        /// 设置"记住我"
        /// </summary>
        /// <param name="user"></param>
        public static void SetRemeberMe(long userKey)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BufferOutput = true;
            //先删除Cookie
            ClearCookie(RemeberMeCookieName);
            //再新建Cookie
            HttpCookie cookie = CreateCookie(RemeberMeCookieName);
            cookie.Expires = DateTime.Now.AddDays(CommonConfig.Current.RemeberMeCookieVaildTime);
            cookie.Values.Add("user_key", userKey.ToString());
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 获取记住我的UserKey
        /// </summary>
        /// <returns></returns>
        public static long GetRemberMeUserKey()
        {
            try
            {
                HttpCookie cookie = GetCookie(RemeberMeCookieName);
                if (cookie != null)
                {
                    return long.Parse(cookie["user_key"]);
                }
            }
            catch { }
            return 0;
        }
        /// <summary>
        /// 获取登录用户的UserKey
        /// </summary>
        /// <returns></returns>
        public static long GetLoginUserKey()
        {
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                return long.Parse(cookie["user_key"]);
            }
            return 0;
        }
        /// <summary>
        /// 获取登录用户的RoleKey
        /// </summary>
        /// <returns></returns>
        public static long GetLoginUserRoleKey()
        {
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                return long.Parse(cookie["role_key"]);
            }
            return 0;
        }
        /// <summary>
        /// 获取登录用户的PermArea(kind+area+op,....)
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserPermAreas()
        {
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                return cookie["areas"];
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取登录用户的PermArea(kind+menukey,....)
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserPermMenus()
        {
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                return cookie["menus"];
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取登录用户的扩展信息主键（如教师是teacher_key,学生是student_key)
        /// </summary>
        /// <returns></returns>
        public static long GetLoginUserObjKey()
        {
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                return long.Parse(cookie["obj_key"]);
            }
            return 0;
        }
        /// <summary>
        /// 获取登录用户的信息实体类(只有一部分：RowKey,NickName,HeadImg)
        /// </summary>
        /// <returns></returns>
        public static UserEntity GetLoginUserInfo()
        {
            UserEntity userInfo = new UserEntity();
            HttpCookie cookie = GetCookie(LoginCookieName);
            if (cookie != null)
            {
                userInfo.RowKey = long.Parse(cookie.Values["user_key"]);
                userInfo.NickName = HttpUtility.UrlDecode(cookie.Values["nick_name"]);
                userInfo.HeadImg = HttpUtility.UrlDecode(cookie.Values["head_img"]);
            }
            return userInfo;
        }


        /// <summary>
        /// 用于单点登录判断,并执行响应动作
        /// </summary>
        /// <returns></returns>
        public static bool CheckLoginStatus()
        {
            bool flag = true;
            string redirect = null;
            Dictionary<long, CookieInfo> dict = HttpContext.Current.Application["CookieCache"] as Dictionary<long, CookieInfo>;
            long userKey = Utility.GetLoginUserKey();
            if (userKey == 0)
            {
                //跳转登录页
                redirect = "/home/login";
                flag = false;
                //if (!CanAnonymous())
                //{
                //    //跳转登录页
                //    redirect = "/home/login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
                //    flag = false;
                //}
            }
            else
            {
                if (!dict.ContainsKey(userKey))
                {
                    Utility.ClearCookie(Utility.LoginCookieName);
                    //跳转登录页
                    redirect = "/home/login";
                    flag = false;
                    //if (!CanAnonymous())
                    //{
                    //    //跳转登录页
                    //    redirect = "/home/login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
                    //    flag = false;
                    //}
                }
                else
                {
                    CookieInfo ci = dict[userKey];
                    if (ci.ValidKey == Utility.GetCookieKey())
                    {
                        HttpContext.Current.Application.Lock();
                        ci.LastTime = DateTime.Now;
                        UpdateLoginCookieTime();
                        HttpContext.Current.Application.UnLock();
                    }
                    else
                    {
                        //跳转登录页
                        Utility.ClearCookie(Utility.LoginCookieName);
                        //跳转登录页
                        redirect = "/home/login";
                        flag = false;
                        //if (!CanAnonymous())
                        //{
                        //    //跳转登录页
                        //    redirect = "/home/login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
                        //    flag = false;
                        //}
                    }
                }
            }
            if (redirect != null)
            {
                Utility.Redirect(HttpContext.Current.Response, redirect);
            }
            return flag;
        }

        /// <summary>
        /// 用于单点登录判断，只判断，不执行动作
        /// </summary>
        /// <returns></returns>
        public static bool CheckLoginStatusWithOutAction(out string msg)
        {
            msg = "";
            bool flag = true;
            Dictionary<long, CookieInfo> dict = HttpContext.Current.Application["CookieCache"] as Dictionary<long, CookieInfo>;
            long userKey = Utility.GetLoginUserKey();
            if (userKey == 0)
            {
                flag = false;
                msg = "太久没操作，请重新登陆!";
            }
            else
            {
                if (!dict.ContainsKey(userKey))
                {
                    flag = false;
                    msg = "服务器不记得你了!";
                }
                else
                {
                    CookieInfo ci = dict[userKey];
                    if (ci.ValidKey != Utility.GetCookieKey())
                    {
                        flag = false;
                        msg = "帐号已在别处登陆!";
                    }
                }
            }
            
            return flag;
        }

       

        #endregion

        #region 后台渲染页面为HTML相关
        /// <summary>
        /// 渲染View并返回渲染后的HTML字符串
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName">视图文件名(全路径)</param>
        /// <param name="masterName">母板页文件名（全路径）</param>
        /// <returns></returns>
        public static string RenderViewToString(Controller controller, string viewName, string masterName)
        {
            IView view = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, masterName).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        /// <summary>
        /// 渲染PartialView并返回渲染后的HTML字符串
        /// </summary>
        /// <param name="controller">一般在Controller中使用时赋值为“this”即可</param>
        /// <param name="partialViewName">部分视图文件名（只需要部分视图的文件名称[带后缀]即可）</param>
        /// <returns></returns>
        public static string RenderPartialViewToString(Controller controller, string partialViewName)
        {
            //partialViewName = "~/Views/Shared/" + partialViewName;
            IView view = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        #endregion

        #region 加解密相关
        /// <summary>
        /// Rijndael加密（可解密）
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string RijndaelEncrypt(string sourceString)
        {
            string str = "";
            string keyStr = "oseage_cxxx_2015";
            byte[] keyIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            using (MemoryStream mStream = new MemoryStream())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(keyStr);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);
                Rijndael rij = Rijndael.Create();
                using (CryptoStream cStream = new CryptoStream(mStream, rij.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write))
                {
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    str = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return str.Replace("+", "%2B");
        }
        /// <summary>
        ///  Rijndael解密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string RijndaelDecrypt(string sourceString)
        {
            sourceString = sourceString.Replace(' ', '+');
            string keyStr = "oseage_cxxx_2015";
            byte[] keyIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            using (MemoryStream mStream = new MemoryStream())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(keyStr);
                byte[] inputByteArray = Convert.FromBase64String(sourceString);
                Rijndael rij = Rijndael.Create();
                using (CryptoStream cStream = new CryptoStream(mStream, rij.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write))
                {
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
        }
        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Unicode.GetBytes(str));
            s256.Clear();
            return Convert.ToBase64String(byte1);
        }
        /// <summary>
        /// MD5加密(用于保存密码)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5String(string str)
        {
            string stringResult = string.Empty;
            try
            {
                MD5CryptoServiceProvider md5CSP = new MD5CryptoServiceProvider();
                byte[] mdbyte = System.Text.Encoding.Unicode.GetBytes(str);
                byte[] resultEncrypt = md5CSP.ComputeHash(mdbyte);
                stringResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);
                stringResult = FormsAuthentication.HashPasswordForStoringInConfigFile(stringResult, "MD5");
            }
            catch { }
            return stringResult;
        }
        /// <summary>
        /// MD5签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string content)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
            string signature = BitConverter.ToString(output).Replace("-", "").ToUpper();
            return signature;
        }
        #endregion
    }
}