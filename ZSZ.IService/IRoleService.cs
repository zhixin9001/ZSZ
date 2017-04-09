using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
  public interface IRoleService:IServiceSupport
  {
    long AddNew(string roleName);

    void Update(long roleId, string roleName);

    void MarkDeleted(long roleId);

    RoleDTO GetById(long id);

    RoleDTO GetByName(string name);

    RoleDTO[] GetAll();

    void AddRoleIds(long adminUserId, long[] roleIds);

    void UpdateRoleIds(long adminUserId, long[] roleIds);

    RoleDTO[] GetByAdminUserId(long admin);
  }
}
