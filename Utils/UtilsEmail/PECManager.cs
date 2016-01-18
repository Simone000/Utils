using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsNoReference;

namespace UtilsEmail
{
    public class PECManager
    {
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string from { get; set; }
        public System.Web.Mail.MailFormat Formato { get; set; }

        //The .NET System.Net.Mail.SmtpClient class cannot handle implicit SSL connection
        public void SendMail_CDO(string DestinatarioPEC, string Subject, string Body, DisposableList<MailAttachmentModel> Attachments = null)
        {
            System.Web.Mail.MailMessage newMail = new System.Web.Mail.MailMessage();
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpHost);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", smtpPort);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", username);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");

            newMail.From = from;
            newMail.BodyFormat = Formato;

            newMail.To = DestinatarioPEC;

            newMail.Subject = Subject;
            newMail.Body = Body;

            if (Attachments != null)
            {
                foreach (var attach in Attachments)
                {
                    var tempFilePath = Path.Combine(Path.GetTempPath(), attach.FileName);
                    using (var tempFileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        attach.FileStream.Seek(0, SeekOrigin.Begin);
                        attach.FileStream.CopyTo(tempFileStream);
                    }
                    newMail.Attachments.Add(new System.Web.Mail.MailAttachment(tempFilePath));
                }
            }
            System.Web.Mail.SmtpMail.SmtpServer.Insert(0, smtpHost);
            System.Web.Mail.SmtpMail.Send(newMail);
        }

        public void SendMail_CDO(List<string> DestinatariPEC, string Subject, string Body, DisposableList<MailAttachmentModel> Attachments = null)
        {
            System.Web.Mail.MailMessage newMail = new System.Web.Mail.MailMessage();
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpHost);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", smtpPort);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", username);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password);
            newMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");

            newMail.From = from;
            newMail.BodyFormat = Formato;

            newMail.To = string.Join(";", DestinatariPEC);

            newMail.Subject = Subject;
            newMail.Body = Body;

            if (Attachments != null)
            {
                foreach (var attach in Attachments)
                {
                    var tempFilePath = Path.Combine(Path.GetTempPath(), attach.FileName);
                    using (var tempFileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        attach.FileStream.Seek(0, SeekOrigin.Begin);
                        attach.FileStream.CopyTo(tempFileStream);
                    }
                    newMail.Attachments.Add(new System.Web.Mail.MailAttachment(tempFilePath));
                }
            }

            System.Web.Mail.SmtpMail.SmtpServer.Insert(0, smtpHost);
            System.Web.Mail.SmtpMail.Send(newMail);
        }
    }
}
