using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.App_Start
{
  [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
  public class HasPermissionAttribute : Attribute
  {
    public string Permission { get; set; }
    public HasPermissionAttribute(string permission)
    {
      this.Permission = permission;
    }
  }
}