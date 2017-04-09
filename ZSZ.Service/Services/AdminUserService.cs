using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities.RBAC;
using ZSZ.Service;
using System.Data.Entity;

namespace ZSZ.Service.Services
{
  public class AdminUserService : IAdminUserService
  {
    public long AddAdminUser(string name, string phoneNum, string password, string email, long? cityId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);

        bool isPhoneNumExist = cs.GetAll().Any(a => a.PhoneNum == phoneNum);
        if (isPhoneNumExist) throw new ArgumentException("This phonenum has exist:" + phoneNum);

        var entity = new AdminUserEntity();
        entity.Name = name;
        entity.PhoneNum = phoneNum;
        entity.PasswordSalt = CommonHelper.GenerateCaptchaCode(5);
        entity.PasswordHash = CommonHelper.CalcMD5(entity.PasswordSalt + password);
        entity.Email = email;
        entity.CityId = cityId;

        ctx.AdminUsers.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
      }
    }

    public bool CheckLogin(string phoneNum, string password)
    {
      bool isValid = false;
      using (var ctx = new ZszDBContext())
      {
        var entity = new CommonService<AdminUserEntity>(ctx).GetAll().SingleOrDefault(a => a.PhoneNum == phoneNum);
        if (entity != null)
        {
          var newPwdHash = CommonHelper.CalcMD5(entity.PasswordSalt + password.Trim());
          isValid = newPwdHash == entity.PasswordHash;
        }

        if (isValid)
        {
          entity.LoginErrorTimes = 0;
        }
        else
        {
          entity.LoginErrorTimes++;
          entity.LastLoginErrorDateTime = DateTime.Now;
        }
        ctx.SaveChanges();

        return isValid;
      }
    }

    public AdminUserDTO[] GetAll(long? cityId)
    {
      if (cityId == null)
      {
        return GetAll();
      }
      else
      {
        using (var ctx = new ZszDBContext())
        {
          var cs = new CommonService<AdminUserEntity>(ctx);
          return cs.GetAll().Include(u => u.City).AsNoTracking()
            .Where(e => e.CityId == cityId)
            .ToList().Select(a => ToDTO(a)).ToArray();
        }
      }
    }

    public AdminUserDTO[] GetAll()
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);
        return cs.GetAll().Include(u => u.City).AsNoTracking().ToList().Select(a => ToDTO(a)).ToArray();
      }
    }

    public AdminUserDTO GetById(long id)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);
        return ToDTO(cs.GetAll().Include(a => a.City).AsNoTracking()
           .SingleOrDefault(a => a.Id == id));
      }
    }

    public AdminUserDTO GetByPhoneNum(string phoneNum)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);
        var users = cs.GetAll().Include(a => a.City).AsNoTracking()
           .Where(a => a.PhoneNum == phoneNum);
        if (users == null)
        {
          return null;
        }
        else if (users.Count() == 1)
        {
          return ToDTO(users.Single());
        }
        else
        {
          throw new ApplicationException(string.Format("The phonenum {0} belongs to more than one admin"));
        }
      }
    }

    public bool HasPermission(long adminUserId, string permissionName)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);
        var user = cs.GetAll().Include(r => r.Roles).AsNoTracking()
          .SingleOrDefault(r => r.Id == adminUserId);
        if (user == null)
        {
          throw new ArgumentException("Can't find the admin,AdminId:" + adminUserId);
        }
        return user.Roles.SelectMany(r => r.Permissions)
          .Any(p => p.Name == permissionName);
      }
    }

    public void MarkDeleted(long adminUserId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);
        cs.MarkDeleted(adminUserId);
      }
    }

    public void RecordLoginError(long id)
    {
      throw new NotImplementedException();
    }

    public void ResetLoginError(long id)
    {
      throw new NotImplementedException();
    }

    public void UpdateAdminUser(long id, string name, string phonenum, string password, string email, long? cityId)
    {
      using (var ctx = new ZszDBContext())
      {
        var cs = new CommonService<AdminUserEntity>(ctx);
        var user = cs.GetById(id);
        user.Name = name;
        user.PhoneNum = phonenum;
        user.PasswordHash = CommonHelper.CalcMD5(user.PasswordSalt + password);
        user.Email = email;
        user.CityId = cityId;
        ctx.SaveChanges();
      }
    }

    private AdminUserDTO ToDTO(AdminUserEntity entity)
    {
      if (entity == null) return null;

      var dto = new AdminUserDTO()
      {
        Name = entity.Name,
        PhoneNum = entity.PhoneNum,
        Email = entity.Email,
        CityId = entity.CityId,
        LoginErrorTimes = entity.LoginErrorTimes,
        LastLoginErrorDateTime = entity.LastLoginErrorDateTime
      };

      if (entity.City != null)
      {
        dto.CityName = entity.City.Name;
      }

      return dto;
    }
  }
}
