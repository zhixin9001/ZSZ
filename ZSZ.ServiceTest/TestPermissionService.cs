using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Services;

namespace ZSZ.ServiceTest
{
  [TestClass]
  public class TestPermissionService
  {
    private PermissionService permService = new PermissionService();
    #region TestAddPermission
    [TestMethod]
    public void TestAddPermissionAndGetById()
    {
      var permName = "ForTest_" + Guid.NewGuid();
      var id = permService.AddPermission(permName, "ForTest");
      var perm = permService.GetById(id);
      Assert.AreEqual(permName, perm.Name);
      Assert.ThrowsException<ArgumentException>(() => permService.AddPermission(permName, "ForTest"));
    }
    [TestMethod]
    public void TestAddPermissionEmptyTypeName()
    {
      Assert.ThrowsException<ArgumentException>(() => permService.AddPermission("", "ForTest"));
      Assert.ThrowsException<ArgumentException>(() => permService.AddPermission(null, "ForTest"));
      Assert.ThrowsException<ArgumentException>(() => permService.AddPermission(string.Empty, "ForTest"));
    }

    #endregion

    #region TestAddPermIds
    [TestMethod]
    public void TestAddPermIds_RoleNotExist_PermsExist()
    {
      Assert.ThrowsException<ArgumentException>(() => permService.AddPermIds(999, new long[] { 1, 2, 3 }));
    }
    [TestMethod]
    public void TestAddPermIds_RoleExist_PermsAdded()
    {
      Assert.ThrowsException<ArgumentException>(() => permService.AddPermIds(2, new long[] { 1, 2, 3 }));
    }
    [TestMethod]
    public void TestAddPermIds_RoleExist_PermsNotExist()
    {
      permService.AddPermIds(2, new long[] { 999, 998, 997 });
      permService.AddPermIds(2, new long[] { 999, 4, 5 });
      //Assert.ThrowsException<ArgumentException>(() => permService.AddPermIds(2, new long[] { 999, 998, 997 }));
    }
    [TestMethod]
    public void TestAddPermIds_RoleExist_PermsEmpty()
    {
      permService.AddPermIds(2, new long[] { });
    }
    #endregion
    [TestMethod]
    public void TestGetAll()
    {
      var perm = permService.GetAll();
      Assert.IsTrue(perm.Count() > 0);
    }

    #region TestGetById
    [TestMethod]
    public void TestGetById_Exist()
    {
      var perm = permService.GetById(1);
      Assert.AreEqual("ForTest_0243be3d-4ed5-478d-ab68-72bbd8b46698", perm.Name);
      Assert.AreEqual("ForTest", perm.Description);
    }
    [TestMethod]
    public void TestGetById_NotExist()
    {
      var perm = permService.GetById(999);
      Assert.IsNull(perm);
    }
    #endregion

    #region TestGetByName
    [TestMethod]
    public void TestGetByName_Exist()
    {
      var perm = permService.GetByName("ForTest_0243be3d-4ed5-478d-ab68-72bbd8b46698");
      Assert.AreEqual(1, perm.Id);
      Assert.AreEqual("ForTest", perm.Description);
    }
    [TestMethod]
    public void TestGetByName_NotExist()
    {
      var perm = permService.GetByName("999");
      Assert.IsNull(perm);
    }
    [TestMethod]
    public void TestGetByName_Null()
    {
      var perm = permService.GetByName("");
      var perm1 = permService.GetByName(null);
      var perm2 = permService.GetByName(string.Empty);
      Assert.IsNull(perm);
      Assert.IsNull(perm1);
      Assert.IsNull(perm2);
    }
    #endregion

    #region TestGetByRoldId
    [TestMethod]
    public void TestGetByRoldId_Exist()
    {
      var perms = permService.GetByRoldId(2);
      Assert.AreEqual(5, perms.Count());
    }
    [TestMethod]
    public void TestGetByRoldId_NotExist()
    {
      Assert.ThrowsException<ArgumentException>(() => permService.GetByRoldId(999));
    }
    #endregion

    #region TestUpdatePermIds
    [TestMethod]
    public void TestUpdatePermIds_RoleExist_PermsExist()
    {
      permService.UpdatePermIds(2, new long[] { 7, 8 });
      var perms = permService.GetByRoldId(2);
      foreach (var item in perms)
      {
        Assert.IsTrue((new long[] { 7, 8 }).Contains(item.Id));
      }
    }
    [TestMethod]
    public void TestUpdatePermIds_RoleNotExist_PermsExist()
    {
      Assert.ThrowsException<ArgumentException>(() => permService.UpdatePermIds(999, new long[] { 7, 8 }));
    }
    [TestMethod]
    public void TestUpdatePermIds_RoleExist_PermsNotExist()
    {
      Assert.ThrowsException<ArgumentException>(() => permService.UpdatePermIds(2, new long[] { 999 }));
    }
    [TestMethod]
    public void TestUpdatePermIds_RoleExist_PermsEmpty()
    {
      permService.UpdatePermIds(2, new long[] { });
      Assert.IsTrue(permService.GetByRoldId(2).Count() == 0);
    }
    #endregion
  }
}
