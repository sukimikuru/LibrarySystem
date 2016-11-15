using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using LibrarySystem.DAL;
using LibrarySystem.Portal.Classes;
using LibrarySystem.Common;
using LibrarySystem.Entities;
using LibrarySystem.ResWeb;
using System.IO;

namespace LibrarySystem.Portal.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// 调用数据上下文
        /// </summary>
        LibraryDC db = CommonConfig.Current.db;

        // GET: User
        public ActionResult List()
        {
            return View();
        }


        #region 部门相关

        /// <summary>
        /// 获取树
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartJson()
        {
            string result = "0";
            string html = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            try
            {
                UserEntity userInfo = db.Single<UserEntity>(Utility.GetLoginUserKey());
                List<DepartmentEntity> departList = getAllDepartFromRoot(userInfo.DepartKey);



                ////根节点
                //{
                //    sb.Append("{");
                //    sb.AppendFormat("\"id\":\"{0}\",\"parent\":\"{1}\",\"text\":\"{2}\"", "1", "#", "图书馆");
                //    sb.Append("}");
                //    sb.Append(",");
                //}


                foreach (DepartmentEntity item in departList)
                {
                    //跟节点的parent好像只能是#，所以转一下
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",\"parent\":\"{1}\",\"text\":\"{2}\"", item.RowKey, item.RowKey == userInfo.DepartKey ? "#" : item.Parent.ToString(), item.Name);
                    sb.Append("}");
                    sb.Append(",");
                }


                if (sb[sb.Length - 1] == ',')
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");



                //foreach (DepartmentEntity item in departList)
                //{


                //}



                //sb.Append("{");
                //sb.AppendFormat("\"id\":\"{0}\",\"parent\":\"{1}\",\"text\":\"{2}\"", "1", "#", "图书馆");
                //sb.Append("}");
                //sb.Append(",");

                //sb.Append("{");
                //sb.AppendFormat("\"id\":\"{0}\",\"parent\":\"{1}\",\"text\":\"{2}\"", "2", "1", "1-1");
                //sb.Append("}");
                //sb.Append(",");


                //sb.Append("{");
                //sb.AppendFormat("\"id\":\"{0}\",\"parent\":\"{1}\",\"text\":\"{2}\"", "3", "1", "1-2");
                //sb.Append("}");
                //sb.Append(",");


                //sb.Append("{");
                //sb.AppendFormat("\"id\":\"{0}\",\"parent\":\"{1}\",\"text\":\"{2}\"", "4", "2", "2-1");
                //sb.Append("}");
                //sb.Append("]");


                html = sb.ToString();
                result = "1";
            }
            catch (Exception ex)
            {

                Utility.LogError("OSeage.CLS.Areas.COM.Controllers.DeptController->DeptListPager", "", ex.ToString());
            }
            sb.Append("]");
            return new JsonResult() { Data = new { result, html } };
        }


        public List<DepartmentEntity> getAllDepartFromRoot(long root_key)
        {
            List<DepartmentEntity> result = new List<DepartmentEntity>();

            try
            {
                result.Add(db.Single<DepartmentEntity>(root_key));
                result.AddRange(getDepartChildren(root_key));
            }
            catch
            {

            }
            return result;
        }

        public List<DepartmentEntity> getDepartChildren(long parent_key)
        {
            List<DepartmentEntity> result = new List<DepartmentEntity>();
            try
            {
                List<DepartmentEntity> temp = db.AllList<DepartmentEntity>().Where(p => p.Parent == parent_key).ToList();
                if (temp.Count > 0)
                {
                    result.AddRange(temp);
                    foreach (DepartmentEntity item in temp)
                    {
                        result.AddRange(getDepartChildren(item.RowKey));
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveDepartInfo()
        {
            string result = "0";
            string row_key_str = "";
            try
            {
                long row_key = 0, parent = 0;
                string name = "";

                long.TryParse(Request["row_key"], out row_key);
                long.TryParse(Request["parentId"], out parent);
                name = Request["name"];

                long user_key = Utility.GetLoginUserKey();

                if (row_key == 0)
                {
                    DepartmentEntity addItem = new DepartmentEntity();
                    addItem.RowKey = Utility.CreateRowKey();
                    addItem.Parent = parent;
                    addItem.Name = name;
                    addItem.Creator = addItem.Editor = user_key;
                    addItem.CreateTime = addItem.UpdateTime = DateTime.Now;
                    addItem.Status = true;

                    if (db.InsertEntity<DepartmentEntity>(addItem))
                    {
                        row_key_str = addItem.RowKey.ToString();
                        result = "1";
                    }

                }
                else
                {

                }
            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result, row_key_str } };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult DelDepartInfo()
        {
            string result = "0";
            try
            {
                long row_key = 0;
                long.TryParse(Request["row_key"], out row_key);



                if (row_key > 0)
                {
                    if (db.DelEntity<DepartmentEntity>(row_key.ToString()))
                        result = "1";
                }
                else
                {

                }
            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result } };
        }

        #endregion

        #region 用户相关

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserList()
        {
            string result = "0";
            string html = "";
            try
            {
                List<UserEntity> userList = new List<UserEntity>();
                userList = db.PageList<UserEntity>(1, Utility.PageMaxCount, (p => p.DepartKey == long.Parse(Request["depart_key"])), null);



                ViewData["userList"] = userList;
                html = Utility.RenderPartialViewToString(this, "UserDesignList");
                result = "1";
            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result, html } };
        }

        /// <summary>
        /// 用户详情页面数据
        /// </summary>
        /// <returns></returns>
        public JsonResult UserDetail()
        {
            string result = "0";
            string html = "";
            try
            {
                UserEntity userInfo = db.Single<UserEntity>(long.Parse(Request["row_key"]));

                if (userInfo == null)
                    userInfo = new UserEntity();

                ViewData["userInfo"] = userInfo;
                ViewData["type"] = Request["type"];
                html = Utility.RenderPartialViewToString(this, "UserDesignDetail");
                result = "1";
            }
            catch (Exception ex)
            {

            }
            return new JsonResult() { Data = new { result, html } };

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUserInfo()
        {
            string result = "0";

            //头像相关
            string head_img = "";
            string nick_name = "";
            HttpPostedFileBase file = null;

            //是通过右上角更新自己的信息，或者用户列表中更新自己的信息，则ajax返回时，把右上角改了
            string update_cookie = "0";

            try
            {
                long row_key = 0, depart_key = 0;
                long.TryParse(Request["row_key"], out row_key);
                long.TryParse(Request["depart_key"], out depart_key);




                if (row_key == 0)
                {
                    if (depart_key > 0)
                    {
                        //用户名查重
                        List<UserEntity> queryList = db.PageList<UserEntity>(1, Utility.PageMaxCount, (p => p.LoginName == Request["login_name"]), null);
                        if (queryList.Count > 0)
                            result = "2";
                        else
                        {
                            UserEntity addUser = new UserEntity();
                            addUser.DepartKey = depart_key;
                            addUser.RowKey = Utility.CreateRowKey();
                            addUser.Pwd = CommonConfig.Current.UserDefaultPwd;
                            addUser.LoginName = Request["login_name"];
                            nick_name = addUser.NickName = Request["nick_name"];
                            addUser.Sex = Request["sex"];
                            addUser.Role = Request["role"];
                            addUser.Phone = Request["Phone"];
                            addUser.Creator = addUser.Editor = Utility.GetLoginUserKey();
                            addUser.CreateTime = addUser.UpdateTime = DateTime.Now;
                            addUser.Status = true;

                            //头像
                            file = Request.Files["loadFile"];
                            if (file != null && file.ContentLength > 0)
                            {
                                long rowKey = Utility.CreateRowKey();
                                string filepath = RWUtility.UploadToServer(file, true);
                                head_img = addUser.HeadImg = filepath;
                            }

                            if (db.InsertEntity<UserEntity>(addUser))
                                result = "1";
                        }
                    }
                }
                else
                {
                    UserEntity editItem = db.Single<UserEntity>(row_key);
                    editItem.LoginName = Request["login_name"];
                    nick_name = editItem.NickName = Request["nick_name"];
                    editItem.Sex = Request["sex"];
                    editItem.Role = Request["role"];
                    editItem.Phone = Request["Phone"];
                    editItem.Editor = Utility.GetLoginUserKey();
                    editItem.UpdateTime = DateTime.Now;


                    //头像
                    file = Request.Files["loadFile"];
                    if (file != null && file.ContentLength > 0)
                    {
                        long rowKey = Utility.CreateRowKey();
                        string filepath = RWUtility.UploadToServer(file, true);
                        head_img = editItem.HeadImg = filepath;
                    }


                    foreach (System.Data.Linq.ObjectChangeConflict occ in db.ChangeConflicts)
                    {
                        occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                    }
                    db.SubmitChanges();

                    result = "1";
                }


                if (!string.IsNullOrEmpty(head_img) && result == "1")
                {
                    UploadImageModel model = new UploadImageModel();
                    model.headFileName = Request.Form["headFileName"].ToString();
                    model.x = Convert.ToInt32(Request["x"]);
                    model.y = Convert.ToInt32(Request["y"]);
                    if (Request["isIE"] == "1")
                    {
                        Stream stream = file.InputStream;
                        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                        model.width = Convert.ToInt32(image.Width);
                        model.height = Convert.ToInt32(image.Height);
                    }
                    else
                    {
                        model.width = Convert.ToInt32(Request["width"]);
                        model.height = Convert.ToInt32(Request["height"]);
                    }

                    RWUtility.CutAvatar(head_img, model.x, model.y, model.width, model.height, 75L, ResFileType.UserIcon120x120, 120);
                    RWUtility.CutAvatar(head_img, model.x, model.y, model.width, model.height, 75L, ResFileType.UserIcon94x94, 94);
                    RWUtility.CutAvatar(head_img, model.x, model.y, model.width, model.height, 75L, ResFileType.UserIcon48x48, 48);
                    RWUtility.CutAvatar(head_img, model.x, model.y, model.width, model.height, 75L, ResFileType.UserIcon28x28, 28);

                    if (Request["type"] == "2" || Utility.GetLoginUserKey() == row_key)
                    {
                        //更新cookie中的头像
                        Utility.UpdateCookie("head_img", head_img, Utility.LoginCookieName);
                    }


                }

                if (result == "1")
                {
                    if (Request["type"] == "2" || Utility.GetLoginUserKey() == row_key)
                    {
                        //更新cookie中的昵称
                        Utility.UpdateCookie("nick_name", nick_name, Utility.LoginCookieName);

                        update_cookie = "1";
                    }
                }

                

                head_img = RWUtility.FormatResUrl(head_img, ResFileType.UserIcon28x28);
            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result, head_img, nick_name, update_cookie } };
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public JsonResult ResetUserPwd()
        {
            string result = "0";
            try
            {
                foreach (string id in Request["ids"].Split(','))
                {
                    UserEntity editItem = db.Single<UserEntity>(long.Parse(id));
                    editItem.Pwd = CommonConfig.Current.UserDefaultPwd;

                    foreach (System.Data.Linq.ObjectChangeConflict occ in db.ChangeConflicts)
                    {
                        occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                    }
                    db.SubmitChanges();
                }

                result = "1";



            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result } };
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteUserInfo()
        {
            string result = "0";
            try
            {
                if (db.DelEntity<UserEntity>(Request["ids"]))
                    result = "1";
            }
            catch (Exception ex)
            {

            }
            return new JsonResult() { Data = new { result } };
        }

        #endregion




    }
}