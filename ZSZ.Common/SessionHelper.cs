using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZSZ.Common
{
  public class SessionHelper
  {
    public const string LOGIN_SESSION_NAME= "LoginUserId";
    public static long? GetLoginId(HttpContextBase ctx)
    {
      return (long?)ctx.Session["LoginUserId"];
    }
  }
}
