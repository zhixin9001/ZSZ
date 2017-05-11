using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Jobs
{
  public class ReportJob : IJob
  {
    private static ILog log = LogManager.GetLogger(typeof(ReportJob));

    public void Execute(IJobExecutionContext context)
    {
      log.Debug("Prepare for collect today new house...");
      try
      {
        string bossEmails
                  , smtpServer
                  , smtpUserName
                  , smtpPassword
                  , smtpEmail;
        StringBuilder strb = new StringBuilder();
        var container = AutofacDependencyResolver.Current.ApplicationContainer;
        using (container.BeginLifetimeScope())
        {
          var _CityService = container.Resolve<ICityService>();
          var _HouseService = container.Resolve<IHouseService>();
          var _SettingService = container.Resolve<IIdNameService>();

          bossEmails = _SettingService.GetValue(Consts.BOSS_EMAIL);
          smtpServer = _SettingService.GetValue(Consts.SMTP_SERVER);
          smtpUserName = _SettingService.GetValue(Consts.SMTP_USERNAME);
          smtpPassword = _SettingService.GetValue(Consts.SMTP_PASSWORD);
          smtpEmail = _SettingService.GetValue(Consts.SMTP_EMAIL);
          foreach (var city in _CityService.GetAll())
          {
            long count = _HouseService.GetTodayNewHouseCount(city.Id);
            strb.Append(city.Name).Append(" house added:").AppendLine(count.ToString());
          }
        }
        log.Debug("Finish collecting the added new house count, begin sending email...");
        log.Debug(string.Format("bossemail:{0},smtpserver:{1},smtpusername:{2}",bossEmails,
            smtpServer,smtpUserName));
        using (MailMessage mailMessage = new MailMessage())
        using (SmtpClient smtpClient = new SmtpClient(smtpServer))
        {
          foreach (var bossEmail in bossEmails.Split(','))
          {
            mailMessage.To.Add(bossEmail);
          }

          mailMessage.Body = strb.ToString();
          mailMessage.From = new MailAddress(smtpEmail);
          mailMessage.Subject = "Today's new house count";
          smtpClient.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
          smtpClient.Send(mailMessage);
        }
        log.Debug("Sending email finished");
      }
      catch(Exception ex)
      {
        log.Error("Error while sending email", ex);
      }
    }
  }
}