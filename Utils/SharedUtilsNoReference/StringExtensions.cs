using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class StringExtensions
    {
        public static string TrimNulls(this string Stringa)
        {
            if (Stringa == null)
                return string.Empty;
            return Stringa.Trim();
        }

        public static string TrimNulls(this JToken DataValue)
        {
            if (DataValue != null)
                return DataValue.ToObject<string>().Trim();
            return string.Empty;
        }

        public static bool AnyNull(params string[] args)
        {
            return args.Where(p => string.IsNullOrWhiteSpace(p)).Any();
        }




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
