using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Utils
{
    public class SendTemplateEmail
    {
        #region constants

        private static int MAILPORT = 25;
        private static string MAILSERVER = "jaxappmail.interlinebrands.com";

        #endregion constants

        #region Send Email

        /// <summary>
        /// Send a email from someone to a list of users
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmails"></param>
        /// <param name="subjectLine"></param>
        /// <param name="emailBody"></param>
        public static void SendNewEmail(string fromEmail, List<string> toEmails, string subjectLine, string emailBody, Attachment attachment = null)
        {
            var emailMessage = CreateMailMessage(fromEmail, toEmails, subjectLine, emailBody, attachment);
            SendNewEmail(emailMessage);
        }

        /// <summary>
        /// Send a email with multiple attachments from someone to a list of users
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmails"></param>
        /// <param name="subjectLine"></param>
        /// <param name="emailBody"></param>
        /// <param name="list of attachments"></param>
        public static void SendNewEmailWithMultipleAttachments(string fromEmail, List<string> toEmails, string subjectLine, string emailBody, List<Attachment> attachments = null)
        {
            var emailMessage = CreateMailMessageWithMultipleAttachments(fromEmail, toEmails, subjectLine, emailBody, attachments);
            SendNewEmail(emailMessage);
        }

        #endregion Send Email

		#region Attachment Helper

        /// <summary>
        /// Create an Attachment from the byte array and a file name
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="fileData">The byte array for the file data</param>
        /// <returns></returns>
        public Attachment CreateAttachment(string fileName, byte[] fileData)
        {
            return new Attachment(new MemoryStream(fileData), fileName);
        }

        #endregion Attachment Helper
		
        #region Private functions

        /// <summary>
        /// Create the mail message
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmails"></param>
        /// <param name="subjectLine"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        private static MailMessage CreateMailMessage(string fromEmail, List<string> toEmails, string subjectLine, string emailBody, Attachment attachment = null)
        {
            var emailMessage = new MailMessage();

            //create new mail address from the fromemail string
            emailMessage.From = new MailAddress(fromEmail);

            //add emails from list of to list
            foreach (var toEmail in toEmails)
            {
                emailMessage.To.Add(toEmail);
            }

            //add subject and body
            emailMessage.Subject = subjectLine;
            emailMessage.Body = emailBody;
            //set the body to html form
            emailMessage.IsBodyHtml = true;

            if (attachment != null)
            {
                emailMessage.Attachments.Add(attachment);
            }

            return emailMessage;
        }

        /// <summary>
        /// Create the mail message
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmails"></param>
        /// <param name="subjectLine"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        private static MailMessage CreateMailMessageWithMultipleAttachments(string fromEmail, List<string> toEmails, string subjectLine, string emailBody, List<Attachment> attachments = null)
        {
            var emailMessage = new MailMessage();

            //create new mail address from the fromemail string
            emailMessage.From = new MailAddress(fromEmail);

            //add emails from list of to list
            foreach (var toEmail in toEmails)
            {
                emailMessage.To.Add(toEmail);
            }

            //add subject and body
            emailMessage.Subject = subjectLine;
            emailMessage.Body = emailBody;
            //set the body to html form
            emailMessage.IsBodyHtml = true;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    emailMessage.Attachments.Add(attachment);
                }
            }

            return emailMessage;
        }

        private static int GetMailPort()
        {
            var mailPort = MAILPORT;
            try
            {
                var configSetting = System.Configuration.ConfigurationManager.AppSettings["EmailPort"];
                if (configSetting != string.Empty && configSetting != null) mailPort = Convert.ToInt32(configSetting);
            }
            catch { mailPort = MAILPORT; }

            return mailPort;
        }

        private static string GetMailServer()
        {
            var mailServer = MAILSERVER;
            try
            {
                var configSetting = System.Configuration.ConfigurationManager.AppSettings["EmailServer"];
                if (configSetting != string.Empty && configSetting != null) mailServer = configSetting;
            }
            catch { mailServer = MAILSERVER; }

            return mailServer;
        }

        /// <summary>
        /// Send the email message created
        /// </summary>
        /// <param name="emailMessage"></param>
        private static void SendNewEmail(MailMessage emailMessage)
        {
            var emailServer = GetMailServer();
            var emailPort = GetMailPort();
            var smtpClient = new SmtpClient(MAILSERVER); //new SmtpClient(MAILSERVER, MAILPORT);
            smtpClient.Send(emailMessage);
        }

        #endregion Private functions
    }
}