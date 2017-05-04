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
  public class IdNameService : IIdNameService
  {
    public long AddNew(string typeName, string name)
    {
      using (var ctx = new ZszDBContext())
      {
        var entity = new IdNameEntity()
        {
          TypeName = typeName,
          Name = name
        };

        ctx.IdNames.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
      }
    }

    public IdNameDTO[] GetAll(string typeName)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<IdNameEntity>(ctx);
        return cs.GetAll().AsNoTracking().Where(h => h.TypeName == typeName).ToList()
          .Select(h => ToDTO(h)).ToArray();
      }
    }

    public IdNameDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<IdNameEntity>(ctx);
        return ToDTO(cs.GetById(id));
      }
    }

    public string GetValue(string value)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<IdNameEntity>(ctx);
        return cs.GetAll().AsNoTracking().Where(h => h.TypeName == value).FirstOrDefault().Name;
      }
    }

    private IdNameDTO ToDTO(IdNameEntity entity)
    {
      if (entity == null) return null;

      var dto = new IdNameDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        CreateDateTime = entity.CreateDateTime,
        TypeName = entity.TypeName
      };

      return dto;
    }
  }
}
