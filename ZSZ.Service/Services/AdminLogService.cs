using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service.Services
{
  public class AdminLogService : IAdminLogServie
  {
    public long AddNew(long adminUserId, string message)
    {
      using (var ctx = new ZszDBContext())
      {
        var adminLog = new AdminLogEntity();
        adminLog.AdminUserId = adminUserId;
        adminLog.Msg = message;
        ctx.AdminUserLogs.Add(adminLog);
        ctx.SaveChanges();

        return adminLog.Id;
      }
    }

    public AdminLogDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminLogEntity>(ctx);
        var entity = cs.GetById(id);
        if (entity == null)
        {
          throw new ArgumentException("Can not find by the given Id:" + id);
        }

        var dto = new AdminLogDTO()
        {
          Id=entity.Id,
          AdminUserName=entity.AdminUser.Name,
          AdminUserId=entity.AdminUserId,
          CreateDateTime=entity.CreateDateTime,
          AdminUserPhoneNum=entity.AdminUser.PhoneNum,
          Message=entity.Msg
        };

        return dto;
      }
    }
  }
}
