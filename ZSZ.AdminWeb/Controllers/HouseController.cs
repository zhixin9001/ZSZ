using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
  public class HouseController : Controller
  {
    private const int PAGE_SIZE = 10;
    public IAdminUserService _AdminUserService { get; set; }
    public IRegionService _RegionService { get; set; }
    public IIdNameService _IdNameService { get; set; }
    public IAttachmentService _AttachmentService { get; set; }
    public ICityService _CityService { get; set; }
    public IHouseService _HouseService { get; set; }

    // GET: House
    public ActionResult List(long typeId, int pageIndex = 1)
    {
      var userId = (long)HttpContext.Session["LoginUserId"];
      long? cityId = _AdminUserService.GetById(userId).CityId;
      if (!cityId.HasValue)
      {
        return View("Error", (object)"总部不能进行房源管理");
      }
      var houses = _HouseService.GetPagedData(cityId.Value, typeId, PAGE_SIZE, (pageIndex - 1) * PAGE_SIZE);
      long totalCount = _HouseService.GetTotalCount(cityId.Value, typeId);
      ViewBag.pageIndex = pageIndex;
      ViewBag.totalCount = totalCount;
      ViewBag.typeId = typeId;
      return View();
    }
    [HttpGet]
    public ActionResult Add()
    {
      var houseAddViewModel = new HouseAddViewModel
      {
        //Regions = _RegionService.GetAll()
      };
      return View();
    }
    //[HttpPost]
    //public ActionResult Add()
    //{
    //  return View();
    //}
  }
}