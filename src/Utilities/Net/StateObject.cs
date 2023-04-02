using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.Net
{
    /// <summary>
    /// Represents the socket state object.
    /// </summary>
    public class StateObject
    {
        /// <summary>
        /// The transaction id to execute.
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// The socket client.
        /// </summary>
        public Socket Client { get; set; }
        /// <summary>
        /// The socket receive buffer. Default is 10Mb.
        /// </summary>
        public byte[] ReceiveBuffer { get; set; } = new byte[1024 * 1024 * 10];
    }
}
