using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
  public class MainController : Controller
  {
    // GET: Main
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Register()
    {
      return View();
    }

    private const string VERIFY_CODE = "verifyCode";
    public ActionResult CreateVerifyCode()
    {
      string verifyCode = CommonHelper.GenerateCaptchaCode(4);
      TempData[VERIFY_CODE] = verifyCode;
      MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 40, 100, 12, 3);
      return File(ms, "image/jpeg");
    }

    public ActionResult SendSmsVerifyCode(string phoneNum, string verifyCode)
    {
      if (!verifyCode.Equals(TempData[VERIFY_CODE].ToString()))
      {
        return Json(new AjaxResult { Status = "error", ErrorMsg = "图形验证码填写错误" });
      }
      else
      {
        string userName = "zhixin";
        string appKey = "7e4055bc468ed92ef1134d";
        string templateId = "193";
        string code = "6666";
        
        var smsSender = new SMSSender(userName, appKey, templateId, code, phoneNum);
        var sendResult = smsSender.SendSMS();
        if (sendResult.Code.Equals("0"))
        {
          return Json(new AjaxResult { Status = "ok" });
        }
        else
        {
          return Json(new AjaxResult { Status = "error", ErrorMsg = "短信验证码发送失败" });
        }

      }
    }
  }
}