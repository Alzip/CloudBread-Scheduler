/**
* @brief This Notification functinos are used by batch scheduling service of CloudBread 
* @author Dae Woo Kim
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slack.Webhooks;
using System.Net.Mail;
using CloudBread_Scheduler.globals;
namespace CloudBread_Scheduler
{
    public static class CBNotification
    {
        /// @brief this function wrapping send email notication
        public static string SendEmail(string toAddress, string subject, string body)
        {
            string result = "";
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", /// GMail SMTP server
                    Port = 587,     /// check official gmail site https://support.google.com/mail/answer/13287
                    /// smtp.gmail.com
                    /// Use Authentication: Yes
                    /// Port for TLS/STARTTLS: 587
                    /// Port for SSL: 465
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(globalVal.CBNotiEmailSenderID, globalVal.CBNotiEmailSenderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(globalVal.CBNotiEmailSenderID, globalVal.CBNotiEmailSenderPassword, subject, body);
                smtp.Send(message);
                result = "ok";

                return result;
            }

            catch (Exception)
            {
                throw;
            }


        }

        /// @brief this function wrapping send slack message to channel
        public static void SendSlackMsg(string Channel, string Text, string Username)
        {
            // slack webhook configuration : https://cloudbread.slack.com/apps/manage/A0F7XDUAZ-incoming-webhooks 
            var slackClient = new SlackClient(globalVal.CBNotiSlackWebhookURL);

            var slackMessage = new SlackMessage
            {
                Channel = Channel,
                Text = Text,
                Username = Username
            };

            try
            {
                slackClient.Post(slackMessage);

            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}
