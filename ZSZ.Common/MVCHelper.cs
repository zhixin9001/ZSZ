using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZSZ.Common
{
  public enum AjaxResultEnum
  {
    ok,
    error
  }
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

    public static JsonResult ReturnJsonResult(AjaxResultEnum status, string msg = "")
    {
      return new JsonResult
      {
        Data = new AjaxResult
        {
          Status = status.ToString(),
          Msg = msg
        }
      };
    }

    public static string ToQueryString(NameValueCollection nvc)
    {
      StringBuilder strb = new StringBuilder();
      string value = string.Empty;
      foreach (var key in nvc.AllKeys)
      {
        value = nvc[key];
        strb.Append(key).Append("=").Append(Uri.EscapeDataString(value)).Append("&");
      }

      return strb.ToString().TrimEnd('&');
    }

    public static string RemoveQueryString(NameValueCollection nvc, string name)
    {
      var newNvc = new NameValueCollection(nvc);
      newNvc.Remove(name);
      return ToQueryString(newNvc);
    }

    public static string UpdateQueryString(NameValueCollection nvc, string name, string value)
    {
      var newNvc = new NameValueCollection(nvc);
      if (newNvc.AllKeys.Contains(name))
      {
        newNvc[name] = value;
      }
      else
      {
        newNvc.Add(name, value);
      }
      return ToQueryString(newNvc);
    }
  }
}
