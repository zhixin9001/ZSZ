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
  public class TestCommunityService
  {
    private CommunityService cs = new CommunityService();
    [TestMethod]
    public void TestGetByRegionId_Exist()
    {
      var communities = cs.GetByRegionId(1);
      Assert.AreEqual(1,communities.Count());
      var c = communities[0];
      Assert.AreEqual(c.Name, "ForTest");
      Assert.AreEqual(c.RegionId, 1);
      Assert.AreEqual(c.Traffic, "ForTest");
      Assert.AreEqual(c.BuiltYear, 2014);
      Assert.AreEqual(c.Location, "ForTest");
      Assert.AreEqual(c.CreateDateTime.ToShortDateString(), "2017/4/12");
    }
    [TestMethod]
    public void TestGetByRegionId_NotExist()
    {
      var communities = cs.GetByRegionId(999);
      Assert.AreEqual(0, communities.Count());
    }
  }
}
