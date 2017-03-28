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
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;

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
      //log4net.Config.XmlConfigurator.Configure();
      //ILog log = LogManager.GetLogger(typeof(Program));
      //log.Error("an error has occured");
      #endregion

      #region Quartz
      IScheduler sched = new StdSchedulerFactory().GetScheduler();
      JobDetailImpl jdBossReport = new JobDetailImpl("jbTest",typeof(JobTest));
      IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(22, 52).Build();//每天23:45执行一次
      triggerBossReport.Key = new TriggerKey("triggerTest");
      sched.ScheduleJob(jdBossReport, triggerBossReport);
      sched.Start();

      #endregion
      Console.WriteLine("OK");
      Console.ReadKey();
    }
  }
}
