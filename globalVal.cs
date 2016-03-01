using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 

namespace CloudBread_Scheduler.globals
{
    public static class globalVal
    {
        public static string CBAzureWebJobsDashboard = ConfigurationManager.AppSettings["CBAzureWebJobsDashboard"].ToString();
        public static string CBAzureWebJobsStorage = ConfigurationManager.AppSettings["CBAzureWebJobsStorage"].ToString();
        public static string CBSchedulerDBConnectionString = ConfigurationManager.AppSettings["CBSchedulerDBConnectionString"].ToString();
        public static int conRetryCount = int.Parse(ConfigurationManager.AppSettings["CloudBreadconRetryCount"]);    /// adding v2.0.0
        public static int conRetryFromSeconds = int.Parse(ConfigurationManager.AppSettings["CloudBreadconRetryFromSeconds"]);     /// adding v2.0.0

        public static string CBNotiEmailSenderID = ConfigurationManager.AppSettings["CBNotiEmailSenderID"];
        public static string CBNotiEmailSenderPassword = ConfigurationManager.AppSettings["CBNotiEmailSenderPassword"];
        
        public static string CBNotiSlackWebhookURL = ConfigurationManager.AppSettings["CBNotiSlackWebhookURL"];

    }
}
