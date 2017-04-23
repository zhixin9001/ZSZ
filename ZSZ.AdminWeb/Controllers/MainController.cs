using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
    // GET: Main
    
    public ActionResult Index()
        {
            return View();
        }
    }
}