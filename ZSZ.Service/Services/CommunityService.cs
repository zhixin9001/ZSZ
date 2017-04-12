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
  public class CommunityService : ICommunityService
  {
    public CommunityDTO[] GetByRegionId(long regionId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<CommunityEntity>(ctx);
        return cs.GetAll().Include(c => c.Region).AsNoTracking().Where(c => c.RegionId == regionId).ToList().Select(c => ToDTO(c)).ToArray();
      }
    }

    private CommunityDTO ToDTO(CommunityEntity entity)
    {
      if (entity == null) return null;

      var dto = new CommunityDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        CreateDateTime = entity.CreateDateTime,
        BuiltYear = entity.BuiltYear,
        RegionId = entity.RegionId,
        Location = entity.Location,
        Traffic = entity.Traffic
      };
      return dto;
    }
  }
}
