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
  public class PermissionController : Controller
  {
    public IPermissionService _PermSvc { get; set; }
    // GET: Permission
    public ActionResult List()
    {
      var perms = _PermSvc.GetAll();
      return View(perms);
    }

    public ActionResult Delete(long id)
    {
      _PermSvc.MarkDeleted(id);
      return RedirectToAction(nameof(List));
    }

    public ActionResult Delete1(long id)
    {
      _PermSvc.MarkDeleted(id);
      return Json(new AjaxResult() { Status = "ok" });
    }

    [HttpGet]
    public ActionResult Add()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Add(PermissionAddNewModel model)
    {
      _PermSvc.AddPermission(model.Name, model.Description);
      return Json(new AjaxResult { Status = "ok" });
    }

    [HttpGet]
    public ActionResult Edit(long id)
    {
      var perm = _PermSvc.GetById(id);
      return View(perm);
    }

    public ActionResult Edit(PermissionEditModel model)
    {
      _PermSvc.UpdatePermission(model.Id, model.Name, model.Description);

      return Json(new AjaxResult { Status = "ok" });
    }
  }
}