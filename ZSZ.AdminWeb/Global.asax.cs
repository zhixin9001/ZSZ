using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.AdminWeb.App_Start;
using ZSZ.Common;

namespace ZSZ.AdminWeb
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      log4net.Config.XmlConfigurator.Configure();
      ModelBinders.Binders.Add(typeof(string), new TrimAndToDBC());
      ModelBinders.Binders.Add(typeof(int), new TrimAndToDBC());
      ModelBinders.Binders.Add(typeof(long), new TrimAndToDBC());
      ModelBinders.Binders.Add(typeof(double), new TrimAndToDBC());
      GlobalFilters.Filters.Add(new ExceptionFilter());
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);
    }
  }
}
