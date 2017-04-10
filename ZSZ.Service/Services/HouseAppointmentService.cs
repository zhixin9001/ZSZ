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
  public class HouseAppointmentService : IHouseAppointmentService
  {
    public long AddNew(long? userId, string name, string phoneNum, long houseId, DateTime visitDate)
    {
      using (var ctx = new ZszDBContext())
      {
        var entity = new HouseAppointEntity()
        {
          UserId = userId,
          Name = name,
          PhoneNum = phoneNum,
          HouseId = houseId,
          VisitDate = visitDate
        };

        ctx.HouseAppointments.Add(entity);
        ctx.SaveChanges();

        return entity.Id;
      }
    }

    public bool Follow(long adminUserId, long houseAppointmentId)
    {
      throw new NotImplementedException();
    }

    public HouseAppointmentDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseAppointEntity>(ctx);
        var house = cs.GetAll().Include(a => a.House)
          .Include(nameof(HouseAppointEntity.House) + "." + nameof(HouseEntity.Community))
          .Include(a => a.FollowAdminUser)
          .Include(nameof(HouseAppointEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
          .AsNoTracking().SingleOrDefault(a => a.Id == id);
        return ToDTO(house);
      }
    }

    public HouseAppointmentDTO[] GetPagedData(long cityId, string status, int pageSize, int currentIndex)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseAppointEntity>(ctx);
        var entities = cs.GetAll().Include(a => a.House)
        .Include(nameof(HouseAppointEntity.House) + "." + nameof(HouseEntity.Community))
        .Include(a => a.FollowAdminUser)
        .Include(nameof(HouseAppointEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
        .AsNoTracking().Where(a => a.House.Community.Region.CItyId == cityId
          && a.Status == status)
        .OrderByDescending(a => a.CreateDateTime)
        .Skip(currentIndex)
        .Take(pageSize);
        return entities.ToList().Select(a => ToDTO(a)).ToArray();
      }
    }

    public long GetTotalCount(long cityId, string status)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseAppointEntity>(ctx);
        return cs.GetAll().LongCount(a => a.House.Community.Region.CItyId == cityId
          && a.Status == status);
      }
    }

    private HouseAppointmentDTO ToDTO(HouseAppointEntity entity)
    {
      if (entity == null) return null;

      var dto = new HouseAppointmentDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        Status = entity.Status,
        CreateDateTime = entity.CreateDateTime,
        FollowAdminUserId = entity.FollowAdminUserId,
        FollowDateTime = entity.FollowDateTime,
        HouseId = entity.HouseId,
        PhoneNum = entity.PhoneNum,
        VisitDate = entity.VisitDate,
        UserId = entity.UserId,
        CommunityName = entity.House.Community.Name,
        RegionName = entity.House.Community.Region.Name
      };
      if (entity.FollowAdminUser != null)
      {
        dto.FollowAdminUserName = entity.FollowAdminUser.Name;
      }

      return dto;
    }
  }
}
