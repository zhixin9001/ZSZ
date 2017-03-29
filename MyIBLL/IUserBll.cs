using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIBLL
{
  public interface IUserBll
  {
    void Check(string userName, string pwd);
    void AddNew(string userName, string pwd);
  }
}
