using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
  public interface IPermissionService : IServiceSupport
  {
    PermissionDTO GetById(long id);

    PermissionDTO[] GetAll();

    void UpdatePermission(long id, string permName, string description);

    void MarkDeleted(long id);

    PermissionDTO GetByName(string name);

    PermissionDTO[] GetByRoldId(long roleId);

    long AddPermission(string permName, string description);

    void AddPermIds(long roleId, long[] permIds);

    void UpdatePermIds(long roleId, long[] permIds);
  }
}
