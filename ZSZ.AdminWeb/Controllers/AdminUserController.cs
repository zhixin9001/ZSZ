using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
  public class AdminUserController : Controller
  {
    public IAdminUserService _AuService { get; set; }
    public ICityService _CityService { get; set; }
    public IRoleService _RoleService { get; set; }

    // GET: AdminUser
    public ActionResult List()
    {
      var list = _AuService.GetAll();
      return View(list);
    }

    public ActionResult Delete(long id)
    {
      _AuService.MarkDeleted(id);
      return Json(new AjaxResult { Status = "OK" });
    }

    [HttpGet]
    public ActionResult Add()
    {
      var cities = _CityService.GetAll().ToList();
      cities.Insert(0, new DTO.CityDTO { Id = 0, Name = "总部" });
      var roles = _RoleService.GetAll();
      var model = new AdminUserAddViewModel
      {
        Cities = cities.ToArray(),
        Roles = roles
      };

      return View(model);
    }

    public ActionResult CheckPhoneNum(string phoneNum, long? userId)
    {
      var user = _AuService.GetByPhoneNum(phoneNum);
      bool isOK = false;
      //如果没有给userId，则说明是“插入”，只要检查是不是存在这个手机号
      if (userId == null)
      {
        isOK = (user == null);
      }
      else//如果有userId，则说明是修改，则要把自己排除在外
      {
        isOK = (user == null || user.Id == userId);
      }
      return Json(new AjaxResult { Status = isOK ? "ok" : "exists" });
    }

    [HttpPost]
    public ActionResult Add(AdminUserAddModel model)
    {
      if (!ModelState.IsValid)
      {
        string msg = MVCHelper.GetValidMsg(ModelState);
        return Json(new AjaxResult { Status = "error", ErrorMsg = msg });
      }

      bool isExist = _AuService.GetByPhoneNum(model.PhoneNum) != null;
      if (isExist)
      {
        return Json(new AjaxResult { Status = "error", ErrorMsg = "The phone num has been used" });
      }

      long? cityId = null;
      if (model.CityId != 0)
      {
        cityId = model.CityId;
      }

      long userId = _AuService.AddAdminUser(model.Name,
                model.PhoneNum, model.Password, model.Email, cityId);

      _RoleService.AddRoleIds(userId, model.RoleIds);
      return Json(new AjaxResult { Status = "ok" });
    }

    [HttpGet]
    public ActionResult Edit(long id)
    {
      var adminUser = _AuService.GetById(id);
      if (adminUser == null)
      {
        return View("Error", (object)"Can not find the admin user");
      }

      var cities = _CityService.GetAll().ToList();
      cities.Insert(0, new DTO.CityDTO { Id = 0, Name = "总部" });

      var roles = _RoleService.GetAll();
      var userRoles = _RoleService.GetByAdminUserId(id);

      var model = new AdminUserEditViewModel
      {
        UserRoleIds = userRoles.Select(r => r.Id).ToArray(),
        AdminUser = adminUser,
        Cities = cities.ToArray(),
        Roles = roles
      };
      return View(model);
    }

    [HttpPost]
    public ActionResult Edit(AdminUserEditModel model)
    {
      //修改了UpdateAdminUser方法的实现：当然password为空，不更新Password
      long? cityId = null;
      if (model.CityId > 0)//==0为总部
      {
        cityId = model.CityId;
      }
      _AuService.UpdateAdminUser(model.Id, model.Name,
              model.PhoneNum, model.Password, model.Email, cityId);
      _RoleService.UpdateRoleIds(model.Id, model.RoleIds);
      return Json(new AjaxResult { Status = "ok" });
    }
  }
}