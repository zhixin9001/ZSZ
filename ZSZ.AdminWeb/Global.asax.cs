using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.AdminWeb.App_Start;
using ZSZ.Common;
using ZSZ.IService;

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
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      
      FilterConfig.RegisterConfig(GlobalFilters.Filters);
      #region Autofac

      var builder = new ContainerBuilder();
      //Register all cotrollers in this assembly
      builder.RegisterControllers(typeof(MvcApplication).Assembly)
        .PropertiesAutowired();  // the properties will be resolved automatically
                                 //Get all the relative assamblies
                                 //Assembly asm = Assembly.Load("TestService");
      Assembly[] asms = new Assembly[] { Assembly.Load("ZSZ.Service") };
      builder.RegisterAssemblyTypes(asms)
        .Where(t => !t.IsAbstract
          && typeof(IServiceSupport).IsAssignableFrom(t)
        )
        .AsImplementedInterfaces()
        .PropertiesAutowired();


      //type1.IsAssignableFrom(type2)  type1类型的变量，是否可以指向type2类型的对象
      //type2 是否实现了type1接口
      var container = builder.Build();
      //set this container was the default resolver, mvc system will get object from this container too
      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

      #endregion
    }
  }
}
