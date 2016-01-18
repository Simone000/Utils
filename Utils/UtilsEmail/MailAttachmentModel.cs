using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsEmail
{
    public class MailAttachmentModel : IDisposable
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }

        public void Dispose()
        {
            if (this.FileStream != null)
            {
                this.FileStream.Dispose();
            }
        }
    }
}
