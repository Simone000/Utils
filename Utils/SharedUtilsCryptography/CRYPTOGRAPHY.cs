using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsCryptography
{
    public static class CRYPTOGRAPHY
    {
        /*
         * -SHA1: recommended for the best encryption strength.
         * -MD5 (Message Digest 5): better performance
         * AES (Rijndael)
         * 3DES (TripleDES): applica tre volte consecutive il DES (Data Encryption Standard).
        */



        //usabile per creare la MachineKey se lunga 40-128 caratteri
        public static string CreateKey_Hexadecimal(int length)
        {
            byte[] random_bytes = new byte[length / 2]; //ogni carattere usa 2 byte

            // Create a cryptographically strong random number generator and fill the byte array
            RNGCryptoServiceProvider crypt_rng = new RNGCryptoServiceProvider();
            crypt_rng.GetBytes(random_bytes);

            //converto in esadecimale
            return BitConverter.ToString(random_bytes).Replace("-", string.Empty);
        }
    }
}
