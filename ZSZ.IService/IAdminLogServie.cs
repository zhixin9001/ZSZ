using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
  public interface IAdminLogServie : IServiceSupport
  {
    long AddNew(long adminUserId, string message);

    AdminLogDTO GetById(long id);
  }
}
