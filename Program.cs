/**
* @peojct
* @brief This CloudBread-Scheduler is batch scheduling service of CloudBread 
* @author Dae Woo Kim
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Diagnostics;

namespace CloudBread_Scheduler1
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            /// adding webjob conifg
            JobHostConfiguration config = new JobHostConfiguration();
            config.Tracing.ConsoleLevel = TraceLevel.Verbose;
            
            /// using timer
            config.UseTimers();

            var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        private static bool VerifyConfiguration()
        {
            string webJobsDashboard = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            string webJobsStorage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            bool configOK = true;
            if (string.IsNullOrWhiteSpace(webJobsDashboard) || string.IsNullOrWhiteSpace(webJobsStorage))
            {
                configOK = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");

            }
            return configOK;
        }
    }
}
