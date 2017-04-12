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
  public class TestIdNameService
  {
    private IdNameService idnService = new IdNameService();
    [TestMethod]
    public void TestAddNewAndGetById()
    {
      var id = idnService.AddNew("ForTest", "ForTest");
      var idEntity = idnService.GetById(id);
      Assert.AreEqual("ForTest", idEntity.Name);
      Assert.AreEqual("ForTest", idEntity.TypeName);

      Assert.IsNull(idnService.GetById(999));
    }

    #region TestGetAll
    [TestMethod]
    public void TestGetAll_Exist()
    {
      var idns = idnService.GetAll("ForTest");
      Assert.IsTrue(idns.Count() > 0);
      Assert.IsNotNull(idns.Select(d => d.TypeName == "ForTest"));
    }
    [TestMethod]
    public void TestGetAll_NotExist()
    {
      var idns = idnService.GetAll("For");
      Assert.AreEqual(0, idns.Count());
    }
    #endregion
  }
}
