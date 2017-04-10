using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;
using ZSZ.Service;
using ZSZ.DTO;
using System.Data.Entity;

namespace ZSZ.Service.Services
{
  public class CityService : ICityService
  {
    public CityDTO[] GetAll()
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<CityEntity>(ctx);
        return cs.GetAll().ToList().Select(c => ToDTO(c)).ToArray();
      }
    }

    public CityDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<CityEntity>(ctx);
        return ToDTO(cs.GetById(id));
      }
    }

    private CityDTO ToDTO(CityEntity entity)
    {
      if (entity == null) return null;

      var dto = new CityDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        CreateDateTime = entity.CreateDateTime
      };
      return dto;
    }
  }
}
