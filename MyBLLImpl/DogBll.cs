using MyIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBLLImpl
{
  public class DogBll : IDogBll
  {
    public void Bark()
    {
      Console.WriteLine("Bark!!Bark!!!");
    }
  }
}
