using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsCosiAPI
{
    //todo: swagger
    public static class PdfConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="HtmlHeader"></param>
        /// <param name="HtmlFooter"></param>
        /// <param name="BaseUrl"></param>
        /// <param name="urlHtmlToPdf">http://cosimainworker.cloudapp.net/api/Pdf/HtmlToPdf</param>
        /// <param name="Key">SHA1 of password</param>
        /// <returns></returns>
        public static async Task<byte[]> ToPdfBytesAsync(string Html, string HtmlHeader, string HtmlFooter, string BaseUrl,
                                                         string urlHtmlToPdf = @"http://cosimainworker.cloudapp.net/api/Pdf/HtmlToPdf",
                                                         string Key = "")
        {
            using (var client = new HttpClient())
            {
                var pdfModel = new HtmlToPdfModel()
                {
                    Html = Html,
                    HtmlHeader = HtmlHeader,
                    HtmlFooter = HtmlFooter,
                    BaseUrl = BaseUrl
                };
                var pdfContent = new StringContent(JsonConvert.SerializeObject(pdfModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urlHtmlToPdf, pdfContent).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }
    }

    public class HtmlToPdfModel
    {
        public string Html { get; set; }
        public string HtmlHeader { get; set; }
        public string HtmlFooter { get; set; }
        public string BaseUrl { get; set; }
    }
}
