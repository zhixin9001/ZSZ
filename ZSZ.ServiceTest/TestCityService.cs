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
  public class TestCityService
  {
    private CityService ctService = new CityService();
    [TestMethod]
    public void TestGetAll()
    {
      var cts = ctService.GetAll();
      Assert.AreEqual(cts.Count(), 2);
    }
    #region TestGetById
    [TestMethod]
    public void TestGetById_Exist()
    {
      var ct = ctService.GetById(2);
      Assert.AreEqual(ct.Name, "西安");
    }
    [TestMethod]
    public void TestGetById_NotExist()
    {
      var ct = ctService.GetById(999);
      Assert.IsNull(ct);
    }

    [TestMethod]
    public void TestGetById_Deleted()
    {
      var ct = ctService.GetById(3);
      Assert.IsNull(ct);
    }
    #endregion
  }
}
