using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ZSZ.Common
{
  public class SMSSender
  {
    public string UserName { get; set; } = "zhixin";
    public string AppKey { get; set; } = "7e4055bc468ed92ef1134d";
    public string TemplateId { get; set; } = "193";
    public string Code { get; set; } = "6666";
    public string PhoneNum { get; set; } = "15354567890";

    public SMSSender(string userName, string appKey, string templateId, string code, string phoneNum)
    {
      UserName = userName;
      AppKey = appKey;
      TemplateId = templateId;
      Code = code;
      PhoneNum = phoneNum;
    }

    public SMSResult SendSMS()
    {
      WebClient wc = new WebClient();
      string downLoadString = "http://sms.rupeng.cn/SendSms.ashx?userName={0}&appKey={1}&templateId={2}&code={3}&phoneNum={4}";
      string response = wc.DownloadString(string.Format(downLoadString
          , Uri.EscapeDataString(UserName)
          , Uri.EscapeDataString(AppKey)
          , Uri.EscapeDataString(TemplateId)
          , Uri.EscapeDataString(Code)
          , Uri.EscapeDataString(PhoneNum)));

      JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
      var result = jsSerializer.Deserialize<SMSResult>(response);
      return result;
    }
  }

  public class SMSResult
  {
    public string Msg { get; set; }
    public string Result { get; set; }
  }
}
