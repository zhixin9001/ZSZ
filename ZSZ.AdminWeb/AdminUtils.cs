using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb
{
  public class AdminUtils
  {
    public static long? GetUserId(HttpContextBase ctx)
    {
      return (long?)ctx.Session[SessionHelper.LOGIN_SESSION_NAME];
    }

    public static long GetCityId(HttpContextBase ctx)
    {
      long? userId = GetUserId(ctx);
      if (userId == null)
      {
        long? cityId = (long?)ctx.Session[Consts.CITY_ID];
        if (cityId.HasValue)
        {
          return cityId.Value;
        }
        else
        {
          var cityService = DependencyResolver.Current.GetService<ICityService>();
          return cityService.GetAll()[0].Id;
        }
      }
      else
      {
        var userService = DependencyResolver.Current.GetService<IUserService>();
        long? cityId = userService.GetbyId(userId.Value).CityId;
        if (!cityId.HasValue)
        {
          var cityService = DependencyResolver.Current.GetService<ICityService>();
          return cityService.GetAll()[0].Id;
        }
        else
        {
          return cityId.Value;
        }
      }
    }
  }
}