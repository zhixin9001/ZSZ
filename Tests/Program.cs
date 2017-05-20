using Autofac;
using CaptchaGen;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using System.Net;
using ZSZ.Service.Entities;
using Qiniu.Common;
using Qiniu.Util;
using Qiniu.IO.Model;
using Qiniu.IO;
using Qiniu.Http;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Enyim.Caching;
using System.Threading;
using ServiceStack.Redis;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;
using PlainElastic.Net.Queries;

namespace Tests
{
  class Program
  {
    static void Main(string[] args)
    {
      #region SendEmail
      //using (MailMessage mailMessage = new MailMessage())
      //using (SmtpClient smtpClient = new SmtpClient("smtp.126.com"))
      //{
      //  mailMessage.To.Add("zhixin@bizsolution.com.cn");
      //  mailMessage.Body = "from the smtp 126";
      //  mailMessage.From = new MailAddress("zhixin9001@126.com");
      //  mailMessage.Subject = "Smtp 126";
      //  //smtpClient.EnableSsl=true
      //  smtpClient.Credentials = new System.Net.NetworkCredential("zhixin9001@126.com", "123456zx");
      //  smtpClient.Send(mailMessage);
      //}
      #endregion

      #region Thumbnail
      //ImageProcessingJob job = new ImageProcessingJob();
      //job.Filters.Add(new FixedResizeConstraint(200,200));
      //job.SaveProcessedImageToFileSystem(@"D:\1.png",@"D:\1_1.png");
      #endregion

      #region WaterMark
      //ImageWatermark imgWatermark = new ImageWatermark(@"D:\1_0.png");
      //imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;
      //imgWatermark.Alpha = 50;
      //ImageProcessingJob jobNormal = new ImageProcessingJob();
      //jobNormal.Filters.Add(imgWatermark);
      //jobNormal.Filters.Add(new FixedResizeConstraint(400, 400));
      //jobNormal.SaveProcessedImageToFileSystem(@"D:\1.png", @"D:\1_2.png");
      #endregion

      #region Capatcha
      //string captchaStr = CommonHelper.GenerateCaptchaCode(6);
      //using (MemoryStream ms = ImageFactory.GenerateImage(captchaStr, 60, 100, 20, 2))
      //using (FileStream fs = File.OpenWrite(@"d:\2.jpg"))
      //{
      //  ms.CopyTo(fs);
      //}
      #endregion

      #region Log4Net
      //log4net.config.xmlconfigurator.configure();
      //ILog log = LogManager.GetLogger(typeof(Program));
      //log.Error("an error has occured");
      #endregion

      #region Quartz
      //IScheduler sched = new StdSchedulerFactory().GetScheduler();
      //JobDetailImpl jdBossReport = new JobDetailImpl("jbTest", typeof(JobTest));
      ////IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(22, 52).Build();//每天23:45执行一次
      //CalendarIntervalScheduleBuilder builder = CalendarIntervalScheduleBuilder.Create();
      //builder.WithInterval(3, IntervalUnit.Second);
      //IMutableTrigger trigger = builder.Build();
      //trigger.Key = new TriggerKey("sdfdd");
      ////triggerBossReport.Key = new TriggerKey("triggerTest");
      //sched.ScheduleJob(jdBossReport, trigger);
      //sched.Start();

      #endregion

      #region AutoFac
      //IUserBll userBll = new UserBll();
      //userBll.AddNew("","");
      //ContainerBuilder builder = new ContainerBuilder();
      ////builder.RegisterType<UserBll>().As<IUserBll>();
      ////builder.RegisterType<UserBll>().AsImplementedInterfaces();

      //Assembly asm = Assembly.Load("MyBLLImpl");
      //builder.RegisterAssemblyTypes(asm).AsImplementedInterfaces().PropertiesAutowired();

      //IContainer container = builder.Build();
      ////IUserBll userBll = container.Resolve<IUserBll>();
      ////IDogBll dogBll = container.Resolve<IDogBll>();
      ////dogBll.Bark();
      ////userBll.AddNew("autofac", "");

      ////IEnumerable<IDogBll> dogBlls = container.Resolve<IEnumerable<IDogBll>>();
      ////foreach(var item in dogBlls)
      ////{
      ////  item.Bark();
      ////}

      //ISchool school = container.Resolve<ISchool>();
      //school.FangXue();
      //string captchaStr = CommonHelper.GenerateCaptchaCode(6);
      //using (MemoryStream ms = ImageFactory.GenerateImage(captchaStr, 60, 100, 20, 2))
      //using (FileStream fs = File.OpenWrite(@"d:\2.jpg"))
      //{
      //  ms.CopyTo(fs);
      //}
      #endregion

      #region SMS
      //string userName = "zhixin";
      //string appKey = "7e4055bc468ed92ef1134d";
      //string templateId = "193";
      //string code = "6666";
      //string phoneNum = "15354567890";
      //WebClient wc = new WebClient();

      //string downLoadString = "http://sms.rupeng.cn/SendSms.ashx?userName={0}&appKey={1}&templateId={2}&code={3}&phoneNum={4}";
      //string response = wc.DownloadString(string.Format(downLoadString
      //   , Uri.EscapeDataString(userName)
      //   , Uri.EscapeDataString(appKey)
      //   , Uri.EscapeDataString(templateId)
      //   , Uri.EscapeDataString(code)
      //   , Uri.EscapeDataString(phoneNum)));   
      #endregion

      #region UnitTest Simulate
      //AsertEqual(Add(0, 1), 1);
      //AsertEqual(Add(0, 1), 2);
      #endregion

      //using (ZszDBContext ctx = new ZszDBContext())
      //{
      //  ctx.Database.Delete();
      //  ctx.Database.Create();
      //}

      #region Test DateTime
      //Console.WriteLine("MinValue="+DateTime.MinValue);
      //Console.WriteLine("MaxValue=" + DateTime.MaxValue);

      //var dateTime1 = DateTime.Now;
      //var dateTime2 = dateTime1;
      //Console.WriteLine("dateTime1=" + dateTime1.ToShortDateString());
      //Console.WriteLine("dateTime2=" + dateTime2.ToShortDateString());
      //dateTime1=dateTime1.AddDays(3);
      //Console.WriteLine("**dateTime1.AddDays(3)");
      //Console.WriteLine("dateTime1=" + dateTime1.ToShortDateString());
      //Console.WriteLine("dateTime2=" + dateTime2.ToShortDateString());

      //var dateTimeUTC = DateTime.UtcNow;
      //var s = new Random();
      #endregion

      #region QiNiu
      //var AK = "mPRqfvRKWQoGB2X0SOluytNuTA6Rn41K7XQlDM7c";
      //var SK = "hM_YswqE79hwSTb4TVPZf7exG5RTclXeI53APU3z";
      //Qiniu.Common.Config.AutoZone(AK, "zhixin9001", false);


      //// 生成(上传)凭证时需要使用此Mac
      //// 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
      //// 实际应用中，请自行设置您的AccessKey和SecretKey
      //Mac mac = new Mac(AK, SK);
      //string bucket = "zhixin9001";
      //string saveKey = "1.png";
      //string localFile = "D:\\1.png";
      //// 上传策略，参见 
      //// https://developer.qiniu.com/kodo/manual/put-policy
      //PutPolicy putPolicy = new PutPolicy();
      //// 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
      //// putPolicy.Scope = bucket + ":" + saveKey;
      //putPolicy.Scope = bucket;
      //// 上传策略有效期(对应于生成的凭证的有效期)          
      //putPolicy.SetExpires(3600);
      //// 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
      //putPolicy.DeleteAfterDays = 1;
      //// 生成上传凭证，参见
      //// https://developer.qiniu.com/kodo/manual/upload-token            
      //string jstr = putPolicy.ToJsonString();
      //string token = Auth.CreateUploadToken(mac, jstr);
      //UploadManager um = new UploadManager();
      //HttpResult result = um.UploadFile(localFile, saveKey, token);
      //Console.WriteLine(result);
      #endregion

      #region Memcached
      //MemcachedClientConfiguration config = new MemcachedClientConfiguration();
      //config.Servers.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211));
      //config.Protocol = MemcachedProtocol.Binary;
      //MemcachedClient client = new MemcachedClient(config);
      //var p = new Person { Id = 3, Name = "yzx" };
      ////保存到缓存中
      ////HttpContext.Cache.Insert(cacheKey, model, null,
      ////DateTime.Now.AddSeconds(1),TimeSpan.Zero);
      //client.Store(StoreMode.Set, "p" + p.Id, p, DateTime.Now.AddSeconds(3));//还可以指定第四个参数指定数据的过期时间。
      //Thread.Sleep(2000);

      //Person p1 = client.Get<Person>("p3");//HttpContext.Cache[cacheKey]
      //Console.WriteLine(p1.Name);
      //Thread.Sleep(2000);
      //p1 = client.Get<Person>("p3");
      //Console.WriteLine(p1.Name);
      #endregion

      #region Redis
      //PooledRedisClientManager redisMgr = new PooledRedisClientManager("127.0.0.1");
      //using (IRedisClient redisClient = redisMgr.GetClient())
      //{
      //  var p = new Person { Id = 3, Name = "yzx" };
      //  //redisClient.Set("p", p,DateTime.Now.AddSeconds(3));
      //  redisClient.Set("p3", p, TimeSpan.FromSeconds(6));
      //  Thread.Sleep(2000);
      //  var p1 = redisClient.Get<Person>("p3");
      //  Console.WriteLine(p1.Name);
      //  Thread.Sleep(2000);
      //  p1 = redisClient.Get<Person>("p3");
      //  Console.WriteLine(p1.Name);
      //}
      #endregion

      #region ElasticSearch
      Person p1 = new Person()
      {
        Id = 1,
        Name = "P1",
        Age = 10,
        Desc = "P1 Person"
      };

      ElasticConnection client = new ElasticConnection("localhost",9200);
      var serializer = new JsonNetSerializer();
      IndexCommand cmd = new IndexCommand("zsz", "persons", p1.Id.ToString());
      var ss = serializer.Serialize(p1);
      OperationResult result = client.Put(cmd, ss);
      var indexResult = serializer.ToIndexResult(result.Result);
      if (indexResult.created)
      {
        Console.WriteLine("Created successfully");
      }
      else
      {
        Console.WriteLine(indexResult.status + "-" + indexResult._version);
      }

      SearchCommand sCmd = new SearchCommand("zsz", "persons");
      var query = new QueryBuilder<Person>().Query(
          b => b.Bool(
              m => m.Must(
                  t => t.QueryString(
                      t1 => t1.DefaultField("Desc").Query("P")
                    )
                )
            )
        ).Build();

      var resultS = client.Post(sCmd, query);
      var serializerS = new JsonNetSerializer();
      var searchResult = serializer.ToSearchResult<Person>(resultS);
      if (searchResult.Documents.Count() <= 0)
      {
        Console.WriteLine("No items found");
      }
      else
      {
        foreach (var doc in searchResult.Documents)
        {
          Console.WriteLine(doc.Id + "," + doc.Name + "," + doc.Age);
        }
      }
      
      #endregion

      Console.ReadKey();
    }
    //[Serializable]
    public class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public int Age { get; set; }
      public string Desc { get; set; }
    }

    public static int Add(int i, int j)
    {
      return i + j;
    }

    public static void AsertEqual(int value, int expectValue)
    {
      if (value != expectValue)
        throw new Exception("Bug Occured");
    }
  }
}
