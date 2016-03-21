using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class STRINGS
    {
        public static string TrimNull(this string Stringa)
        {
            if (Stringa == null)
                return string.Empty;
            return Stringa.Trim();
        }

        public static string TrimNull(this JToken DataValue)
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
        /// FileName_yyyy-MM-dd-ss-fff.estensione
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string MakeUnique(this string FileName)
        {
            var estensione = Path.GetExtension(FileName);
            var senzaEstensione = Path.GetFileNameWithoutExtension(FileName);
            var stringaUnivoca = DateTime.Now.ToString("yyyy-MM-dd-ss-fff");

            var nuovoFile = senzaEstensione.Trim() + "_" + stringaUnivoca + estensione;
            return nuovoFile;
        }
    }
}
