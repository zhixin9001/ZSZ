using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
  public class HomeController : Controller
  {
    public IAdminUserService _AdminService { get; set; }

    // GET: Main
    public ActionResult Index()
    {
      long? userId = SessionHelper.GetLoginId(HttpContext);
      if (!userId.HasValue)
      {
        return Redirect("~/Home/Login");
      }
      var user = _AdminService.GetById(userId.Value);

      return View(user);
    }

    [HttpGet]
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(LoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return Json(new AjaxResult { Status = "Error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
      }

      if (model.VerifyCode != TempData["verifyCode"].ToString())
      {
        return Json(new AjaxResult { Status = "Error", ErrorMsg = "VerifyCode Error" });
      }

      bool result = _AdminService.CheckLogin(model.PhoneNum, model.Password);
      if (result)
      {
        Session[SessionHelper.LOGIN_SESSION_NAME] = _AdminService.GetByPhoneNum(model.PhoneNum).Id;
        return Json(new AjaxResult { Status = "OK" });
      }
      else
      {
        return Json(new AjaxResult { Status = "Error", ErrorMsg = "Name or Password is error" });
      }
    }

    public ActionResult Logout()
    {
      Session.Abandon();
      return Redirect("~/Home/Login");
    }

    public ActionResult CreateVerifyCode()
    {
      string verifyCode = CommonHelper.GenerateCaptchaCode(4);
      TempData["verifyCode"] = verifyCode;
      //In there the ms needn't using, the mvc framework will automatically doing this for us
      MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20, 6);
      return File(ms, "image/jpeg");
    }
  }
}