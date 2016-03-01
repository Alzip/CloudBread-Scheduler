using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;

namespace CloudBread_Scheduler1
{
    /// @brief this class is scheuder queue message format
    public class CBBatchJob
    {
        public string JobID { get; set; }
        public string JobTitle { get; set; }
        public string JobTriggerDT { get; set; }
        public string JobTrackID { get; set; }
    }

    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void CBProcessQueueMessage([QueueTrigger("cloudbread-batch")] CBBatchJob bj, int dequeueCount)
        {
            try
            {
                Console.WriteLine("CB task Starting {0} at CBProcessQueueMessage", bj.JobID);

                switch (bj.JobID)
                {
                    case "CDBatch-DAU":
                        /// Database connection retry policy
                        RetryPolicy retryPolicy = new RetryPolicy<SqlAzureTransientErrorDetectionStrategy>(globalVal.conRetryCount, TimeSpan.FromSeconds(globalVal.conRetryFromSeconds));
                        using (SqlConnection connection = new SqlConnection(globalVal.DBConnectionString))
                        {
                            using (SqlCommand command = new SqlCommand("sspBatchDAU", connection))
                            {
                                connection.OpenWithRetry(retryPolicy);
                                command.ExecuteNonQueryWithRetry(retryPolicy);
                            }
                            connection.Close();
                        } 
                         break;

                    case "CDBatch-DARPU":

                        break;
                    case "CDBatch-HAU":
                        break;
                    default:
                        break;
                }
            }

            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        /// Timer trigger of CBProcessHAUTrigger
        public static void CBProcessHAUTrigger([TimerTrigger("0 * */1 * * *")] TimerInfo timer)
        {
            try
            {
                Console.WriteLine("CB task timer starting at CBProcessHAUTrigger");

                CBBatchJob bj = new CBBatchJob();
                bj.JobID = "CDBatch-HAU";
                bj.JobTitle = "Hourly Active User Batch";
                bj.JobTriggerDT = DateTimeOffset.UtcNow.ToString();
                bj.JobTrackID = Guid.NewGuid().ToString();

                /// send message to cloudbread-batch queue
                /// Azure Queue Storage connection retry policy
                var queueStorageRetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 10);
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                queueClient.DefaultRequestOptions.RetryPolicy = queueStorageRetryPolicy;
                CloudQueue queue = queueClient.GetQueueReference("cloudbread-batch");
                queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(bj)));

                Console.WriteLine("CB task timer done at CBProcessHAUTrigger");

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        /// Timer trigger of CBProcessDAU_DARPUTrigger
        public static void CBProcessDAU_DARPUTrigger([TimerTrigger("0 5 12 * * *")] TimerInfo timer) // every day 12:05 
        {
            try
            {
                Console.WriteLine("CB task timer starting at CBProcessDAU_DARPUTrigger");

                // send message to cloudbread-batch queue
                /// Azure Queue Storage connection retry policy
                var queueStorageRetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 10);
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                queueClient.DefaultRequestOptions.RetryPolicy = queueStorageRetryPolicy;
                CloudQueue queue = queueClient.GetQueueReference("cloudbread-batch");

                /// send message to queue - DAU
                CBBatchJob bj = new CBBatchJob();
                bj.JobID = "CDBatch-DAU";
                bj.JobTitle = "Daily Active User Batch";
                bj.JobTriggerDT = DateTimeOffset.UtcNow.ToString();
                bj.JobTrackID = Guid.NewGuid().ToString();
                queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(bj)));

                /// send message to queue - DARPU
                bj.JobID = "CDBatch-DARPU";
                bj.JobTitle = "Daily ARPU Batch";
                bj.JobTriggerDT = DateTimeOffset.UtcNow.ToString();
                bj.JobTrackID = Guid.NewGuid().ToString();
                queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(bj)));

                Console.WriteLine("CB task timer done at CBProcessDAU_DARPUTrigger");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }
        
    }
}
