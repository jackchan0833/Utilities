using JC.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the error information.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// The error code.
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// Constructs the <see cref="ErrorInfo"/>.
        /// </summary>
        public ErrorInfo() { }
        /// <summary>
        /// Constructs the <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMsg">The error message.</param>
        public ErrorInfo(string errorCode, string errorMsg) { Code = errorCode; Message = errorMsg; }
    }
}
