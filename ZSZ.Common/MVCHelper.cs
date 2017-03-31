using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace ZSZ.Common
{
  public class MVCHelper
  {
    public static string GetValidMsg(ModelStateDictionary modelState)
    {
      StringBuilder sb = new StringBuilder();
      foreach (var key in modelState.Keys)
      {
        if (modelState[key].Errors.Count <= 0)
        {
          continue;
        }
        sb.Append("属性【").Append(key).Append("】错误：");
        foreach (var modelError in modelState[key].Errors)
        {
          sb.AppendLine(modelError.ErrorMessage);
        }
      }
      return sb.ToString();
    }
  }
}
