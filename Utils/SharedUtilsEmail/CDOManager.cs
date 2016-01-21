using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mail; //I'm NOT using System.NET (The .NET System.Net.Mail.SmtpClient class cannot handle implicit SSL connection)

namespace SharedUtilsEmail
{
    /// <summary>
    /// using System.Web.Mail,
    /// The .NET System.Net.Mail.SmtpClient class cannot handle implicit SSL connection
    /// </summary>
    public class CDOManager
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MailFrom"></param>
        /// <param name="MailsTo">A semicolon-delimited list of e-mail addresses</param>
        public void SendEmail(string MailFrom, string MailsTo,
                              string Subject, string Body,
                              bool IsBodyEncoded = false,
                              bool IsBodyHtml = false,
                              List<System.Net.Mail.Attachment> Attachments = null)
        {
            var mail = new MailMessage();
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", SmtpHost);
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", SmtpPort);
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", Username);
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", Password);
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout", 180); //30 seconds default


            mail.From = MailFrom;
            mail.To = MailsTo;

            mail.Subject = Subject;

            string body = Body;
            if (IsBodyEncoded)
                body = System.Net.WebUtility.HtmlDecode(body);
            mail.Body = body;

            mail.BodyFormat = IsBodyHtml ? MailFormat.Html : MailFormat.Text;

            //allegati
            var filePathsUsati = new List<string>();
            if (Attachments != null && Attachments.Count > 0)
            {
                foreach (var attach in Attachments)
                {
                    //Scrivo il file nella path temporanea
                    var tempFilePath = Path.Combine(Path.GetTempPath(), attach.Name.MakeUnique());
                    using (var tempFileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        attach.ContentStream.Seek(0, SeekOrigin.Begin);
                        attach.ContentStream.CopyTo(tempFileStream);
                    }
                    filePathsUsati.Add(tempFilePath);

                    mail.Attachments.Add(new MailAttachment(tempFilePath));
                }
            }


            //todo H3G: System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings["PECSMTPHost"] + ":" + ConfigurationManager.AppSettings["PECSMTPPort"];
            SmtpMail.SmtpServer.Insert(0, SmtpHost);


            SmtpMail.Send(mail);

            //elimino allegati creati
            foreach (var fileToDelete in filePathsUsati)
            {
                try
                {
                    if (File.Exists(fileToDelete))
                    {
                        File.Delete(fileToDelete);
                    }
                }
                catch(Exception Exc)
                {
                    Trace.TraceError("CDOManager, SendEmail, Delete attachments files Exc: " + Exc.ToString());
                }
            }
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// FileName_dd-MM-yyyy-ss-fff.estensione
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string MakeUnique(this string FileName)
        {
            var estensione = Path.GetExtension(FileName);
            var senzaEstensione = Path.GetFileNameWithoutExtension(FileName);
            var stringaUnivoca = DateTime.Now.ToString("dd-MM-yyyy-ss-fff");

            var nuovoFile = senzaEstensione.Trim() + "_" + stringaUnivoca + estensione;
            return nuovoFile;
        }
    }
}
