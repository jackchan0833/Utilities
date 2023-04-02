using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities.RSA
{
    /// <summary>
    /// Represents the RSA secret with private an public key.
    /// </summary>
    public class RSASecretKey
    {
        /// <summary>
        /// The public key with XML format.
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// The private key with XML format.
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// The public key with base64.
        /// </summary>
        public string Base64PublicKey { set; get; }
        /// <summary>
        /// The private key with base64.
        /// </summary>
        public string Base64PrivateKey { set; get; }
    }

}
