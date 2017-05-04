using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
  public class MainController : Controller
  {
    private const string VERIFY_CODE = "verifyCode";
    private const string SMS_CODE = "smsCode";
    public ICityService _CityService { get; set; }
    public IUserService _UserService { get; set; }

    // GET: Main
    public ActionResult Index()
    {
      return View();
    }
    [HttpGet]
    public ActionResult Register()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Register(UserRegModel model)
    {
      if (ModelState.IsValid == false)
      {
        return Json(new AjaxResult { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
      }

      //检查一下注册时候的手机号是不是被改掉了。防止漏洞
      //TODO

      string serverSmsCode = (string)TempData[SMS_CODE];
      if (!model.SmsCode.Equals(serverSmsCode))
      {
        return Json(new AjaxResult { Status = "error", ErrorMsg = "Sms Code Error" });
      }
      _UserService.AddNew(model.PhoneNum, model.Password);

      return Json(new AjaxResult { Status = "ok" });
    }

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
        TempData[SMS_CODE] = code;

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