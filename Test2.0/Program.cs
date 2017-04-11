using System;
using System.Collections.Generic;
using System.Text;

namespace Test2._0
{
  class Program
  {
    static void Main(string[] args)
    {
      #region Test DateTime
      var dateTime1 = new DateTime();
      var dateTime2 = new DateTime();
      Console.WriteLine("MinValue=" + DateTime.MinValue);
      Console.WriteLine("MaxValue=" + DateTime.MaxValue);
      #endregion

      //Console.WriteLine("OK");
      Console.ReadKey();
    }
  }
}
