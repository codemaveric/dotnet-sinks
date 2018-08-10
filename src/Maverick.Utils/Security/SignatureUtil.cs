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
            string signatureString = "";
            foreach (var item in signatureHash)
            {
                signatureString += item.ToString("x2");
            }
            return signatureString;
        }

        public static string GenerateHMACSignature(string input, string secret)
        {
            var secretKeyByteArray = Convert.FromBase64String(secret);
            byte[] signature = Encoding.UTF8.GetBytes(input);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
               
                return Convert.ToBase64String(signatureBytes);
            }
        }

        public static bool VerifyHMACSignature(string input, string compareSignature, string secret)
        {
            var secretKeyByteArray = Convert.FromBase64String(secret);
            //var secretKeyByteArray = Encoding.UTF8.GetBytes(secret);

            byte[] signature = Encoding.UTF8.GetBytes(input);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                var base64String = Convert.ToBase64String(signatureBytes);

                return (compareSignature.Equals(base64String, StringComparison.Ordinal));
            }
        }
    }
}
