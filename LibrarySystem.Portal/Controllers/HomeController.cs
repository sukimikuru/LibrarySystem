using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibrarySystem.Portal.Classes;
using LibrarySystem.Common;
using LibrarySystem.Entities;
using LibrarySystem.DAL;
using LibrarySystem.ResWeb;
using System.IO;

namespace LibrarySystem.Portal.Controllers
{
    public class HomeController : Controller
    {

        LibraryDC db = CommonConfig.Current.db;

        #region 登录相关
        public ViewResult Login()
        {
            return View();
        }

        public JsonResult LoginSys()
        {
            int result = 0;
            string msg = string.Empty;
            try
            {
                if (Request.RequestType == "POST")
                {
                    string user_name = Request["user_name"];
                    string password = Request["password"];
                    //password = BaseUtility.SHA256Encrypt(password);
                    bool isRememberMe = bool.Parse(Request["RememberMe"]);
                    result = Login(user_name, password, out msg);
                    if (result == 1)
                    {
                        //Utility.GenOpLog(OpType.Login.GetDBCode(), 0, DateTime.Now + ",登陆", "成功");
                    }
                    if (result == 1 && isRememberMe)
                    {
                        Utility.SetRemeberMe(Utility.GetLoginUserKey());
                    }
                }
                else
                {
                    msg = "非法请求。";
                }
            }
            catch (Exception ex)
            {
                Utility.LogError("OSeage.CX.Portal.Controllers.HomeController->LoginSys", Request["user_name"] + "|" + Request["password"], ex.ToString());
                result = 0;
                msg = "登录发生未知错误，请与管理员联系。";
            }
            return new JsonResult() { Data = new { result, msg } };
        }

        public void Logout()
        {
            Utility.ClearCookie(Utility.LoginCookieName);
            Utility.ClearCookie(Utility.RemeberMeCookieName);
            if (Request.UrlReferrer != null)
            {
                string returnUrl = "/home/login?ReturnUrl=" + HttpUtility.UrlEncode(Request.UrlReferrer.AbsoluteUri);

                returnUrl = "/home/login";

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Utility.Redirect(Response, returnUrl);
                }
                else
                {
                    Utility.Redirect(Response, "/home/login");
                }
            }
            else
            {
                Utility.Redirect(Response, "/home/login");
            }
        }

        private int Login(string user_name, string password, out string msg)
        {
            int result = 0;
            msg = string.Empty;
            try
            {
                UserEntity user = new UserEntity();
                user.LoginName = user_name;
                //user.Password = password;
                user.Status = true;
                //user = _userWrapp.Select(user);

                List<UserEntity> list = db.PageList<UserEntity>(1, Utility.PageMaxCount, (p => p.LoginName == user_name), null);

                if (list.Count == 0)
                {
                    msg = "不存在该用户。";
                }
                else
                {
                    if (list.Count > 1)
                    {
                        msg = "存在同名用户，请找管理员。";
                    }
                    else
                    {
                        user = list[0];
                        if (user.Role == RoleKind.Normal.GetDBCode())
                        {
                            msg = "该用户不具有后台登录权限。";
                        }
                        else
                        {
                            if (user.RowKey > 0)
                            {
                                bool fixPwd = false;
                                long remberKey = Utility.GetRemberMeUserKey();
                                if (remberKey > 0 && remberKey == user.RowKey)
                                {
                                    fixPwd = true;
                                }
                                else
                                {
                                    fixPwd = (Utility.SHA256Encrypt(password) == user.Pwd);
                                }
                                if (fixPwd)
                                {
                                    result = Utility.SetLogin(user);
                                }
                                else
                                {
                                    msg = "您输入的用户名或者密码错误，请重试。";
                                }
                            }
                            else
                            {
                                msg = "您输入的用户名或者密码错误，请重试。";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogError("OSeage.CX.Portal.Controllers.HomeController->Login", user_name + password, ex.ToString());
                msg = "登录发生未知错误，请与管理员联系。";
            }
            return result;
        }

        /// <summary>
        /// 根据用户名获取头像
        /// </summary>
        /// <returns></returns>
        public string GetUserIconPath()
        {
            //IUserInfoWrapper _UserWrapper = WCFClient.CreateClientChannel<IUserInfoWrapper>();
            //UserInfoEntity user = new UserInfoEntity();
            //try
            //{
            //    user.LoginName = Request["loginName"];
            //    user.Status = true;
            //    user = _UserWrapper.Select(user);
            //    return RWUtility.FormatResUrl(user.Headimg, ResFileType.UserIcon94x94);
            //}
            //catch (Exception ex)
            //{
            //    return "";
            //}
            return "";
        }

        /// <summary>
        /// 单点登录，检查自己是否被挤下去了
        /// </summary>
        /// <returns></returns>
        public JsonResult BeLogOut()
        {
            string result = "0";
            string msg = "";
            try
            {
                if (!Utility.CheckLoginStatusWithOutAction(out msg))
                    result = "1";
            }
            catch
            { }
            return new JsonResult() { Data = new { result, msg } };

        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePwd()
        {
            return View();
        }
        /// <summary>
        /// 保存新密码
        /// </summary>
        /// <returns></returns>
        public JsonResult SavePwd()
        {
            string result = "0";
            try
            {
                UserEntity userInfo = db.Single<UserEntity>(Utility.GetLoginUserKey());

                if (Utility.SHA256Encrypt(Request["pwd"]) != userInfo.Pwd)
                {
                    result = "2";
                }
                else
                {
                    userInfo.Pwd = Utility.SHA256Encrypt(Request["pwd1"]);

                    foreach (System.Data.Linq.ObjectChangeConflict occ in db.ChangeConflicts)
                    {
                        occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                    }
                    db.SubmitChanges();
                    result = "1";
                }
                //Utility.CheckLoginExpired(ref result);
                //UserInfoEntity userInfo = new UserInfoEntity();
                //userInfo.RowKey = Utility.GetLoginUserKey();
                //userInfo.Password = Utility.SHA256Encrypt(Request["pwd"]);
                //userInfo = _uiWrapp.Select(userInfo);
                //if (userInfo.RowKey > 0)
                //{
                //    userInfo.ClearChanged();
                //    userInfo.RowKey = userInfo.RowKey;
                //    userInfo.Password = Utility.SHA256Encrypt(Request["pwd1"]);
                //    if (_uiWrapp.Update(new List<UserInfoEntity>() { userInfo }))
                //    {
                //        result = "1";
                //    }
                //}
            }
            catch (Exception ex)
            {

            }
            return new JsonResult() { Data = new { result } };
        }
        #endregion

        #region 文件上传
        /// <summary>
        /// 把物理文件保存到服务器
        /// </summary>
        /// <returns></returns>
        public JsonResult FileUpload()
        {
            string result = "0";
            string path = string.Empty;
            try
            {
                path = RWUtility.UploadToServer(Request.Files[0], true);
                result = "1";
            }
            catch (Exception ex)
            {
               
            }
            return new JsonResult() { Data = new { result, path } };
        }
        #endregion

        #region 删除资源
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <returns></returns>
        public JsonResult DelFile()
        {
            string result = "0";
            try
            {
                string path = Request["path"];
                if (!string.IsNullOrEmpty(path) && !path.Contains(".."))
                {
                    path = RWUtility.GetResPath(path);
                    if (System.IO.File.Exists(path) && Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.Delete(Path.GetDirectoryName(path), true);
                        result = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return new JsonResult() { Data = new { result } };
        }
        #endregion
    }
}