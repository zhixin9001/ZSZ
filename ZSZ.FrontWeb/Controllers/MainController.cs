using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
  public class MainController : Controller
  {
    public ICityService iCityService { get; set; }
    // GET: Main
    public ActionResult Index()
    {
      //iCityService.AddNew("北京");

      return Content(Session["ssk"].ToString());
    }

    public ActionResult Index1()
    {
      //iCityService.AddNew("北京");
      Session["ssk"] = "ssk";
      return Content("OK");
    }
  }
}