using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1
{
  public class JsonNetResult : JsonResult
  {
    public JsonNetResult()
    {
      Settings = new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Error
      };
    }
    public JsonNetResult(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet, string contentType = null, Encoding contentEncoding = null)
    {
      Data = data;
      JsonRequestBehavior = behavior;
      ContentEncoding = contentEncoding;
      ContentType = contentType;
    }

    private JsonSerializerSettings _settings;
    public JsonSerializerSettings Settings
    {
      get
      {
        _settings = _settings ?? new JsonSerializerSettings();
        _settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        return _settings;
      }
      private set { _settings = value; }
    }

    public override void ExecuteResult(ControllerContext context)
    {

      if (context == null)
        throw new ArgumentNullException("context");
      if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("JSON GET is not allowed");
      var response = context.HttpContext.Response;
      response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

      if (ContentEncoding != null)
        response.ContentEncoding = ContentEncoding;
      if (Data == null)
        return;
      var scriptSerializer = JsonSerializer.Create(Settings);
      using (var sw = new StringWriter())
      {
        scriptSerializer.Serialize(sw, Data);
        response.Write(sw.ToString());
      }
    }
  }
}