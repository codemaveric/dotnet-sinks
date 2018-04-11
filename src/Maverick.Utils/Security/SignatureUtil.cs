using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Maverick.Utils.Security
{
    public class SignatureUtil
    {
        public static string GenerateSHA256Signature(string input)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] signature = Encoding.UTF8.GetBytes(input);

            var signatureHash = sha256.ComputeHash(signature);
            return Encoding.UTF8.GetString(signatureHash);
        }

        public static string GenerateHMACSignature(string input, string secret)
        {
            var secretKeyByteArray = Convert.FromBase64String(secret);

            byte[] signature = Encoding.UTF8.GetBytes(input);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                return Encoding.UTF8.GetString(signatureBytes);
            }
        }
    }
}
