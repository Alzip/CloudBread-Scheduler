using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CloudBread_Scheduler
{
    public static class globalVal
    {
        public static string CBAzureWebJobsDashboard = ConfigurationManager.AppSettings["CBAzureWebJobsDashboard"].ToString();
        public static string CBAzureWebJobsStorage = ConfigurationManager.AppSettings["CBAzureWebJobsStorage"].ToString();
        public static string CBSchedulerDBConnectionString = ConfigurationManager.AppSettings["CBSchedulerDBConnectionString"].ToString();

        public static string CBNotiEmailSenderID = ConfigurationManager.AppSettings["CBNotiEmailSenderID"];
        public static string CBNotiEmailSenderPassword = ConfigurationManager.AppSettings["CBNotiEmailSenderPassword"];
        
        public static string CBNotiSlackWebhookURL = ConfigurationManager.AppSettings["CBNotiSlackWebhookURL"];

    }
}
