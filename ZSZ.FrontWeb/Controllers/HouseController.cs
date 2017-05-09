using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
  public class HouseController : Controller
  {
    public IHouseService _HouseService { get; set; }
    public IAttachmentService _AttachmentService { get; set; }
    public IRegionService _RegionService { get; set; }
    public IHouseAppointmentService _AppointmentService { get; set; }

    // GET: House
    public ActionResult Index(long id)
    {
      var house = _HouseService.GetById(id);
      if (house == null) return View("Error", (object)"The House isn't Exist");

      var pics = _HouseService.GetPics(id);
      var attachments = _AttachmentService.GetAttachments(id);

      var model = new HouseIndexViewModel
      {
        House = house,
        Pics = pics,
        Attachments = attachments
      };

      return View(model);
    }

    public ActionResult Search(long typeId, string keyWords, string monthRent, string orderByType, long? regionId)
    {
      long cityId = FrontUtils.GetCityId(HttpContext);

      var regions = _RegionService.GetAll(cityId);
      HouseSearchViewModel model = new HouseSearchViewModel()
      {
        Regions = regions
      };

      int? startMonthRent, endMonthRent;
      ParseMonthRent(monthRent, out startMonthRent, out endMonthRent);

      HouseSearchOptions searchOpts = new HouseSearchOptions()
      {
        CityId = cityId,
        CurrentIndex = 1,
        StartMonthRent = startMonthRent,
        EndMonthRent = endMonthRent,
        Keywords = keyWords,
      };

      switch (orderByType)
      {
        case "MonthRentAsc":
          searchOpts.OrderByType = OrderByType.MonthRentAsc;
          break;
        case "MonthRentDesc":
          searchOpts.OrderByType = OrderByType.MonthRentDesc;
          break;
        case "AreaAsc":
          searchOpts.OrderByType = OrderByType.AreaAsc;
          break;
        case "AreaDesc":
          searchOpts.OrderByType = OrderByType.AreaDesc;
          break;
      }
      searchOpts.PageSize = 10;
      searchOpts.RegionId = regionId;
      searchOpts.TypeId = typeId;

      var searchResult = _HouseService.Search(searchOpts);
      model.Houses = searchResult.Result;

      return View(model);
    }

    public void ParseMonthRent(string value, out int? startMonthRent, out int? endMonthRent)
    {
      if (string.IsNullOrEmpty(value) || !value.Contains('-'))
      {
        startMonthRent = null;
        endMonthRent = null;
        return;
      }

      string[] arrValue = value.Split('-');
      if (arrValue.Length < 2)
      {
        startMonthRent = null;
        endMonthRent = null;
        return;
      }

      string strStart = arrValue[0];
      string strEnd = arrValue[1];

      if (strStart.Equals("*"))
      {
        startMonthRent = null;
      }
      else
      {
        startMonthRent = Convert.ToInt32(strStart);
      }
      if (strEnd.Equals("*"))
      {
        endMonthRent = null;
      }
      else
      {
        endMonthRent = Convert.ToInt32(strEnd);
      }
    }

    public ActionResult Search2(long typeId, string keyWords, string monthRent, string orderByType, long? regionId)
    {
      long cityId = FrontUtils.GetCityId(HttpContext);
      var regions = _RegionService.GetAll(cityId);
      return View(regions);
    }

    public ActionResult LoadMore(long typeId, string keyWords, string monthRent, string orderByType, long? regionId, int pageIndex)
    {
      long cityId = FrontUtils.GetCityId(HttpContext);

      int? startMonthRent, endMonthRent;
      ParseMonthRent(monthRent, out startMonthRent, out endMonthRent);

      HouseSearchOptions searchOpts = new HouseSearchOptions()
      {
        CityId = cityId,
        CurrentIndex = 1,
        StartMonthRent = startMonthRent,
        EndMonthRent = endMonthRent,
        Keywords = keyWords,
      };

      switch (orderByType)
      {
        case "MonthRentAsc":
          searchOpts.OrderByType = OrderByType.MonthRentAsc;
          break;
        case "MonthRentDesc":
          searchOpts.OrderByType = OrderByType.MonthRentDesc;
          break;
        case "AreaAsc":
          searchOpts.OrderByType = OrderByType.AreaAsc;
          break;
        case "AreaDesc":
          searchOpts.OrderByType = OrderByType.AreaDesc;
          break;
      }
      searchOpts.PageSize = 10;
      searchOpts.RegionId = regionId;
      searchOpts.TypeId = typeId;

      var searchResult = _HouseService.Search(searchOpts);

      var houses = searchResult.Result;
      return Json(new AjaxResult { Status = AjaxResultEnum.ok.ToString(), Data = houses });
    }

    public ActionResult MakeAppointment(HouseMakeAppointmentModel model)
    {
      if (!ModelState.IsValid)
      {
        return MVCHelper.ReturnJsonResult(AjaxResultEnum.error, MVCHelper.GetValidMsg(ModelState));
      }

      long? userId = FrontUtils.GetUserId(HttpContext);

      _AppointmentService.AddNew(userId, model.Name, model.PhoneNum, model.HouseId, model.VisitDate);
      return MVCHelper.ReturnJsonResult(AjaxResultEnum.ok);
    }
  }
}