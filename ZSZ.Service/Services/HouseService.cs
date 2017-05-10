using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ZSZ.Service.Services
{
  public class HouseService : IHouseService
  {
    public long AddNew(HouseDTO house)
    {
      var entity = new HouseEntity();
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AttachmentEntity>(ctx);
        var atts = cs.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
        foreach (var item in atts)
        {
          entity.Attachments.Add(item);
        }
        entity.Address = house.Address;
        entity.Area = house.Area;
        entity.CheckInDateTime = house.CheckInDateTime;
        entity.CommunityId = house.CommunityId;
        entity.DecorateStatusId = house.DecorateStatusId;
        entity.Description = house.Direction;
        entity.FloorIndex = house.FloorIndex;
        //entity.HousePics  The pic will be added separately
        entity.LookableDateTime = house.LookableDateTime;
        entity.MonthRent = house.MonthRent;
        entity.OwnerName = house.OwnerName;
        entity.OwnerPhoneNum = house.OwnerPhoneNum;
        entity.RoomTypeId = house.RoomTypeId;
        entity.StatusId = house.StatusId;
        entity.TotalFloorCount = house.TotalFloorCount;
        entity.TypeId = house.TypeId;
        ctx.Houses.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
      }
    }

    public long AddNew(HouseAddNewDTO house)
    {
      HouseEntity houseEntity = new HouseEntity
      {
        Address = house.Address,
        Area = house.Area
      };


      using (ZszDBContext ctx = new ZszDBContext())
      {
        CommonService<AttachmentEntity> attBS
            = new CommonService<AttachmentEntity>(ctx);
        //拿到house.AttachmentIds为主键的房屋配套设施
        var atts = attBS.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
        //houseEntity.Attachments = new List<AttachmentEntity>();
        foreach (var att in atts)
        {
          houseEntity.Attachments.Add(att);
        }
        houseEntity.CheckInDateTime = house.CheckInDateTime;
        houseEntity.CommunityId = house.CommunityId;
        houseEntity.DecorateStatusId = house.DecorateStatusId;
        houseEntity.Description = house.Description;
        houseEntity.Direction = house.Direction;
        houseEntity.FloorIndex = house.FloorIndex;
        //houseEntity.HousePics 新增后再单独添加
        houseEntity.LookableDateTime = house.LookableDateTime;
        houseEntity.MonthRent = house.MonthRent;
        houseEntity.OwnerName = house.OwnerName;
        houseEntity.OwnerPhoneNum = house.OwnerPhoneNum;
        houseEntity.RoomTypeId = house.RoomTypeId;
        houseEntity.StatusId = house.StatusId;
        houseEntity.TotalFloorCount = house.TotalFloorCount;
        houseEntity.TypeId = house.TypeId;
        ctx.Houses.Add(houseEntity);
        ctx.SaveChanges();
        return houseEntity.Id;
      }
    }

    public long AddNewHousePic(HousePicDTO housePic)
    {
      using (var ctx = new ZszDBContext())
      {
        var entity = new HousePicEntity()
        {
          HouseId = housePic.HouseId,
          ThumbUrl = housePic.ThumbUrl,
          Url = housePic.Url
        };

        ctx.HousePics.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
      }
    }

    public void DeleteHousePic(long housePicId)
    {
      using (var ctx = new ZszDBContext())
      {
        var entity = ctx.HousePics.SingleOrDefault(s => s.IsDeleted != true && s.Id == housePicId);
        if (entity != null)
        {
          ctx.HousePics.Remove(entity);
          ctx.SaveChanges();
        }
      }
    }

    public HouseDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        var house = cs.GetAll()
          .Include(h => h.Attachments)
          .Include(h => h.Community)
          .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
          .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
          .Include(h => h.DecorateStatus)
          .Include(h => h.HousePics)
          .Include(h => h.RoomType)
          .Include(h => h.Status)
          .Include(h => h.Type)
          .SingleOrDefault(h => h.Id == id);
        return ToDTO(house);
      }
    }

    public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        return cs.GetAll().LongCount(h => h.Community.Region.CItyId == cityId
          && h.CreateDateTime >= startDateTime
          && h.CreateDateTime < endDateTime);
      }
    }

    public HouseDTO[] GetPagedData(long cityId, long typeId, int pageSize, int currentIndex)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        var houses = cs.GetAll()
          .Include(h => h.Attachments)
          .Include(h => h.Community)
          .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
          .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
          .Include(h => h.DecorateStatus)
          .Include(h => h.HousePics)
          .Include(h => h.RoomType)
          .Include(h => h.Status)
          .Include(h => h.Type)
          .Where(h => h.Community.Region.CItyId == cityId && h.TypeId == typeId)
          .OrderByDescending(h => h.CreateDateTime)
          .Skip(currentIndex)
          .Take(pageSize);

        return houses.ToList().Select(h => ToDTO(h)).ToArray();
      }
    }

    public HousePicDTO[] GetPics(long houseId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        var house = cs.GetById(houseId);
        var housePics = house.HousePics;
        return housePics.Select(p => new HousePicDTO()
        {
          Id = p.Id,
          HouseId = p.HouseId,
          CreateDateTime = p.CreateDateTime,
          ThumbUrl = p.ThumbUrl,
          Url = p.Url
        }).ToArray();
      }
    }

    public long GetTotalCount(long cityId, long typeId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        return cs.GetAll()
          .LongCount(h => h.Community.Region.CItyId == cityId
            && h.TypeId == typeId);
      }
    }

    public void MarkDeleted(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        cs.MarkDeleted(id);
      }
    }

    public void Update(HouseDTO house)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        var entity = cs.GetById(house.Id);
        entity.Address = house.Address;
        entity.Area = house.Area;
        entity.CheckInDateTime = house.CheckInDateTime;
        entity.CommunityId = house.CommunityId;
        entity.DecorateStatusId = house.DecorateStatusId;
        entity.Description = house.Description;
        entity.Direction = house.Direction;
        entity.FloorIndex = house.FloorIndex;
        entity.LookableDateTime = house.LookableDateTime;
        entity.MonthRent = house.MonthRent;
        entity.OwnerName = house.OwnerName;
        entity.OwnerPhoneNum = house.OwnerPhoneNum;
        entity.RoomTypeId = house.RoomTypeId;
        entity.StatusId = house.StatusId;
        entity.TotalFloorCount = house.TotalFloorCount;
        entity.TypeId = house.TypeId;

        entity.Attachments.Clear();
        var atts = ctx.Attachments.Where(a => a.IsDeleted == false && house.AttachmentIds.Contains(a.Id));
        foreach (var item in atts)
        {
          entity.Attachments.Add(item);
        }
        ctx.SaveChanges();
      }
    }

    private HouseDTO ToDTO(HouseEntity entity)
    {
      if (entity == null) return null;

      var dto = new HouseDTO()
      {
        Id = entity.Id,
        CityName = entity.Community.Region.City.Name,
        CheckInDateTime = entity.CheckInDateTime,
        CityId = entity.Community.Region.CItyId,
        CommunityBuiltYear = entity.Community.BuiltYear,
        CommunityId = entity.CommunityId,
        CommunityLocation = entity.Community.Location,
        CommunityName = entity.Community.Name,
        CommunityTraffic = entity.Community.Traffic,
        CreateDateTime = entity.CreateDateTime,
        DecorateStatusId = entity.DecorateStatusId,
        DecorateStatusName = entity.DecorateStatus.Name,
        Description = entity.Description,
        FloorIndex = entity.FloorIndex,
        LookableDateTime = entity.LookableDateTime,
        MonthRent = entity.MonthRent,
        OwnerName = entity.OwnerPhoneNum,
        OwnerPhoneNum = entity.OwnerPhoneNum,
        RegionId = entity.Community.RegionId,
        RegionName = entity.Community.Region.Name,
        RoomTypeId = entity.RoomTypeId,
        RoomTypeName = entity.RoomType.Name,
        StatusId = entity.StatusId,
        StatusName = entity.Status.Name,
        TotalFloorCount = entity.TotalFloorCount,
        TypeId = entity.TypeId,
        TypeName = entity.Type.Name,
        Address = entity.Address,
        Area = entity.Area,
        Direction = entity.Direction
      };
      var firstPic = entity.HousePics.FirstOrDefault();
      if (firstPic != null)
      {
        dto.FirstThumbUrl = firstPic.ThumbUrl;
      }
      dto.AttachmentIds = entity.Attachments.Select(i => i.Id).ToArray();

      return dto;
    }

    public HouseSearchResult Search(HouseSearchOptions options)
    {
      using (ZszDBContext ctx = new ZszDBContext())
      {
        CommonService<HouseEntity> cs = new CommonService<HouseEntity>(ctx);
        var items = cs.GetAll().Where(h => h.Community.Region.CItyId == options.CityId
            && h.TypeId == options.TypeId);
        if (options.RegionId != null)
          items = items.Where(h => h.Community.RegionId == options.RegionId);
        if (options.StartMonthRent != null)
          items = items.Where(h => h.MonthRent >= options.StartMonthRent);
        if (options.EndMonthRent != null)
          items = items.Where(h => h.MonthRent <= options.EndMonthRent);
        if (!string.IsNullOrEmpty(options.Keywords))
          items = items.Where(h => h.Address.Contains(options.Keywords)
              || h.Description.Contains(options.Keywords)
              || h.Community.Name.Contains(options.Keywords)
              || h.Community.Location.Contains(options.Keywords)
              || h.Community.Traffic.Contains(options.Keywords));
        long totalCount = items.LongCount();

        items = items.Include(h => h.Attachments)
             .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
             .Include(h => h.DecorateStatus)
             .Include(h => h.HousePics)
             .Include(h => h.RoomType)
             .Include(h => h.Status)
             .Include(h => h.Type)
             .Include(h => h.Attachments);

        switch (options.OrderByType)
        {
          case OrderByType.AreaAsc:
            items = items.OrderBy(t => t.Area);
            break;
          case OrderByType.AreaDesc:
            items = items.OrderByDescending(t => t.Area);
            break;
          case OrderByType.MonthRentAsc:
            items = items.OrderBy(t => t.MonthRent);
            break;
          case OrderByType.MonthRentDesc:
            items = items.OrderByDescending(t => t.MonthRent);
            break;
          case OrderByType.CreateDateDesc:
            items = items.OrderByDescending(t => t.CreateDateTime);
            break;
        }

        items = items.Skip((options.CurrentIndex - 1) * options.PageSize)
          .Take(options.PageSize);

        HouseSearchResult searchResult = new HouseSearchResult()
        {
          TotalCount = totalCount
        };
        List<HouseDTO> houses = new List<HouseDTO>();
        foreach (var item in items)
        {
          houses.Add(ToDTO(item));
        }
        searchResult.Result = houses.ToArray();
        return searchResult;
      }
    }

    public int GetTodayNewHouseCount(long cityID)
    {
      using (ZszDBContext ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        return cs.GetAll().Count(h => h.Community.Region.CItyId == cityID
          && SqlFunctions.DateDiff("hh", h.CreateDateTime, DateTime.Now) <= 24);
      }
    }
  }
}
