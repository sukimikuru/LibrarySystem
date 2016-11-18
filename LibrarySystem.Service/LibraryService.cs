using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibrarySystem.DAL;
using LibrarySystem.Entities;

using System.Data.Linq;
//using LibrarySystem.Common;
using LibrarySystem.Service.Classes;


namespace LibrarySystem.Service
{
    public class LibraryService : ILibrary
    {

        LibraryDC db = new LibraryDC(Utility.GetConn());

        public bool LoginSys(string login_name, string password, out string msg, out UserEntity userInfo)
        {

            bool result = false;
            userInfo = new UserEntity();
            msg = string.Empty;
            try
            {
                if (Login(login_name, password, out msg, out userInfo) == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public bool LogOutSystem(long row_key)
        {
            return true;
        }

        private int Login(string user_name, string password, out string msg, out UserEntity userInfo)
        {
            int result = 0;
            msg = string.Empty;
            userInfo = new UserEntity();
            try
            {

                UserEntity user = new UserEntity();
                user.LoginName = user_name;
                //user.Password = password;
                user.Status = true;
                //user = _userWrapp.Select(user);

                List<UserEntity> temp = db.AllList<UserEntity>();
                //List<UserEntity> list = new List<UserEntity>();
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
                                //long remberKey = Utility.GetRemberMeUserKey();
                                long remberKey = 0;
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
                                    //result = Utility.SetLogin(user);
                                    result = 1;
                                    userInfo = user;
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

        public int add(int x, int y)
        {

            return x + y;
        }

        public List<ResEntity> ResPagerList(int pageIndex, int pageSize, Func<ResEntity, bool> wh, params string[] ob)
        {
            return db.PageList<ResEntity>(pageIndex, pageSize, wh, ob);

        }
    }
}
