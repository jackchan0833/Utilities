using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JC.Utilities.Constants
{
    /// <summary>
    /// Represents the messages.
    /// </summary>
    public class Messages
    {
        public static MessageInfo ListDefaultEmptyOption = new MessageInfo("ListDefaultOption", "--Select--");
        public static MessageInfo Second = new MessageInfo("Second", "second");
        public static MessageInfo Millisecond = new MessageInfo("Millisecond", "millisecond");
    }
}
