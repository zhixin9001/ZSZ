using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
  public interface IUserService : IServiceSupport
  {
    long AddNew(string phoneNum, string password);

    UserDTO GetbyId(long id);

    UserDTO GetByPhoneNum(string phoneNum);

    bool CheckLogin(string phoneNum, string password);

    void SetUserCityId(long userId, long cityId);

    void UpdatePwd(long userId,string newPassword);
  }
}
