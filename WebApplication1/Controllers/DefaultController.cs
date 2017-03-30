using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestIService;

namespace WebApplication1.Controllers
{
  public class DefaultController : Controller
  {
    public IUserService UserService { get; set; }

    // GET: Default
    public ActionResult Index()
    {
      bool b= UserService.CheckLogin("", "");
      TestHelper.Test();
      return Content(b.ToString());
    }

    [HttpGet]
    public ActionResult Test2()
    {
      return View();
    }
    //[HttpPost]
    //public ActionResult Test2()
    //{
    //  return View();
    //}
  }
}