using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
  public class HouseAppointmentController : Controller
  {
    public IHouseAppointmentService _AppointmentService { get; set; }
    public IAdminUserService _UserService { get; set; }

    public ActionResult List()
    {
      long? userId = AdminUtils.GetUserId(HttpContext);
      if (!userId.HasValue) throw new ArgumentException("There are no user");
      long? cityId = _UserService.GetById(userId.Value).CityId;
      if (!cityId.HasValue)
      {
        return View("Error", (object)"Employee in Center can't handle the house appointment");
      }

      var apps = _AppointmentService.GetPagedData(cityId.Value, Consts.HOUSE_STATUS_UNHANDLED, 10, 1);
      return View(apps);
    }

    public ActionResult Follow(long appId)
    {
      long? userId = AdminUtils.GetUserId(HttpContext);
      if (!userId.HasValue) throw new ArgumentException("There are no user");
      bool isOK = _AppointmentService.Follow(userId.Value, appId);
      return isOK ? MVCHelper.ReturnJsonResult(AjaxResultEnum.ok) : MVCHelper.ReturnJsonResult(AjaxResultEnum.fail);
    }
  }
}