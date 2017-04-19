using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.IService;
using ZSZ.Service.Services;

namespace ZSZ.AdminWeb.App_Start
{
  public class AuthorizeFilter : IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationContext filterContext)
    {
      HasPermissionAttribute[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(HasPermissionAttribute), false) as HasPermissionAttribute[];
      if (attrs.Length <= 0)   //If there no Permissions tags, needn't to check permission
      {
        return;
      }

      var userId = (long?)filterContext.HttpContext.Session[SessionHelper.LOGIN_SESSION_NAME];
      if (userId == null)
      {
        //filterContext.HttpContext.Response.Write("Hasn't login");     //this is not recommand
        //filterContext.Result = new ContentResult { Content = "Hasn't login" };   //after setting the filterContext.Result The Controller won't continue

        if (filterContext.HttpContext.Request.IsAjaxRequest())
        {
          var ajaxResult = new AjaxResult
          {
            Status = "redirect",
            Data = "/Main/Login",
            ErrorMsg = "Should Login",
          };

          filterContext.Result = new JsonNetResult { Data = ajaxResult };
        }
        filterContext.Result = new RedirectResult("~/Main/Login");
        return;
      }

      var auService = new AdminUserService();
      foreach (var attr in attrs)
      {
        if (!auService.HasPermission(userId.Value, attr.Permission))
        {
          filterContext.Result = new ContentResult { Content = string.Format("You don't have the permission of {0}", attr.Permission) };
          return;
        }
      }

    }
  }
}