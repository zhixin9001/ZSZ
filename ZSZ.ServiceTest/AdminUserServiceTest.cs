using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSZ.Service.Services;

namespace ZSZ.ServiceTest
{
  [TestClass]
  public class AdminUserServiceTest
  {
    private AdminUserService adminUserService = new AdminUserService();
    [TestMethod]
    public void TestAdd()
    {
      long id = adminUserService.AddAdminUser("abc", "134133", "adsf", "s", null);
      var admin = adminUserService.GetById(id);
      Assert.AreEqual(admin.Name, "abc");
      Assert.AreEqual(admin.PhoneNum, "134133");
      Assert.AreEqual(admin.Email,"s");
      Assert.IsNull(admin.CityId);
      Assert.IsTrue(adminUserService.CheckLogin("134133", "adsf"));
      Assert.IsFalse(adminUserService.CheckLogin("134133", "sadsf"));
      adminUserService.MarkDeleted(id);
    }
  }
}
