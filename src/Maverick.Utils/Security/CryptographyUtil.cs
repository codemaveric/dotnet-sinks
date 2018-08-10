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
            using (RijndaelManaged aes = new RijndaelManaged())
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
                        //encryptedText = Convert.ToBase64String(ms.ToArray());
                        foreach (var item in ms.ToArray())
                        {
                            encryptedText += item.ToString("x2");
                        }
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

            var fullCipher = Encoding.UTF8.GetBytes(encryptedText);
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

            string plaintText = string.Empty;
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                var decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream ms = new MemoryStream(cipher))
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
