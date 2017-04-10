using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service.Services
{
  public class RegionService : IRegionService
  {
    private ZszDBContext _ctx = new ZszDBContext();

    public RegionDTO[] GetAll(long cityId)
    {
      var cs = new CommonService<RegionEntity>(_ctx);
      return cs.GetAll().Include(r => r.City).AsNoTracking()
        .Where(r => r.CItyId == cityId).Select(r => ToDTO(r)).ToArray();
    }

    public RegionDTO GetById(long id)
    {
      var cs = new CommonService<RegionEntity>(_ctx);
      return ToDTO(cs.GetAll().Include(r=>r.City).AsNoTracking().SingleOrDefault(r=>r.Id==id));
    }

    private RegionDTO ToDTO(RegionEntity entity)
    {
      if (entity == null) return null;

      var dto = new RegionDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        CreateDateTime = entity.CreateDateTime,
        CityId = entity.CItyId,

      };

      if (entity.City != null)
      {
        dto.CityName = entity.City.Name;
      }

      return dto;
    }

    ~RegionService()
    {
      _ctx.Dispose();
    }
  }
}
