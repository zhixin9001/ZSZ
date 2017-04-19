using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;

namespace ZSZ.AdminWeb.App_Start
{
  public class FilterConfig
  {
    public static void RegisterConfig(GlobalFilterCollection collection)
    {
      collection.Add(new ExceptionFilter());
      collection.Add(new JsonNetActionFilter());
      collection.Add(new AuthorizeFilter());
    }
  }
}