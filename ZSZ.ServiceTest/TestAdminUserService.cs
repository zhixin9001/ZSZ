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

    #region TestGetAll(long? cityId)
    [TestMethod]
    public void TestGetAll_IdExist()
    {
      auService.GetAll(1);
    }
    [TestMethod]
    public void TestGetAll_IdNotExist()
    {
      var aus = auService.GetAll(9999);
      Assert.AreEqual(aus.Count(), 0);
    }
    [TestMethod]
    public void TestGetAll_IdIsNull()
    {
      var au1 = auService.GetAll(null);
      var au2 = auService.GetAll();
    }
    #endregion
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
      //auService.MarkDeleted(id);
    }
    
  

    [TestMethod]
    public void TestGetAll()
    {
      var aus = auService.GetAll();
    }

    #region TestGetById(long id)
    [TestMethod]
    public void TestGetById_IdExist()
    {
      auService.GetById(5);
    }
    [TestMethod]
    public void TestGetById_IdNotExist()
    {
      var au = auService.GetById(9999);
      Assert.IsNull(au);
    }

    #endregion

    #region GetByPhoneNum(string phoneNum)
    [TestMethod]
    public void GetByPhoneNum_Exist()
    {
      var au = auService.GetByPhoneNum("111111");
      Assert.AreEqual(5, au.Id);
    }
    [TestMethod]
    public void GetByPhoneNum_NotExist()
    {
      var au = auService.GetByPhoneNum("xxxxxx");
      Assert.IsNull(au);
    }
    [TestMethod]
    public void GetByPhoneNum_Null()
    {
      var au = auService.GetByPhoneNum(null);
      Assert.IsNull(au);
    }
    [TestMethod]
    public void GetByPhoneNum_Deleted()
    {
      var au = auService.GetByPhoneNum("134133");
      Assert.IsNull(au);
    }
    #endregion

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
    public void TestMarkDeleted()
    {
      var auId = auService.AddAdminUser("ForTest1", "ForTest1", "ForTest", "ForTest", 1);
      var au = auService.GetById(auId);
      Assert.AreEqual(au.Name, "ForTest1");
      auService.MarkDeleted(auId);
      Assert.IsNull(auService.GetById(auId));
    }

    #region TestUpdateAdminUser
    [TestMethod]
    public void TestUpdateAdminUser()
    {
      //case: Update the existed and not deleted admin user
      auService.UpdateAdminUser(5, "abc1", "111111", "1234", "ss", null);
    }
    [TestMethod]
    public void TestUpdateAdminUser_CheckUpdate()
    {
      //case: Check update result
      var auDTO = auService.GetById(5);
      Assert.AreEqual(auDTO.Name, "abc1");
      Assert.AreEqual(auDTO.PhoneNum, "111111");
      Assert.AreEqual(auDTO.Email, "ss");
    }
    [TestMethod]
    public void TestUpdateAdminUser_LoginWithOld()
    {
      //case: Try to login with old authorization
      Assert.IsFalse(auService.CheckLogin("134133", "adsf"));
    }
    [TestMethod]
    public void TestUpdateAdminUser_LoginWithNew()
    {
      //case: Try to login with new authorization
      Assert.IsTrue(auService.CheckLogin("111111", "1234"));
    }
    [TestMethod]
    public void TestUpdateAdminUser_UpdateDelted()
    {
      //case: Update the deleted admin user
      Assert.ThrowsException<ArgumentException>(() => auService.UpdateAdminUser(10, "abc1", "111111", "1234", "ss", null));
    }
    [TestMethod]
    public void TestUpdateAdminUser_UpdateNotExist()
    {
      //case: Update a not existed admin user      
      Assert.ThrowsException<ArgumentException>(() => auService.UpdateAdminUser(12, "abc1", "111111", "1234", "ss", null));
    }
    #endregion
  }
}
