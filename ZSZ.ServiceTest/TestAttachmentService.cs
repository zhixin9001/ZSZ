using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Services;

namespace ZSZ.ServiceTest
{
  [TestClass]
  public class TestAttachmentService
  {
    private AttachmentService atService = new AttachmentService();
    [TestMethod]
    public void TestGetAll()
    {
      var ats = atService.GetAll();
      Assert.AreEqual(ats.Count(), 1);
    }

    #region TestGetAttachments
    [TestMethod]
    public void TestGetAttachments_HouseExist_AttachmentExist()
    {
      throw new Exception();
    }
    [TestMethod]
    public void TestGetAttachments_HouseExist_AttachmentNotExist()
    {
      throw new Exception();
    }
    [TestMethod]
    public void TestGetAttachments_HouseNotExist()
    {
      Assert.ThrowsException<ArgumentException>(() => atService.GetAttachments(2));
    }
    #endregion
  }
}
