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

      string userName = _ConfigService.GetValue("短信平台UserName");
      string appKey = _ConfigService.GetValue("短信平台AppKey");
      string templateId = _ConfigService.GetValue("短信平台改密模板ID");
      string smsCode = "6666";

      SMSSender smsSender = new SMSSender(userName, appKey, templateId, smsCode, phoneNum);
      var sendResult = smsSender.SendSMS();
      if (sendResult.Code.Equals("0"))
      {
        TempData[Consts.FORGET_PWD_PHONENUM] = phoneNum;
        TempData[Consts.SMS_CODE] = smsCode;
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.ok);
      }
      else
      {
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.error, sendResult.Msg);
      }
    }
    [HttpGet]
    public ActionResult ForgetPassword2()
    {
      return View();
    }
    [HttpPost]
    public ActionResult ForgetPassword2(string smsCode)
    {
      string serverSmsCode = TempData[Consts.SMS_CODE].ToString();
      if (!smsCode.Equals(serverSmsCode))
      {
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.error, "SMS code error");
      }
      else
      {
        TempData[Consts.FORGET_PWD2_OK] = true;
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.ok);
      }
    }
    [HttpGet]
    public ActionResult ForgetPassword3()
    {
      return View();
    }
    [HttpPost]
    public ActionResult ForgetPassword3(string password)
    {
      bool? is2OK = (bool?)TempData[Consts.FORGET_PWD2_OK];
      if (is2OK!=true)
      {
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.error, "You haven'r passed the sms verify");
      }

      string phoneNum = TempData[Consts.FORGET_PWD_PHONENUM].ToString();
      var user = _UserService.GetByPhoneNum(phoneNum);
      _UserService.UpdatePwd(user.Id, password);
      return MVCHelper.ReturnJsonResult(AjaxResultEnum.ok);
    }
    [HttpGet]
    public ActionResult ForgetPassword4()
    {
      return View();
    }
  }
}