using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;

namespace Tests
{
  class Program
  {
    static void Main(string[] args)
    {
      var s = CommonHelper.GenerateCaptchaCode(4);
      Console.WriteLine(s);
      Console.ReadKey();
    }
  }
}
