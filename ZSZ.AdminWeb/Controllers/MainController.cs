﻿using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
  public class MainController : Controller
  {
    public IAdminUserService adminService { get; set; }

    // GET: Main
    public ActionResult Index()
    {
      return View();
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

      bool result = adminService.CheckLogin(model.PhoneNum, model.Password);
      if (result)
      {
        Session["LoginUserId"] = adminService.GetByPhoneNum(model.PhoneNum).Id;
        return Json(new AjaxResult { Status = "OK" });
      }
      else
      {
        return Json(new AjaxResult { Status = "Error", ErrorMsg = "Name or Password is error" });
      }
    }

    public ActionResult CreateVerifyCode()
    {
      string verifyCode = CommonHelper.GenerateCaptchaCode(4);
      TempData["verifyCode"] = verifyCode;
      //In there the ms needn't using, the mvc framework will automatically doing this for us
      MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20,6);
      return File(ms, "image/jpeg");
    }
  }
}