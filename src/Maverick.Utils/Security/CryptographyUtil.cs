using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Maverick.Utils.Security
{
    public class CryptographyUtil
    {
        public static string EncryptAES(string plaintext, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException(nameof(plaintext));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            string encryptedText = string.Empty;
            using (AesManaged aes = new AesManaged())
            {
                var encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plaintext);
                        }
                        encryptedText = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            return encryptedText;
        }

        public static string DecryptAES(string encryptedText, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentNullException(nameof(encryptedText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            string plaintText = string.Empty;
            using (AesManaged aes = new AesManaged())
            {
                var decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(encryptedText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sw = new StreamReader(cs))
                        {
                            plaintText = sw.ReadToEnd();
                        }
                    }
                }
            }
            return plaintText;
        }

    }
}
