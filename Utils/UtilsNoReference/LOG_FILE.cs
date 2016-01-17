using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsNoReference
{
    //todo: async con FileIO
    public static class LOG_FILE
    {
        private static readonly object threadLock = new object();

        public static void WriteLine(string filename, string msg)
        {
            lock (threadLock)
            {
                string log_path = AppDomain.CurrentDomain.BaseDirectory + filename + ".txt";
                string new_line = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + msg + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(log_path, new_line);
            }
        }
        public static void WriteLine(string msg)
        {
            lock (threadLock)
            {
                string log_path = AppDomain.CurrentDomain.BaseDirectory + "Log_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt";
                string new_line = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + msg + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(log_path, new_line);
            }
        }

        public static void Write(Exception Exception, string TitoloEccezione)
        {
            string testo = DateTime.Now.ToLongTimeString() + Environment.NewLine +
                           TitoloEccezione + Environment.NewLine +
                           Exception.ToString() + Environment.NewLine + Environment.NewLine;


            string nomeFile = "Log_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeFile);

            lock (threadLock)
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, testo);
                }
                else
                {
                    File.AppendAllText(path, testo);
                }
            }
        }
    }
}
