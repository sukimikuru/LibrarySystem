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
namespace LibrarySystem.Portal.Controllers
{
    public class ResController : Controller
    {
        /// <summary>
        /// 调用数据上下文
        /// </summary>
        LibraryDC db = CommonConfig.Current.db;

        // GET: Res
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 资源列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetResList()
        {
            string result = "0";
            string html = "";
            try
            {
                List<ResEntity> resList = new List<ResEntity>();
                resList = db.AllList<ResEntity>();


                ViewData["resList"] = resList;
                html = Utility.RenderPartialViewToString(this, "ResDesignList");
                result = "1";
            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result, html } };
        }

        /// <summary>
        /// 资源详情页面数据
        /// </summary>
        /// <returns></returns>
        public JsonResult ResDetail()
        {
            string result = "0";
            string html = "";
            try
            {
                ResEntity resInfo = db.Single<ResEntity>(long.Parse(Request["row_key"]));

                if (resInfo == null)
                    resInfo = new ResEntity();

                ViewData["resInfo"] = resInfo;
                html = Utility.RenderPartialViewToString(this, "ResDesignDetail");
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
        public JsonResult SaveResInfo()
        {
            string result = "0";
            try
            {

                long row_key = 0;
                long.TryParse(Request["row_key"], out row_key);
                if (row_key == 0)
                {
                    ResEntity addRes = new ResEntity();
                    addRes.RowKey = Utility.CreateRowKey();
                    addRes.Name = Request["name"];
                    addRes.Type = Request["res_type"];
                    addRes.Note = Request["note"];

                    if (Request["res_type"] == ResType.Web.GetDBCode())
                    {
                        addRes.Path = Request["web_path"];
                    }
                    else if (!string.IsNullOrEmpty(Request["upload_infos"]))
                    {
                        addRes.Path = Request["upload_infos"];
                    }

                    addRes.Creator = addRes.Editor = Utility.GetLoginUserKey();
                    addRes.CreateTime = addRes.UpdateTime = DateTime.Now;
                    addRes.Status = true;



                    if (db.InsertEntity<ResEntity>(addRes))
                        result = "1";
                }
                else
                {
                    ResEntity editItem = db.Single<ResEntity>(row_key);
                    editItem.Name = Request["name"];
                    editItem.Type = Request["res_type"];
                    editItem.Note = Request["note"];

                    if (Request["res_type"] == ResType.Web.GetDBCode())
                    {
                        editItem.Path = Request["web_path"];
                    }
                    else if (!string.IsNullOrEmpty(Request["upload_infos"]))
                    {
                        editItem.Path = Request["upload_infos"];
                    }

                    editItem.Editor = Utility.GetLoginUserKey();
                    editItem.UpdateTime = DateTime.Now;

                    foreach (System.Data.Linq.ObjectChangeConflict occ in db.ChangeConflicts)
                    {
                        occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                    }
                    db.SubmitChanges();
                    result = "1";
                }
            }
            catch (Exception ex)
            { }
            return new JsonResult() { Data = new { result } };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteResInfo()
        {
            string result = "0";
            try
            {
                if (db.DelEntity<ResEntity>(Request["ids"]))
                    result = "1";
            }
            catch (Exception ex)
            {

            }
            return new JsonResult() { Data = new { result } };
        }
    }
}