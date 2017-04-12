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
  public class TestHouseAppointmentService
  {
    private HouseAppointmentService hAppService = new HouseAppointmentService();

    #region TestAddNew
    [TestMethod]
    public void TestAddNew_NewAppointment()
    {
      throw new Exception();
    }
    [TestMethod]
    public void TestAddNew_ExistedAppointment()
    {
      throw new Exception();
    }
    [TestMethod]
    public void TestAddNew_NullUserId()
    {
      throw new Exception();
    }
    #endregion
  }
}
