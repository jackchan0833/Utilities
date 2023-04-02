using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the message information.
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// The message category.
        /// </summary>
        public string Category { set; get; }
        /// <summary>
        /// The message code.
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// The message text.
        /// </summary>
        public string Text { set; get; }
        /// <summary>
        /// The message language.
        /// </summary>
        public string Language { set; get; }
        /// <summary>
        /// Constructs the <see cref="MessageInfo"/>.
        /// </summary>
        public MessageInfo()
        {
        }
        /// <summary>
        /// Constructs the <see cref="MessageInfo"/>.
        /// </summary>
        public MessageInfo(string code, string text, string category = "message")
        {
            Code = code;
            Text = text;
            Category = category;
        }
    }
}
