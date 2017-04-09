using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service.Services
{
  public class AdminLogService : IAdminLogServie
  {
    public void AddNew(long adminUserId, string message)
    {
      using (var ctx = new ZszDBContext())
      {
        var adminLog = new AdminLogEntity();
        adminLog.AdminUserId = adminUserId;
        adminLog.Msg = message;
        ctx.AdminUserLogs.Add(adminLog);
        ctx.SaveChanges();
      }
    }
  }
}
