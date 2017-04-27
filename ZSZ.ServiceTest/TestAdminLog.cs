using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSZ.Service.Services;
using ZSZ.IService;
using ZSZ.DTO;
using ZSZ.Service;

namespace ZSZ.ServiceTest
{
  [TestClass]
  public class TestAdminLog
  {
    private ZszDBContext _ctx = new ZszDBContext();
    [TestMethod]
    public void TestAddNew()
    {
      var adminLog = new AdminLogService();
      var id = adminLog.AddNew(1, "Testing");
      var log = adminLog.GetById(id);
      Assert.AreEqual(log.Message, "Testing");
    }

  }
}
