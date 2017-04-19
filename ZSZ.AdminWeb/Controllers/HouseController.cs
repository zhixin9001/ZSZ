using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.DTO;
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
    public ICommunityService _CommunityService { get; set; }

    // GET: House
    public ActionResult List(long typeId, int pageIndex = 1)
    {
      var userId = SessionHelper.GetLoginId(HttpContext).Value;
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
      return View(houses);
    }
    [HttpGet]
    public ActionResult Add()
    {
      var userId = SessionHelper.GetLoginId(HttpContext).Value;
      long? cityId = _AdminUserService.GetById(userId).CityId;
      if (!cityId.HasValue)
      {
        return View("Error", (object)"总部不能进行房源管理");
      }
      var houseAddViewModel = new HouseAddViewModel
      {
        Regions = _RegionService.GetAll(cityId.Value),
        Attachments = _AttachmentService.GetAll(),
        Statuses = _IdNameService.GetAll("房屋状态"),
        DecorateStatuses = _IdNameService.GetAll("装修状态"),
        Types = _IdNameService.GetAll("房屋类别"),
        RoomTypes = _IdNameService.GetAll("户型")
      };

      return View(houseAddViewModel);
    }
    [HttpPost]
    public ActionResult Add(HouseAddModel model)
    {
      var userId = SessionHelper.GetLoginId(HttpContext).Value;
      long? cityId = _AdminUserService.GetById(userId).CityId;
      if (!cityId.HasValue)
      {
        return View("Error", (object)"总部不能进行房源管理");
      }

      if (!ModelState.IsValid)
      {
        var msg = MVCHelper.GetValidMsg(ModelState);
        return Json(new AjaxResult { Status = "error", ErrorMsg = msg });
      }

      HouseAddNewDTO dto = new HouseAddNewDTO();
      dto.Address = model.Address;
      dto.Area = model.Area;
      dto.AttachmentIds = model.AttachmentIds;
      dto.CheckInDateTime = model.CheckInDateTime;
      dto.CommunityId = model.CommunityId;
      dto.DecorateStatusId = model.DecorateStatusId;
      dto.Description = model.Description;
      dto.Direction = model.Direction;
      dto.FloorIndex = model.FloorIndex;
      dto.LookableDateTime = model.LookableDateTime;
      dto.MonthRent = model.MonthRent;
      dto.OwnerName = model.OwnerName;
      dto.OwnerPhoneNum = model.OwnerPhoneNum;
      dto.RoomTypeId = model.RoomTypeId;
      dto.StatusId = model.StatusId;
      dto.TotalFloorCount = model.TotalFloor;
      dto.TypeId = model.TypeId;

      long houseId = _HouseService.AddNew(dto);
      return Json(new AjaxResult { Status = "ok" });
    }
    
    public ActionResult LoadCommunities(long regionId)
    {
      var communities = _CommunityService.GetByRegionId(regionId);
      return Json(new AjaxResult { Status = "ok", Data = communities });
    }

    [HttpGet]
    public ActionResult Edit(long id)
    {
      var userId = SessionHelper.GetLoginId(HttpContext).Value;
      long? cityId = _AdminUserService.GetById(userId).CityId;
      if (cityId == null)
      {
        return View("Error", (object)"总部不能进行房源管理");
      }
      var house = _HouseService.GetById(id);
      HouseEditViewModel model = new HouseEditViewModel();
      model.House = house;

      var regions = _RegionService.GetAll(cityId.Value);
      var roomTypes = _IdNameService.GetAll("户型");
      var statuses = _IdNameService.GetAll("房屋状态");
      var decorateStatuses = _IdNameService.GetAll("装修状态");
      var attachments = _AttachmentService.GetAll();
      var types = _IdNameService.GetAll("房屋类别");

      model.Regions = regions;
      model.RoomTypes = roomTypes;
      model.Statuses = statuses;
      model.DecorateStatuses = decorateStatuses;
      model.Attachments = attachments;
      model.Types = types;
      return View(model);
    }

    [HttpPost]
    public ActionResult Edit(HouseEditModel model)
    {
      HouseDTO dto = new HouseDTO();
      dto.Address = model.Address;
      dto.Area = model.Area;
      dto.AttachmentIds = model.AttachmentIds;
      dto.CheckInDateTime = model.CheckInDateTime;
      //有没有感觉强硬用一些不适合的DTO，有一些没用的属性时候的迷茫？
      dto.CommunityId = model.CommunityId;
      dto.DecorateStatusId = model.DecorateStatusId;
      dto.Description = model.Description;
      dto.Direction = model.Direction;
      dto.FloorIndex = model.FloorIndex;
      dto.Id = model.Id;
      dto.LookableDateTime = model.LookableDateTime;
      dto.MonthRent = model.MonthRent;
      dto.OwnerName = model.OwnerName;
      dto.OwnerPhoneNum = model.OwnerPhoneNum;
      dto.RoomTypeId = model.RoomTypeId;
      dto.StatusId = model.StatusId;
      dto.TotalFloorCount = model.TotalFloor;
      dto.TypeId = model.TypeId;
      _HouseService.Update(dto);

      //CreateStaticPage(model.Id);//编辑房源的时候重新生成静态页面
      return Json(new AjaxResult { Status = "ok" });
    }
  }
}