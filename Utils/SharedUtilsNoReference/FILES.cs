using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class FILES
    {
        public static async Task<string> ReadAllTextAsync(string Path, Encoding Encoding)
        {
            using (StreamReader sr = new StreamReader(Path, Encoding))
                return await sr.ReadToEndAsync().ConfigureAwait(false);
        }

        public static async Task<byte[]> ReadAllBytesAsync(string Path)
        {
            using (var file = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                byte[] buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length).ConfigureAwait(false);
                return buff;
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static void ForceDeleteDirectory(string path)
        {
            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            directory.Delete(true);
        }

        public static void Prepend(string file, string text)
        {
            string currentContent = String.Empty;
            if (File.Exists(file))
            {
                currentContent = File.ReadAllText(file);
            }
            File.SetAttributes(file, FileAttributes.Normal);
            File.WriteAllText(file, text + currentContent);
        }
        
        public static string MakeValidFileName(string filename_originale)
        {
            string caratteriInvalidi = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format("[{0}]+", caratteriInvalidi); //manca @ prima della stringa
            string filename_nuovo = Regex.Replace(filename_originale, invalidReStr, "_").Replace(";", "").Replace(",", "");
            return filename_nuovo;
        }
    }
}
