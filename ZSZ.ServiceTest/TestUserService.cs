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
  public class TestUserService
  {
    private UserService uService = new UserService();
    #region TestAddNew
    [TestMethod]
    public void TestAddNew_NewAndExistedPhoneNum()
    {
      string phoneNum = "ForTest_" + Guid.NewGuid().ToString().Substring(0, 10);
      uService.AddNew(phoneNum, "ForTest");

      Assert.ThrowsException<ArgumentException>(() => uService.AddNew(phoneNum, "ForTest"));
    }

    [TestMethod]
    public void TestAddNew_NullPhone()
    {
      Assert.ThrowsException<DbEntityValidationException>(() => uService.AddNew(null, "ForTest"));
    }
    [TestMethod]
    public void TestAddNew_NullPwd()
    {
      string phoneNum = "ForTest_" + Guid.NewGuid().ToString().Substring(0, 10);
      Assert.ThrowsException<ArgumentException>(() => uService.AddNew(phoneNum, null));
      Assert.ThrowsException<ArgumentException>(() => uService.AddNew(phoneNum, string.Empty));
      Assert.ThrowsException<ArgumentException>(() => uService.AddNew(phoneNum, ""));
    }
    #endregion

    #region TestCheckLogin
    [TestMethod]
    public void TestCheckLogin_ValidAndPwdError()
    {
      string phoneNum = "ForTest_" + Guid.NewGuid().ToString().Substring(0, 10);
      uService.AddNew(phoneNum, "ForTest");
      Assert.IsTrue(uService.CheckLogin(phoneNum, "ForTest"));

      Assert.IsFalse(uService.CheckLogin(phoneNum, "ForTes1t"));
    }

    [TestMethod]
    public void TestCheckLogin_NoPhoneNum()
    {
      Assert.IsFalse(uService.CheckLogin(null, "ForTes1t"));
      Assert.IsFalse(uService.CheckLogin("", "ForTes1t"));
      Assert.IsFalse(uService.CheckLogin(string.Empty, "ForTes1t"));
    }
    #endregion

  }
}
