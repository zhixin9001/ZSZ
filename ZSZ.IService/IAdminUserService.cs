using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
  public interface IAdminUserService : IServiceSupport
  {
    long AddAdminUser(string name, string phoneNum, string password, string email, long? cityId);

    void UpdateAdminUser(long id, string name, string phonenum, string password, string email, long? cityId);

    AdminUserDTO[] GetAll(long? cityId);

    AdminUserDTO[] GetAll();

    AdminUserDTO GetById(long id);

    AdminUserDTO GetByPhoneNum(string phoneNum);

    bool CheckLogin(string phoneNum, string password);

    void MarkDeleted(long adminUserId);

    bool HasPermission(long adminUserId, string permissionName);

    void RecordLoginError(long id);

    void ResetLoginError(long id);
  }
}
