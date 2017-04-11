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
    private AdminUserService adminUserService = new AdminUserService();
    [TestMethod]
    public void TestAddAndLogin()
    {
      long id = adminUserService.AddAdminUser("abc", "134133", "adsf", "s", null);
      var admin = adminUserService.GetById(id);
      Assert.AreEqual(admin.Name, "abc");
      Assert.AreEqual(admin.PhoneNum, "134133");
      Assert.AreEqual(admin.Email, "s");
      Assert.IsNull(admin.CityId);
      Assert.IsTrue(adminUserService.CheckLogin("134133", "adsf"));
      Assert.IsFalse(adminUserService.CheckLogin("134133", "sadsf"));
      adminUserService.GetAll();
      Assert.IsNotNull(adminUserService.GetByPhoneNum("134133"));
      adminUserService.MarkDeleted(id);
    }
    [TestMethod]
    public void TestHasPerm_AddPerm()
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

        string userPhone = "ForTest_" + Guid.NewGuid().ToString().Substring(0,5);
        long userId = adminUserService.AddAdminUser("ForTest", userPhone, "123", "@", null);

        roleService.AddRoleIds(userId, new long[] { roleId });
        permService.AddPermIds(roleId, new long[] { permId });
        
        Assert.IsTrue(adminUserService.HasPermission(userId, permName));
        Assert.IsTrue(adminUserService.HasPermission(userId, permName2));

      }
      catch (DbEntityValidationException ex)
      {
        foreach (var item in ex.EntityValidationErrors.SelectMany(err=>err.ValidationErrors))
        {
          Console.WriteLine(item.ErrorMessage);
          
        }
        throw new Exception();
      }
    }

  }
}
