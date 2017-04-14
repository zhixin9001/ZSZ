using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities.RBAC;
using System.Data.Entity;

namespace ZSZ.Service.Services
{
  public class PermissionService : IPermissionService
  {
    public void AddPermIds(long roleId, long[] permIds)
    {
      using (var ctx = new ZszDBContext())
      {
        var csRole = new CommonService<RoleEntity>(ctx);
        var role = csRole.GetById(roleId);
        if (role == null)
        {
          throw new ArgumentException("The roleid doesn't exist, roleid:" + roleId);
        }

        var permissions = role.Permissions;

        var isExist = permissions.Any(p => permIds.Contains(p.Id));
        if (isExist)
        {
          throw new ArgumentException(string.Format("The role {0} already has the permissions,permIds {1}", roleId, string.Join(",", permIds)));
        }
        var csPerm = new CommonService<PermissionEntity>(ctx);
        var addedPerms = csPerm.GetAll().Where(p => permIds.Contains(p.Id));
        foreach (var item in addedPerms)
        {
          role.Permissions.Add(item);
        }
        ctx.SaveChanges();
      }
    }

    public long AddPermission(string permName, string description)
    {
      if (string.IsNullOrEmpty(permName))
      {
        throw new ArgumentException("The Permission shouldn't be empty or null");
      }
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<PermissionEntity>(ctx);
        bool isExist = cs.GetAll().Any(p => p.Name == permName);
        if (isExist)
        {
          throw new ArgumentException("The Permission has already exist, Permission Name:" + permName);
        }
        var perm = new PermissionEntity()
        {
          Name = permName,
          Description = description
        };

        ctx.Permissions.Add(perm);
        ctx.SaveChanges();

        return perm.Id;
      }
    }

    public PermissionDTO[] GetAll()
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<PermissionEntity>(ctx);
        return cs.GetAll().OrderByDescending(p => p.CreateDateTime).ToList().Select(p => ToDTO(p)).ToArray();
      }
    }

    public PermissionDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<PermissionEntity>(ctx);
        return ToDTO(cs.GetById(id));
      }
    }

    public PermissionDTO GetByName(string name)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<PermissionEntity>(ctx);
        return ToDTO(cs.GetAll().FirstOrDefault(p => p.Name == name));
      }
    }

    public PermissionDTO[] GetByRoldId(long roleId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<RoleEntity>(ctx);
        var role = cs.GetAll().Include(p => p.Permissions).SingleOrDefault(p => p.Id == roleId);
        if (role == null)
        {
          throw new ArgumentException("The roleid doesn't exist, roleid:" + roleId);
        }

        return role.Permissions.ToList().Select(p => ToDTO(p)).ToArray();
      }
    }

    public void MarkDeleted(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<PermissionEntity>(ctx);
        var perm = cs.GetById(id);
        if (perm == null)
        {
          throw new ArgumentException("Id doesn't exist, Id: " + id);
        }
        cs.MarkDeleted(id);
        ctx.SaveChanges();
      }
    }

    public void UpdatePermIds(long roleId, long[] permIds)
    {
      using (var ctx = new ZszDBContext())
      {
        var csRole = new CommonService<RoleEntity>(ctx);
        var role = csRole.GetById(roleId);
        if (role == null)
        {
          throw new ArgumentException("The roleid doesn't exist, roleid:" + roleId);
        }
        if (permIds.Count() > 0)
        {
          var csPerm = new CommonService<PermissionEntity>(ctx);
          var updatedPerms = csPerm.GetAll().Where(p => permIds.Contains(p.Id));
          if (updatedPerms == null || updatedPerms.Count() <= 0)
          {
            throw new ArgumentException("There are no permissions to be updated");
          }
          role.Permissions.Clear();
          foreach (var item in updatedPerms)
          {
            role.Permissions.Add(item);
          }
        }
        else
        {
          role.Permissions.Clear();
        }
        ctx.SaveChanges();
      }
    }

    public void UpdatePermission(long id, string permName, string description)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<PermissionEntity>(ctx);
        var perm = cs.GetById(id);
        if (perm == null)
        {
          throw new ArgumentException("Id doesn't exist, Id: " + id);
        }
        perm.Name = permName;
        perm.Description = description;
        ctx.SaveChanges();
      }
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
