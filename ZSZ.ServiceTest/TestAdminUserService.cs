using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSZ.Service.Services;
using System.Data.Entity.Validation;
using System.Linq;

namespace ZSZ.ServiceTest
{
  [TestClass]
  public class TestAdminUserService
  {
    private AdminUserService auService = new AdminUserService();
    [TestMethod]
    public void TestAddAndLogin()
    {
      long id = auService.AddAdminUser("abc", "134133", "adsf", "s", null);
      var admin = auService.GetById(id);
      Assert.AreEqual(admin.Name, "abc");
      Assert.AreEqual(admin.PhoneNum, "134133");
      Assert.AreEqual(admin.Email, "s");
      Assert.IsNull(admin.CityId);
      Assert.IsTrue(auService.CheckLogin("134133", "adsf"));
      Assert.IsFalse(auService.CheckLogin("134133", "sadsf"));
      auService.GetAll();
      Assert.IsNotNull(auService.GetByPhoneNum("134133"));
      auService.MarkDeleted(id);
    }
    [TestMethod]
    public void TestHasPermission()
    {
      try
      {

        var permService = new PermissionService();
        string permName = "ForTest_" + Guid.NewGuid().ToString();
        long permId = permService.AddPermission(permName, "ForTest");
        string permName2 = "ForTest_" + Guid.NewGuid().ToString();
        long permId2 = permService.AddPermission(permName2, "ForTest");

        var roleService = new RoleService();
        string roleName = "ForTest_" + Guid.NewGuid().ToString();
        long roleId = roleService.AddNew(roleName);

        string userPhone = "ForTest_" + Guid.NewGuid().ToString().Substring(0, 5);
        long userId = auService.AddAdminUser("ForTest", userPhone, "123", "@", null);

        roleService.AddRoleIds(userId, new long[] { roleId });
        permService.AddPermIds(roleId, new long[] { permId });

        Assert.IsTrue(auService.HasPermission(userId, permName));
        Assert.IsFalse(auService.HasPermission(userId, permName2));
      }
      catch (DbEntityValidationException ex)
      {
        foreach (var item in ex.EntityValidationErrors.SelectMany(err => err.ValidationErrors))
        {
          Console.WriteLine(item.ErrorMessage);

        }
        throw new Exception();
      }
    }
    [TestMethod]
    public void TestUpdateAdminUser()
    {
      //case: Update the existed and not deleted admin user
      auService.UpdateAdminUser(1, "abc1", "111111", "1234", "ss", null);
      //case: Check update result
      var auDTO = auService.GetById(1);
      //case: Try to login with old authorization
      //case: Try to login with new authorization
      //case: Update the deleted admin user
      //case: Update a not existed admin user      
    }
  }
}
