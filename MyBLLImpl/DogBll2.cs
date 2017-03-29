using MyIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBLLImpl
{
  public class DogBll2 : IDogBll
  {
    public DogBll2() { }
    public DogBll2(string ss) { }
    public void Bark()
    {
      Console.WriteLine("DogBll2");
    }
  }
}
