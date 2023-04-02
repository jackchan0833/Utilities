using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.Net
{
    /// <summary>
    /// The sample to use TCPClientHelper class.
    /// </summary>
    internal class TCPClientSample
    {
        public void Main()
        {
            TCPClientHelper tcpClientHelper = new TCPClientHelper("127.0.0.1", 622);
            tcpClientHelper.ServerConnected += new Action<Socket>((client) =>
            {
                IPEndPoint iep = client.RemoteEndPoint as IPEndPoint;
                string key = iep.ToString() + ":" + iep.Port;
                Console.WriteLine($"Connected to {0}.", key);
            });
            tcpClientHelper.DataSent += new Action<Socket, string>((client, sendId) =>
            {
                IPEndPoint iep = client.RemoteEndPoint as IPEndPoint;
                string key = iep.ToString() + ":" + iep.Port;
                Console.WriteLine($"{0}: send a message to {1}.", DateTime.Now, key);
            });
            tcpClientHelper.ServerDisconnected += new Action<string, int, StateObject, SocketActionState>((ip, port, stateObject, state) =>
            {
                string key = ip + ":" + port; 
                Console.WriteLine("Closed.");
            });

            tcpClientHelper.DataReceived += new Action<Socket, byte[], int>((client, buffer, len) =>
            {
                var data = new byte[len];
                Buffer.BlockCopy(buffer, 0, data, 0, len);
                string msg = Encoding.UTF8.GetString(data);
                IPEndPoint iep = client.RemoteEndPoint as IPEndPoint;
                string key = iep.ToString() + ":" + iep.Port;
                Console.WriteLine("received msg from {0}: {1}.", key, msg);
            });
            tcpClientHelper.ErrorOccurred += new Action<StateObject, SocketActionState, Exception>((stateObject, socketActionState, ex) =>
            {
                Console.WriteLine("Error occurred: " + ex.ToString());
            });

            tcpClientHelper.Connect();
            tcpClientHelper.Send("Hello world!");
            tcpClientHelper.Close();
        }
    }
}
