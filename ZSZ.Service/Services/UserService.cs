using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service.Services
{
  public class UserService : IUserService
  {
    private ZszDBContext _ctx = new ZszDBContext();

    ~UserService()
    {
      _ctx.Dispose();
    }

    public long AddNew(string phoneNum, string password)
    {
      var cs = new CommonService<UserEntity>(_ctx);
      var isExist = cs.GetAll().Any(u => u.PhoneNum == phoneNum);
      if (isExist)
      {
        throw new ArgumentException("The PhoneNum has already existed:" + phoneNum);
      }
      if (string.IsNullOrEmpty(password) || password.Length < 6)
      {
        throw new ArgumentException("The password length must be more than 6");
      }
      var entity = new UserEntity()
      {
        PhoneNum = phoneNum,
        PasswordSalt = CommonHelper.GenerateCaptchaCode(5)
      };
      entity.PasswordHash = CommonHelper.CalcMD5(entity.PasswordSalt + password);

      _ctx.Users.Add(entity);

      _ctx.SaveChanges();

      return entity.Id;
    }

    public bool CheckLogin(string phoneNum, string password)
    {
      bool isValid = false;
      var cs = new CommonService<UserEntity>(_ctx);
      var entities = cs.GetAll().Where(u => u.PhoneNum == phoneNum);
      if (entities.Count() == 0)
      {
        return isValid;
      }
      else if (entities.Count() > 1)
      {
        throw new ArgumentException("There are more than one same phoneNum:" + phoneNum);
      }

      var entity = cs.GetAll().AsNoTracking().SingleOrDefault(u => u.PhoneNum == phoneNum);
      var inputPwdHash = CommonHelper.CalcMD5(entity.PasswordSalt + password);
      isValid = (inputPwdHash == entity.PasswordHash) ? true : false;

      return isValid;
    }

    public UserDTO GetbyId(long id)
    {
      var cs = new CommonService<UserEntity>(_ctx);
      return ToDTO(cs.GetAll().Include(c => c.City).AsNoTracking().SingleOrDefault(c => c.Id == id));
    }

    public UserDTO GetByPhoneNum(string phoneNum)
    {
      var cs = new CommonService<UserEntity>(_ctx);
      return ToDTO(cs.GetAll().Include(c => c.City).AsNoTracking().SingleOrDefault(c => c.PhoneNum == phoneNum));
    }

    public void SetUserCityId(long userId, long cityId)
    {
      var csUser = new CommonService<UserEntity>(_ctx);
      var user = csUser.GetAll().Include(c => c.City).SingleOrDefault(c => c.Id == userId);
      if (user == null)
      {
        throw new ArgumentException("The user doesn't exist, userId:" + userId);
      }

      var csCity = new CommonService<CityEntity>(_ctx);
      var city = csCity.GetById(cityId);

      //user.CityId = cityId;
      user.City = city ?? throw new ArgumentException("The city doesn't exist, cityId:" + cityId);

      _ctx.SaveChanges();
    }

    public void UpdatePwd(long userId, string newPassword)
    {
      var csUser = new CommonService<UserEntity>(_ctx);
      var user = csUser.GetAll().Include(c => c.City).SingleOrDefault(c => c.Id == userId);
      if (user == null)
      {
        throw new ArgumentException("The user doesn't exist, userId:" + userId);
      }
      user.PasswordHash = CommonHelper.CalcMD5(user.PasswordSalt + newPassword);

      _ctx.SaveChanges();
    }

    private UserDTO ToDTO(UserEntity entity)
    {
      if (entity == null) return null;

      var dto = new UserDTO()
      {
        Id = entity.Id,
        PhoneNum = entity.PhoneNum,
        CityId = entity.CityId,
        CreateDateTime = entity.CreateDateTime,
        LastLoginErrorDateTime = entity.LastLoginErrorDateTime,
        LoginErrorTimes = entity.LoginErrorTimes
      };

      return dto;
    }

    public void IncrLoginError(long id)
    {
      using (ZszDBContext ctx = new ZszDBContext())
      {
        //检查手机号不能重复
        CommonService<UserEntity> cs = new CommonService<UserEntity>(ctx);
        var user = cs.GetById(id);
        if (user == null)
        {
          throw new ArgumentException("用户不存在 " + id);
        }
        user.LoginErrorTimes++;
        user.LastLoginErrorDateTime = DateTime.Now;
        ctx.SaveChanges();
      }
    }

    public void ResetLoginError(long id)
    {
      using (ZszDBContext ctx = new ZszDBContext())
      {
        //检查手机号不能重复
        CommonService<UserEntity> cs = new CommonService<UserEntity>(ctx);
        var user = cs.GetById(id);
        if (user == null)
        {
          throw new ArgumentException("用户不存在 " + id);
        }
        user.LoginErrorTimes = 0;
        user.LastLoginErrorDateTime = null;
        ctx.SaveChanges();
      }
    }

    public bool IsLocked(long id)
    {
      using (ZszDBContext ctx = new ZszDBContext())
      {
        //检查手机号不能重复
        CommonService<UserEntity> cs = new CommonService<UserEntity>(ctx);
        var user = cs.GetById(id);
        //错误登录次数>=5，最后一次登录错误时间在30分钟之内
        return (user.LoginErrorTimes >= 5
            && user.LastLoginErrorDateTime > DateTime.Now.AddMinutes(-30));
      }
      
    }
  }
}
