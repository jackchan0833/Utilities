using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JC.Utilities.Net
{
    /// <summary>
    /// Represents the TCP socket server.
    /// </summary>
    public class TCPServerHelper
    {
        private ManualResetEvent _SocketConnected = new ManualResetEvent(false);
        private ManualResetEvent _DoReceive = new ManualResetEvent(false);
        /// <summary>
        /// The server lisenser state.
        /// </summary>
        public bool IsStarted { get; private set; } = false;
        /// <summary>
        /// The connected tcp clients.
        /// </summary>
        public ConcurrentDictionary<string, Socket> Clients { get; private set; } = new ConcurrentDictionary<string, Socket>();
        /// <summary>
        /// The event when a client connected.
        /// </summary>
        public event Action<Socket> ClientConnected;
        /// <summary>
        /// The event when a client disconnected.
        /// </summary>
        public event Action<string, int> ClientDisconnected;
        /// <summary>
        /// The event when receive data from client.
        /// </summary>
        public event Action<Socket, byte[], int> DataReceived;
        /// <summary>
        /// The event when data has been sent to client.
        /// </summary>
        public event Action<Socket> DataSent;
        /// <summary>
        /// The event when error has been occurred on socket connection.
        /// </summary>
        public event Action<TcpListener, SocketActionState, Exception> ErrorOccurred;
        /// <summary>
        /// The server tcp listenser.
        /// </summary>
        public TcpListener Listenser { get; private set; }
        /// <summary>
        /// The local server ip.
        /// </summary>
        public string ListenserIPAddr { get; private set; }
        /// <summary>
        /// The server listen port.
        /// </summary>
        public int ListenserPort { get; private set; }
        /// <summary>
        /// Constructs the <see cref="TCPServerHelper"/>.
        /// </summary>
        public TCPServerHelper()
        {
        }
        /// <summary>
        /// Constructs the <see cref="TCPServerHelper"/> with specified server port.
        /// </summary>
        /// <param name="port">The server port to listen</param>
        public void Start(int port)
        {
            Start("127.0.0.1", port);
        }
        /// <summary>
        /// Start to listen the port.
        /// </summary>
        /// <param name="listenserIP">The local server ip.</param>
        /// <param name="port">The server port.</param>
        public void Start(string listenserIP, int port)
        {
            this.ListenserIPAddr = "127.0.0.1";
            this.ListenserPort = port;
            IPAddress ipAddr = IPAddress.Parse(listenserIP);
            Listenser = new TcpListener(new IPEndPoint(ipAddr, port));
            Listenser.Start();
            IsStarted = true;

            ThreadPool.QueueUserWorkItem((x) =>
            {
                try
                {
                    while (IsStarted)
                    {
                        _SocketConnected.Reset();
                        Listenser.BeginAcceptSocket(new AsyncCallback(AcceptCallBack), Listenser);
                        _SocketConnected.WaitOne();
                    }
                }
                catch (Exception ex)
                {
                    if (!HandleErrorOccurred(Listenser, SocketActionState.Error, ex))
                    {
                        throw;
                    }
                }
            });
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            if (!IsStarted) return;
            TcpListener tcpListener = (TcpListener)ar.AsyncState;
            Socket client = null;
            try
            {
                client = tcpListener.EndAcceptSocket(ar);
            }
            catch (Exception ex)
            {
                if (!HandleErrorOccurred(Listenser, SocketActionState.Error, ex))
                {
                    throw;
                }
                return;
            }
            finally
            {
                _SocketConnected.Set();
            }
            RaiseClientConnected(client);

            string key = GetClientKey(client.RemoteEndPoint);
            if (Clients.TryAdd(key, client))
            {
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    try
                    {
                        while (IsStarted && Clients.ContainsKey(key))
                        {
                            Thread.Sleep(20);
                            ReceiveAsync(client);
                            Thread.Sleep(20);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!HandleErrorOccurred(Listenser, SocketActionState.Receive, ex))
                        {
                            throw;
                        }
                    }
                });
            }
        }
        /// <summary>
        /// Send specified message to specified client. The default message encoding is UTF8.
        /// </summary>
        /// <param name="clientKey">The client socket key.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="transactionId">The send transaction.</param>
        public void Send(string clientKey, string msg, string transactionId)
        {
            Send(clientKey, msg, Encoding.UTF8, transactionId);
        }
        /// <summary>
        /// Send specified message to specified client.
        /// </summary>
        /// <param name="clientKey">The client socket key.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="encoding">The encoding for message to send.</param>
        /// <param name="transactionId">The send transaction.</param>
        public void Send(string clientKey, string msg, Encoding encoding, string transactionId)
        {
            if (!Clients.TryGetValue(clientKey, out Socket client))
            {
                throw new InvalidOperationException("Cannot find the client socket.");
            }
            Send(client, msg, encoding, transactionId);
        }
        /// <summary>
        /// Send specified message to specified client. The message encoding is UTF8.
        /// </summary>
        /// <param name="client">The client to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="transactionId">The send transaction.</param>
        public void Send(Socket client, string msg, string transactionId)
        {
            Send(client, msg, Encoding.UTF8, transactionId);
        }
        /// <summary>
        /// Send specified message to specified client.
        /// </summary>
        /// <param name="client">The client to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="encoding">The encoding for message to send.</param>
        /// <param name="transactionId">The send transaction.</param>
        public void Send(Socket client, string msg, Encoding encoding, string transactionId)
        {
            byte[] data = encoding.GetBytes(msg);
            StateObject stateObject = new StateObject()
            {
                Client = client,
                TransactionId = transactionId
            };
            client.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, stateObject);
        }
        /// <summary>
        /// Send specified data to specified client.
        /// </summary>
        /// <param name="clientKey">The specified client key.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="transactionId">The send transaction id.</param>
        public void Send(string clientKey, byte[] data, string transactionId)
        {
            if (!Clients.TryGetValue(clientKey, out Socket client))
            {
                throw new InvalidOperationException("Cannot find the client socket.");
            }
            Send(client, data, transactionId);
        }
        /// <summary>
        /// Send specified data to specified client.
        /// </summary>
        /// <param name="client">The specified client.</param>>
        /// <param name="data">The data to send.</param>
        /// <param name="transactionId">The send transaction id.</param>
        public void Send(Socket client, byte[] data, string transactionId)
        {
            StateObject stateObject = new StateObject()
            {
                Client = client,
                TransactionId = transactionId
            };
            client.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, stateObject);
        }
        private void SendCallBack(IAsyncResult ar)
        {
            if (!IsStarted) return;
            var stateObj = (StateObject)ar.AsyncState;
            Socket client = stateObj.Client;
            RaiseDataSent(client);
        }
        private string GetClientKey(EndPoint ep)
        {
            var iep = (IPEndPoint)ep;
            return string.Format("{0}:{1}", iep.Address.ToString(), iep.Port);
        }
        /// <summary>
        /// Gets connected client's key.
        /// </summary>
        /// <param name="client">The connected client.</param>
        /// <returns>The client key.</returns>
        public string GetClientKey(Socket client)
        {
            if (client == null)
            {
                return string.Empty;
            }
            return GetClientKey(client.LocalEndPoint);
        }
        private void ReceiveAsync(Socket client)
        {
            _DoReceive.Reset();
            if (client.Connected)
            {
                var stateObj = new StateObject();
                stateObj.Client = client;
                try
                {
                    client.BeginReceive(stateObj.ReceiveBuffer, 0, stateObj.ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, stateObj);
                    _DoReceive.WaitOne();
                }
                catch (Exception ex)
                {
                    if (!HandleErrorOccurred(Listenser, SocketActionState.Receive, ex))
                    {
                        throw;
                    }
                }
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            StateObject stateObj = null;
            int count = -1;
            try
            {
                stateObj = (StateObject)ar.AsyncState;
                count = stateObj.Client.EndReceive(ar);
            }
            catch (Exception ex)
            {
                if (stateObj != null)
                {
                    CloseClient(stateObj.Client);
                }
                if (!HandleErrorOccurred(Listenser, SocketActionState.Receive, ex))
                {
                    throw;
                }
                return;
            }
            finally
            {
                _DoReceive.Set();
            }
            if (count > 0)
            {
                RaiseDataReceived(stateObj.Client, stateObj.ReceiveBuffer, count);
            }
            else
            {
                CloseClient(stateObj.Client);
            }
        }
        /// <summary>
        /// Close specified client.
        /// </summary>
        /// <param name="clientKey">The specified client key.</param>
        public void CloseClient(string clientKey)
        {
            if (Clients.TryGetValue(clientKey, out var client))
            {
                CloseClient(client);
            }
        }
        /// <summary>
        /// Close specified client.
        /// </summary>
        /// <param name="client">The specified client.</param>
        public void CloseClient(Socket client)
        {
            if (client != null)
            {
                string key = GetClientKey(client.LocalEndPoint);
                Clients.TryRemove(key, out var existClient);
                if (client.Connected)
                {
                    var iep = (IPEndPoint)client.LocalEndPoint;
                    client.Close();

                    string clientIp = iep.Address.ToString();
                    int clientPort = iep.Port;
                    RaiseClientDisconnected(clientIp, clientPort);
                }
            }
        }
        /// <summary>
        /// Close the tcp server.
        /// </summary>
        public void Close()
        {
            IsStarted = false;
            Listenser.Stop();
            Clients.Clear();
        }
        private void RaiseClientDisconnected(string clientIP, int clientPort)
        {
            if (this.ClientDisconnected != null)
            {
                this.ClientDisconnected(clientIP, clientPort);
            }
        }
        private void RaiseClientConnected(Socket tcpClient)
        {
            if (this.ClientConnected != null)
            {
                this.ClientConnected(tcpClient);
            }
        }
        private void RaiseDataReceived(Socket tcpClient, byte[] buffer, int receivedLength)
        {
            if (this.DataReceived != null)
            {
                this.DataReceived(tcpClient, buffer, receivedLength);
            }
        }
        private void RaiseDataSent(Socket tcpClient)
        {
            if (this.DataSent != null)
            {
                this.DataSent(tcpClient);
            }
        }
        private bool HandleErrorOccurred(TcpListener tcpListener, SocketActionState state, Exception ex)
        {
            if (ErrorOccurred != null)
            {
                ErrorOccurred(tcpListener, state, ex);
                return true;
            }
            return false;
        }
    }
}
