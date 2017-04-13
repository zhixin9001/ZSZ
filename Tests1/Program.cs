using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests1
{
  class Program
  {
    static void Main(string[] args)
    {
      #region Test int?

      //int? a=3;
      //int? num = new int?(3);
      //int? b = new Nullable<int>(1);

      //b = a;

      //int? d = null;
      //Console.Write("d==null 的判断结果：");
      //Console.WriteLine(d==null);

      //var d1 = d.GetValueOrDefault();

      //var d2 = d.GetValueOrDefault(99);
      //var d3 = d ?? 99;


      //Console.WriteLine(d1);
      //Console.WriteLine(d2);
      //Console.WriteLine(d3);


      #endregion

      Nullable<int> e = 1;

      int? s1 = 9;
      int s2 = 0;

      s1 = s2;
      s2 = (int)s1;

      string s = null;

      //Console.WriteLine(s);
      Console.ReadKey();
    }

    public struct Nullable1<T> where T : struct
    {
      public bool hasValue;
      public T value;
      public Nullable1(T v)
      {
        this.value = v;
        this.hasValue = true;
      }

    }

  }
}
