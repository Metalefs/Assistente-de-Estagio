using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ADE.Utilidades.Seguranca
{
    public static class Criptografia
    {
        private static string StartingKey = "HR$OdIjSR$2pIj12";

        public static string Criptografar(string Texto, byte[] Key)
        {
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                aes.Padding = PaddingMode.Zeros;
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, Encoding.UTF8.GetBytes(StartingKey));
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(Texto);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted); ;
        }

        public static string Decriptografar(byte[] cipherText, byte[] Key)
        {

            string plaintext = null;

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = Key;
                aes.Padding = PaddingMode.Zeros;
                aes.IV = Encoding.ASCII.GetBytes(StartingKey);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

    }
}
