using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
  public class CityService : ICityService
  {

    public long AddNew(string cityName)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<CityEntity>(ctx);
        var isExist = cs.GetAll().Any(c => c.Name == cityName);
        if (isExist) throw new ArgumentException("the city being add has existed");
        var city = new CityEntity();
        city.Name = cityName;
        ctx.Cities.Add(city);
        ctx.SaveChanges();
        return city.Id;
      }
    }
  }
}
