using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
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
    [HasPermission("House.List")]
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
    [HasPermission("House.Add")]
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
    [HasPermission("House.Add")]
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
    [HasPermission("House.Edit")]
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
    [HasPermission("House.Edit")]
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
    [HasPermission("House.Delete")]
    public ActionResult Delete(long id)
    {
      _HouseService.MarkDeleted(id);
      return Json(new AjaxResult { Status = "ok" });
    }
    [HasPermission("House.Delete")]
    public ActionResult BatchDelete(long[] ids)
    {
      foreach (var item in ids)
      {
        _HouseService.MarkDeleted(item);
      }
      return Json(new AjaxResult { Status = "ok" });
    }

    public ActionResult PicUpload(int houseId)
    {
      return View(houseId);
    }

    public ActionResult UploadPic(int houseId, HttpPostedFileBase file)
    {
      string md5 = CommonHelper.CalcMD5(file.InputStream);
      //file.InputStream.Position = 0;
      string suffix = Path.GetExtension(file.FileName);
      string path = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5 + suffix;
      string thumbPath = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5 + "_thumb" + suffix;

      string fullPath = HttpContext.Server.MapPath("~" + path);
      string thumbFullPath = HttpContext.Server.MapPath("~" + thumbPath);
      new FileInfo(fullPath).Directory.Create();
      //file.SaveAs(fullPath);

      ImageProcessingJob jobThumb = new ImageProcessingJob();
      jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));
      jobThumb.SaveProcessedImageToFileSystem(file.InputStream, thumbFullPath);
      //file.InputStream.Position = 0;
      ImageWatermark imgWatermark =
        new ImageWatermark(HttpContext.Server.MapPath("~/images/totop.png"));
      imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;
      imgWatermark.Alpha = 50;
      ImageProcessingJob jobNormal = new ImageProcessingJob();
      jobNormal.Filters.Add(imgWatermark);
      jobNormal.Filters.Add(new FixedResizeConstraint(400, 400));
      jobNormal.SaveProcessedImageToFileSystem(file.InputStream,fullPath);

      _HouseService.AddNewHousePic(new HousePicDTO { HouseId = houseId, Url = path, ThumbUrl = thumbPath });
      return Json(new AjaxResult { Status = "ok" });
    }
  }
}