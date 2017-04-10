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
  public class AttachmentService : IAttachmentService
  {
    public AttachmentDTO[] GetAll()
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AttachmentEntity>(ctx);
        return cs.GetAll().ToList().Select(a => ToDTO(a)).ToArray();
      }
    }

    public AttachmentDTO[] GetAttachments(long houseId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<HouseEntity>(ctx);
        var houses = cs.GetAll().Include(h=>h.Attachments).AsNoTracking()
          .SingleOrDefault(h=>h.Id==houseId);
        if (houses == null)
        {
          throw new ArgumentException("The house doesn't exist,houseId:"+houseId);
        }
        return houses.Attachments.Select(a => ToDTO(a)).ToArray();
      }
    }

    private AttachmentDTO ToDTO(AttachmentEntity a)
    {
      if (a == null) return null;

      var dto = new AttachmentDTO()
      {
        Id = a.Id,
        Name = a.Name,
        CreateDateTime = a.CreateDateTime,
        IconName = a.IconName
      };
      return dto;
    }
  }
}
