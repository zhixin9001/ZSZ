using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForUnitTest;

namespace UnitTestProject1
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      Assert.AreEqual(2, new Class1().Add(1, 1));   //TestCase
      Assert.AreEqual(2, new Class1().Add(1, 1));
      Assert.AreEqual(3, new Class1().Add(1, 1));
    }
  }
}
