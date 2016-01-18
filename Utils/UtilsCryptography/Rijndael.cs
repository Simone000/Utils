using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCryptography
{
    public static class Rijndael
    {
       //var testo = "quante bello andare al mare";
       //var password = "Ciaooooo";
       //
       //var password_SHA256 = CalculateHash256(password);
       //var testoEncrypted = Encrypt(testo, password_SHA256);
       //
       //Console.WriteLine(Decrypt(testoEncrypted, password_SHA256));



        public static byte[] Encrypt(string testo, byte[] Password_SHA256)
        {
            using (var aesAlg = new RijndaelManaged())
            {
                aesAlg.Mode = CipherMode.ECB; //non usa IV

                aesAlg.Key = Password_SHA256;

                using (var encryptor = aesAlg.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(testo);
                            swEncrypt.Flush();
                            csEncrypt.FlushFinalBlock();
                            return msEncrypt.ToArray();
                        }
                    }
                }
            }
        }

        public static string Decrypt(byte[] testo, byte[] Password_SHA256)
        {
            using (var aesAlg = new RijndaelManaged())
            {
                aesAlg.Mode = CipherMode.ECB; //non usa IV

                aesAlg.Key = Password_SHA256;

                using (var decryptor = aesAlg.CreateDecryptor())
                using (var msEncrypt = new MemoryStream(testo))
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csEncrypt))
                        {
                            string ris = srDecrypt.ReadToEnd();
                            return ris;
                        }
                    }
                }
            }
        }


        public static byte[] CalculateHash256(string Password)
        {
            using (var SHA256 = SHA256Managed.Create())
            {
                var passBytes = UnicodeEncoding.UTF8.GetBytes(Password);
                return SHA256.ComputeHash(passBytes);
            }
        }
    }
}
