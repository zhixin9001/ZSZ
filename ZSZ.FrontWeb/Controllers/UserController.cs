using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
  public class UserController : Controller
  {
    public IIdNameService _ConfigService { get; set; }
    public IUserService _UserService { get; set; }

    [HttpGet]
    public ActionResult ForgetPassword()
    {
      return View();
    }
    [HttpPost]
    public ActionResult ForgetPassword(string phoneNum, string verifyCode)
    {
      if (!verifyCode.Equals(TempData[Consts.VERIFY_CODE].ToString()))
      {
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.error, "VerifyCode error");
      }
      var user = _UserService.GetByPhoneNum(phoneNum);
      if (user == null)
      {
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.error, "Phone num not exists");
      }

      return View();
    }

    // GET: User
    public ActionResult Index()
    {
      string userName = _ConfigService.GetValue("短信平台UserName");
      string appKey = _ConfigService.GetValue("短信平台AppKey");
      string templateId = _ConfigService.GetValue("短信平台改密模板ID");
      return View();
    }
  }
}