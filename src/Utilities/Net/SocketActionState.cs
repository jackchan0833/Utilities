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
    /// Represents the socket execute action state.
    /// </summary>
    public enum SocketActionState
    {
        Connect = 1,
        Send,
        Receive,
        Close,
        Error
    }
}
