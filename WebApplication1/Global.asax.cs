﻿using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);

      var builder = new ContainerBuilder();
      //Register all cotrollers in this assembly
      builder.RegisterControllers(typeof(MvcApplication).Assembly)
        .PropertiesAutowired();  // the properties will be resilved automatically
      //Get all the relative assamblies
      Assembly asm = Assembly.Load("TestService");
      builder.RegisterAssemblyTypes(asm)
        .Where(t => !t.IsAbstract)
        .AsImplementedInterfaces();
      
      var container = builder.Build();
      //set this container was the default resolver, mvc system will get object from this container too
      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    }
  }
}
