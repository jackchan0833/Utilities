using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace JC.Utilities.RSA
{
    /// <summary>
    /// Represents the <see cref="RSAHandler"/>.
    /// </summary>
    public class RSAHandler
    {
        /// <summary>
        /// Generates RSA secret key.
        /// </summary>
        /// <param name="keySize">the size of the key,must from 384 bits to 16384 bits in increments of 8 </param>
        /// <returns></returns>
        public static RSASecretKey GenerateRSASecretKey(int keySize)
        {
            var rsaKey = new RSASecretKey();
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
            {
                rsaKey.PrivateKey = rsa.ToXmlString(true);
                rsaKey.PublicKey = rsa.ToXmlString(false);
                rsaKey.Base64PrivateKey = RSAKeyConverter.ConvertToBase64FromXmlPrivateKey(rsaKey.PrivateKey);
                rsaKey.Base64PublicKey = RSAKeyConverter.ConvertToBase64FromXmlPublicKey(rsaKey.PublicKey);
            }
            return rsaKey;
        }
        /// <summary>
        /// Encrypts the content with specified public key.
        /// </summary>
        /// <param name="xmlPublicKey">The public key that is XML format.</param>
        /// <param name="content">The content to encrypt.</param>
        /// <returns>The encrypt result.</returns>
        public static string RSAEncrypt(string xmlPublicKey, string content)
        {
            string encryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                encryptedContent = Convert.ToBase64String(encryptedData);
            }
            return encryptedContent;
        }

        /// <summary>
        /// Decrptys the content with specified private key.
        /// </summary>
        /// <param name="xmlPrivateKey">The specified private key.</param>
        /// <param name="content">The content to decrypt.</param>
        /// <returns>The decrpty result.</returns>
        public static string RSADecrypt(string xmlPrivateKey, string content)
        {
            string decryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(content), false);
                decryptedContent = Encoding.UTF8.GetString(decryptedData);
            }
            return decryptedContent;
        }
        /// <summary>
        /// Gets the RSA sign with specified private key.
        /// </summary>
        /// <param name="xmlPrivateKey">The specified private key.</param>
        /// <param name="content">The content to generate RSA sign.</param>
        /// <returns>The generated RSA sign.</returns>
        public static string GenerateRSASign(string xmlPrivateKey, string content)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                byte[] data = Encoding.UTF8.GetBytes(content);
                var byteSign = rsa.SignData(data, "SHA256");
                string sign = BufferHandler.ConvertToHexString(byteSign);
                return sign;
            }
        }

        /// <summary>
        /// Verify the content and sign by specified public key.
        /// </summary>
        /// <param name="xmlPublicKey">The specified public key.</param>
        /// <param name="content">Then content to verify.</param>
        /// <param name="sign">Then sign to verify.</param>
        /// <returns>The verify result.</returns>
        public static bool VerifyRSASign(string xmlPublicKey, string content, string sign)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                if (!StringHandler.TryConvertFromHexString(sign, out byte[] signBytes))
                {
                    return false;
                }
                byte[] data = Encoding.UTF8.GetBytes(content);
                return rsa.VerifyData(data, "SHA256", signBytes);
            }
        }
    }
}
