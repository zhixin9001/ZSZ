using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities.RBAC;
using System.Data.Entity;

namespace ZSZ.Service
{
  public class PermissionService : IPermissionService
  {
    public void AddPermIds(long roleId, long[] permIds)
    {
      using (var ctx = new ZszDBContext())
      {
        var csRole = new CommonService<RoleEntity>(ctx);
        var permissions = csRole.GetById(roleId).Permissions;

        var isExist = permissions.Any(p => permIds.Contains(p.Id));
        if (isExist)
        {
          throw new ArgumentException(string.Format("the role {0} already has the permissions,permIds {1}", roleId, permIds.ToString()));
        }
        var csPerm = new CommonService<PermissionEntity>(ctx);

        var addedPerms=
        
      }
    }

    public PermissionDTO[] GetAll()
    {
      throw new NotImplementedException();
    }

    public PermissionDTO GetById(long id)
    {
      throw new NotImplementedException();
    }

    public PermissionDTO GetByName(string name)
    {
      throw new NotImplementedException();
    }

    public PermissionDTO[] GetByRoldId(long roleId)
    {
      throw new NotImplementedException();
    }

    public void UpdatePermIds(long roleId, long[] permIds)
    {
      throw new NotImplementedException();
    }

    private PermissionDTO ToDTO(PermissionEntity entity)
    {
      if (entity == null) return null;
      var dto = new PermissionDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        CreateDateTime = entity.CreateDateTime
      };

      return dto;

    }
  }
}
