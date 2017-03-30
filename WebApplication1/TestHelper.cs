using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestIService;

namespace WebApplication1
{
  public class TestHelper
  {
    public static void Test()
    {
      IUserService iuc = DependencyResolver.Current.GetService<IUserService>();
      iuc.CheckLogin("", "");

      var container = AutofacDependencyResolver.Current.ApplicationContainer;
      using (container.BeginLifetimeScope())
      {
        var service = container.Resolve<IUserService>();
      }
    }
  }
}