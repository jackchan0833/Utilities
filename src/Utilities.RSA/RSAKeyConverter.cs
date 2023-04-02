using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;

namespace JC.Utilities.RSA
{
    /// <summary>
    /// Represents the <see cref="RSAKeyConverter"/>.
    /// </summary>
    public class RSAKeyConverter
    {
        /// <summary>
        /// Convert the xml private key to base64 private key string. (Example: Java is using base64.)
        /// </summary>
        /// <param name="xmlPrivateKey">The specified private key with XML format.</param>
        /// <returns>The base64 private key.</returns>
        public static string ConvertToBase64FromXmlPrivateKey(string xmlPrivateKey)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("-----BEGIN PRIVATE KEY-----");
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                RSAParameters param = rsa.ExportParameters(true);
                RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(
                    new BigInteger(1, param.Modulus), new BigInteger(1, param.Exponent),
                    new BigInteger(1, param.D), new BigInteger(1, param.P),
                    new BigInteger(1, param.Q), new BigInteger(1, param.DP),
                    new BigInteger(1, param.DQ), new BigInteger(1, param.InverseQ));
                PrivateKeyInfo privateKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);

                string content = Convert.ToBase64String(privateKey.ToAsn1Object().GetEncoded());
                var listSplit = StringHandler.SplitString(content, 60);
                foreach (var item in listSplit)
                {
                    result.AppendLine(item);
                }

            }
            result.AppendLine("-----END PRIVATE KEY-----");
            return result.ToString();
        }

        /// <summary>
        /// Convert the xml public key to base64 public key string.
        /// </summary>
        /// <param name="xmlPublicKey">The public key with XML format.</param>
        /// <returns>The base64 public key.</returns>
        public static string ConvertToBase64FromXmlPublicKey(string xmlPublicKey)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("-----BEGIN PUBLIC KEY-----");
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                RSAParameters p = rsa.ExportParameters(false);
                RsaKeyParameters keyParams = new RsaKeyParameters(
                    false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent));
                SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyParams);
                var content = Convert.ToBase64String(publicKeyInfo.ToAsn1Object().GetEncoded());
                var listSplit = StringHandler.SplitString(content, 60);
                foreach (var item in listSplit)
                {
                    result.AppendLine(item);
                }
            }
            result.AppendLine("-----END PUBLIC KEY-----");
            return result.ToString();
        }

        /// <summary>
        /// Convert the base64 private key string to xml private key.
        /// </summary>
        /// <param name="privateKey">The base64 private key.</param>
        /// <returns>The XML private key.</returns>
        public static string ConvertToXmlFromBase64PrivateKey(string privateKey)
        {
            privateKey = privateKey.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "")
                .Replace("\r", "").Replace("\n", "").Trim();
            RsaPrivateCrtKeyParameters privateKeyParams =
                PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey)) as RsaPrivateCrtKeyParameters;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                RSAParameters rsaParams = new RSAParameters()
                {
                    Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned(),
                    Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned(),
                    D = privateKeyParams.Exponent.ToByteArrayUnsigned(),
                    DP = privateKeyParams.DP.ToByteArrayUnsigned(),
                    DQ = privateKeyParams.DQ.ToByteArrayUnsigned(),
                    P = privateKeyParams.P.ToByteArrayUnsigned(),
                    Q = privateKeyParams.Q.ToByteArrayUnsigned(),
                    InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParams);
                return rsa.ToXmlString(true);
            }
        }

        /// <summary>
        /// Convert base64 public key string to xml public key.
        /// </summary>
        /// <param name="publicKey">The base64 public key.</param>
        /// <returns>The XML public key.</returns>
        public static string ConvertToXmlFromBase64PublicKey(string publicKey)
        {
            publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "")
                .Replace("\r", "").Replace("\n", "").Trim();
            RsaKeyParameters p =
                PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey)) as RsaKeyParameters;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                RSAParameters rsaParams = new RSAParameters
                {
                    Modulus = p.Modulus.ToByteArrayUnsigned(),
                    Exponent = p.Exponent.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParams);
                return rsa.ToXmlString(false);
            }
        }
    }
}
