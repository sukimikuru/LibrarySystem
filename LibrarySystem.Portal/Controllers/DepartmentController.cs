using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using LibrarySystem.Portal.Classes;
using LibrarySystem.Common;
using LibrarySystem.DAL;
using LibrarySystem.Entities;
using System.Data.SQLite;

namespace LibrarySystem.Portal.Controllers
{
    public class DepartmentController : Controller
    {
        LibraryDC db = CommonConfig.Current.db;

        // GET: Department
        public ActionResult List()
        {

            return View();
        }

       
    }
}