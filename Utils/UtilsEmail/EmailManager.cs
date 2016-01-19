using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace UtilsEmail
{
    public class EmailManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpClient"></param>
        /// <param name="MailFrom_Address"></param>
        /// <param name="MailTo_Address"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="IsBodyEncoded"></param>
        /// <param name="IsBodyHtml"></param>
        /// <param name="Priority"></param>
        /// <param name="Attachments">DisposableList<Attachment></param>
        /// <returns></returns>
        public static async Task InviaEmailAsync(SmtpClient smtpClient,
                                     MailAddress MailFrom_Address, List<MailAddress> MailTo_Address,
                                     string Subject, string Body,
                                     bool IsBodyEncoded = false,
                                     bool IsBodyHtml = false,
                                     MailPriority Priority = MailPriority.Normal,
                                     List<Attachment> Attachments = null)
        {
            using (MailMessage mailMsg = new MailMessage())
            {
                mailMsg.From = MailFrom_Address;
                foreach (var to in MailTo_Address)
                {
                    mailMsg.To.Add(to);
                }

                mailMsg.Subject = Subject;

                string body = Body;
                if (IsBodyEncoded)
                    body = System.Net.WebUtility.HtmlDecode(body);
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = IsBodyHtml;
                mailMsg.Priority = Priority;

                //allegati
                if (Attachments != null && Attachments.Count > 0)
                    foreach (var attach in Attachments)
                        mailMsg.Attachments.Add(attach);

                await smtpClient.SendMailAsync(mailMsg).ConfigureAwait(false);
            }
        }
    }
}
