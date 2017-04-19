using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
  public class RoleController : Controller
  {
    public IRoleService _RoleService { get; set; }
    public IPermissionService _PermService { get; set; }
    // GET: Role
    [HasPermission("Role.List")]
    public ActionResult List()
    {
      var roles = _RoleService.GetAll();
      return View(roles);
    }

    [HasPermission("Role.Delete")]
    public ActionResult Delete(long id)
    {
      _RoleService.MarkDeleted(id);
      return Json(new AjaxResult() { Status = "ok" });
    }

    [HasPermission("Role.Delete")]
    public ActionResult BatchDelete(long[] selectedIds)
    {
      foreach (var item in selectedIds)
      {
        _RoleService.MarkDeleted(item);
      }
      return Json(new AjaxResult() { Status = "ok" });
    }

    [HasPermission("Role.Add")]
    [HttpGet]
    public ActionResult Add()
    {
      if (!ModelState.IsValid)
      {
        return Json(new AjaxResult { Status = "Error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
      }
      var perms = _PermService.GetAll();
      return View(perms);
    }

    [HasPermission("Role.Add")]
    [HttpPost]
    public ActionResult Add(RoleAddModel model)
    {
      long roleId = _RoleService.AddNew(model.Name);
      _PermService.AddPermIds(roleId, model.PermissionIds);
      return Json(new AjaxResult() { Status = "ok" });
    }
    [HasPermission("Role.Edit")]
    [HttpGet]
    public ActionResult Edit(long id)
    {
      var role = _RoleService.GetById(id);
      var rolePerms = _PermService.GetByRoldId(id);
      var allPerms = _PermService.GetAll();
      var model = new RoleEditGetModel();
      model.Role = role;
      model.RolePerms = rolePerms;
      model.AllPerms = allPerms;
      return View(model);
    }
    [HasPermission("Role.Edit")]
    [HttpPost]
    public ActionResult Edit(RoleEditPostModel model)
    {
      _RoleService.Update(model.Id, model.Name);
      _PermService.UpdatePermIds(model.Id, model.PermissionIds);
      return Json(new AjaxResult() { Status = "ok" });
    }

  }
}