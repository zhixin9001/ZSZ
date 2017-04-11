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
  public class RoleService : IRoleService
  {
    private ZszDBContext _ctx = new ZszDBContext();

    ~RoleService()
    {
      _ctx.Dispose();
    }

    public long AddNew(string roleName)
    {
      var cs = new CommonService<RoleEntity>(_ctx);
      var isExist = cs.GetAll().Any(r => r.Name == roleName);
      if (isExist)
      {
        throw new ArgumentException("The Role being added has already exist, RoleName:" + roleName);
      }
      var entity = new RoleEntity()
      {
        Name = roleName
      };

      _ctx.Roles.Add(entity);
      _ctx.SaveChanges();

      return entity.Id;
    }

    public void AddRoleIds(long adminUserId, long[] roleIds)
    {
      var csAdminUser = new CommonService<AdminUserEntity>(_ctx);
      var adminUser = csAdminUser.GetById(adminUserId);
      if (adminUser == null)
      {
        throw new ArgumentException("The adminuser doesn't exist, AdminUserId: " + adminUserId);
      }

      var csRole = new CommonService<RoleEntity>(_ctx);
      foreach (var roleId in roleIds)
      {
        var role = csRole.GetById(roleId);
        if (role == null)
        {
          throw new ArgumentException("The added roles has some invalid member, roleId:" + role.Id);
        }
        adminUser.Roles.Add(role);
      }

      _ctx.SaveChanges();
    }

    public RoleDTO[] GetAll()
    {
      var cs = new CommonService<RoleEntity>(_ctx);
      return cs.GetAll().ToList().Select(r => ToDTO(r)).ToArray();
    }

    public RoleDTO[] GetByAdminUserId(long admin)
    {
      var cs = new CommonService<AdminUserEntity>(_ctx);
      var adminEntity = cs.GetById(admin);
      if (adminEntity == null)
      {
        throw new ArgumentException("The Admin doesn't exist, AdminId:" + admin);
      }

      return adminEntity.Roles.ToList().Select(r => ToDTO(r)).ToArray();
    }

    public RoleDTO GetById(long id)
    {
      var cs = new CommonService<RoleEntity>(_ctx);
      return ToDTO(cs.GetAll().Include(r => r.Permissions).SingleOrDefault(r => r.Id == id));
    }

    public RoleDTO GetByName(string name)
    {
      var cs = new CommonService<RoleEntity>(_ctx);
      return ToDTO(cs.GetAll().Include(r => r.Permissions).SingleOrDefault(r => r.Name == name));
    }

    public void MarkDeleted(long roleId)
    {
      var cs = new CommonService<RoleEntity>(_ctx);
      cs.MarkDeleted(roleId);
    }

    public void Update(long roleId, string roleName)
    {
      var cs = new CommonService<RoleEntity>(_ctx);
      var role = cs.GetById(roleId);
      if (role == null)
      {
        throw new ArgumentException("The role wished to be updated doesn't exist, roleId:"+roleId);
      }
      role.Name = roleName;

      _ctx.SaveChanges();
    }

    public void UpdateRoleIds(long adminUserId, long[] roleIds)
    {
      var csAdmin = new CommonService<AdminUserEntity>(_ctx);
      var adminUser = csAdmin.GetAll().Include(a => a.Roles).SingleOrDefault(a=>a.Id==adminUserId);
      if (adminUser == null)
      {
        throw new ArgumentException("The admin wished to be updated doesn't exist, adminUserId: "+adminUserId);
      }
      adminUser.Roles.Clear();
      var csRole = new CommonService<RoleEntity>(_ctx);
      foreach (var roleId in roleIds)
      {
        var role = csRole.GetById(roleId);
        if (role == null)
        {
          throw new ArgumentException("The rold doesn't exist, roleId:" +roleId);
        }
        adminUser.Roles.Add(role);
      }

      _ctx.SaveChanges();
    }

    private RoleDTO ToDTO(RoleEntity entity)
    {
      if (entity == null) return null;

      var dto = new RoleDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        CreateDateTime = entity.CreateDateTime,
      };
      return dto;
    }
  }
}
