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
    public ICityService _CityService { get; set; }
    public IUserService _UserService { get; set; }
    public IIdNameService _ConfigService { get; set; }
    
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
        return Json(new AjaxResult { Status = "error", Msg = MVCHelper.GetValidMsg(ModelState) });
      }

      //检查一下注册时候的手机号是不是被改掉了。防止漏洞
      //TODO
      if (!TempData[Consts.PHONE_NUM].ToString().Equals(model.PhoneNum))
      {
        return Json(new AjaxResult { Status = "error", Msg = "The phonenum has changed" });
      }

      string serverSmsCode = (string)TempData[Consts.SMS_CODE];
      if (!model.SmsCode.Equals(serverSmsCode))
      {
        return Json(new AjaxResult { Status = "error", Msg = "Sms Code Error" });
      }
      _UserService.AddNew(model.PhoneNum, model.Password);

      return Json(new AjaxResult { Status = "ok" });
    }

    public ActionResult CreateVerifyCode()
    {
      string verifyCode = CommonHelper.GenerateCaptchaCode(4);
      TempData[Consts.VERIFY_CODE] = verifyCode;
      MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 40, 100, 12, 3);
      return File(ms, "image/jpeg");
    }

    public ActionResult SendSmsVerifyCode(string phoneNum, string verifyCode)
    {
      if (!verifyCode.Equals(TempData[Consts.VERIFY_CODE].ToString()))
      {
        return Json(new AjaxResult { Status = "error", Msg = "图形验证码填写错误" });
      }
      else
      {
        string userName = _ConfigService.GetValue("短信平台UserName");
        string appKey = _ConfigService.GetValue("短信平台AppKey");
        string templateId = _ConfigService.GetValue("短信平台注册模板ID");
        string code = "6666";
        TempData[Consts.SMS_CODE] = code;

        var smsSender = new SMSSender(userName, appKey, templateId, code, phoneNum);
        var sendResult = smsSender.SendSMS();
        if (sendResult.Code.Equals("0"))
        {
          TempData[Consts.PHONE_NUM] = phoneNum;
          return Json(new AjaxResult { Status = "ok" });
        }
        else
        {
          return Json(new AjaxResult { Status = "error", Msg = "短信验证码发送失败" });
        }

      }
    }

    [HttpGet]
    public ActionResult Login()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Login(UserLoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return Json(new AjaxResult { Status = "error", Msg = MVCHelper.GetValidMsg(ModelState) });
      }
      var user = _UserService.GetByPhoneNum(model.PhoneNum);
      if (user != null)
      {
        if (_UserService.IsLocked(user.Id))
        {
          TimeSpan? leftTimeSpan = TimeSpan.FromMinutes(30) - (
                DateTime.Now - user.LastLoginErrorDateTime);

          return Json(new AjaxResult
          {
            Status = "error",
            Msg = string.Format("You has been locked, please try {0} minites later", leftTimeSpan.Value.TotalMinutes.ToString())
          });
        }
      }
      var isOk = _UserService.CheckLogin(model.PhoneNum, model.Password);
      if (!isOk)
      {
        _UserService.IncrLoginError(user.Id);
        return Json(new AjaxResult { Status = "error", Msg = "PhoneNum or Password is Error" });
      }
      else
      {
        _UserService.ResetLoginError(user.Id);
        Session[Consts.USER_ID] = user.Id;
        Session[Consts.CITY_ID] = user.CityId;
        return Json(new AjaxResult { Status = "ok" });
      }
    }

    public ActionResult SwitchCityId(long cityId)
    {
      long? userId = FrontUtils.GetUserId(HttpContext);
      if (!userId.HasValue)
      {
        Session[Consts.CITY_ID] = cityId;
      }
      else
      {
        _UserService.SetUserCityId(userId.Value, cityId);
      }
      return MVCHelper.ReturnJsonResult(AjaxResultEnum.ok);
    }

    // GET: Main
    public ActionResult Index()
    {
      long cityId = FrontUtils.GetCityId(HttpContext);
      string cityName = _CityService.GetById(cityId).Name;
      ViewBag.cityName = cityName;

      var cities = _CityService.GetAll();
      return View(cities);
    }
  }
}