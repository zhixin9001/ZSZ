using MyIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBLLImpl
{
  public class UserBll : IUserBll
  {
    public void AddNew(string userName, string pwd)
    {
      Console.WriteLine("Add new user:"+userName);
    }

    public void Check(string userName, string pwd)
    {
      Console.WriteLine("Login Succeed");
    }
  }
}
