using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JC.Utilities.Net
{
    /// <summary>
    /// Represents the TCP client helper extension.
    /// </summary>
    public class TCPClientHelper : IDisposable
    {
        private bool disposed = false;
        /// <summary>
        /// The current tcp socket client.
        /// </summary>
        public Socket Client { get; private set; }
        /// <summary>
        /// The event when error has been occurred.
        /// </summary>
        public event Action<StateObject, SocketActionState, Exception> ErrorOccurred;
        /// <summary>
        /// The event when connected to server.
        /// </summary>
        public event Action<Socket> ServerConnected;
        /// <summary>
        /// The event when disconnected with server.
        /// </summary>
        public event Action<string, int, StateObject, SocketActionState> ServerDisconnected;
        /// <summary>
        /// The event when data has been received from server.
        /// </summary>
        public event Action<Socket, byte[], int> DataReceived;
        /// <summary>
        /// The event when data has been sent to server.
        /// </summary>
        public event Action<Socket, string> DataSent;
        /// <summary>
        /// The socket client state whether is closed.
        /// </summary>
        public bool IsClosed { get; private set; } = false;
        /// <summary>
        /// The socket client state whether has connected to server.
        /// </summary>
        public bool Connected
        {
            get
            {
                return Client != null && Client.Connected;
            }
        }
        /// <summary>
        /// The server ip address.
        /// </summary>
        public string RemoteIP { get; private set; }
        /// <summary>
        /// The connect port.
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// The max retry count when connect failure.
        /// </summary>
        public int MaxRetryCount { get; private set; }
        /// <summary>
        /// The connect count to have been tried.
        /// </summary>
        public int RetriedCount { get; private set; }
        /// <summary>
        /// The retry interval (milliseconds) when connect failure. Default is as 1 seconds.
        /// </summary>
        public int RetryInterval { get; private set; } = 1000; //milliseconds
        /// <summary>
        /// Whether has been enabled retry when connect failure. Default is as false.
        /// </summary>
        public bool EnabledRetry { get; private set; } = false;
        /// <summary>
        /// The last error exception when error has been occurred.
        /// </summary>
        public Exception LastErrorException { get; private set; }

        /// <summary>
        /// Constructs the <see cref="TCPClientHelper"/>.
        /// </summary>
        /// <param name="ip">The server ip address.</param>
        /// <param name="port">The port to connect.</param>
        public TCPClientHelper(string ip, int port)
        {
            this.RemoteIP = ip;
            this.Port = port;
            this.LastErrorException = null;
            InitClient();
        }
        /// <summary>
        /// Enable the retry connection when connect failure.
        /// </summary>
        /// <param name="maxRetryCount">The max retry count.</param>
        /// <param name="retryInterval">The retry interval (milliseconds).</param>
        public void EnableRetry(int maxRetryCount = 5, int retryInterval = 1000)
        {
            this.MaxRetryCount = maxRetryCount;
            this.RetryInterval = retryInterval;
            this.EnabledRetry = true;
        }
        private void InitClient()
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LastErrorException = null;
            this.RetriedCount = 0;
            //Client.SendTimeout = 30;
        }
        /// <summary>
        /// Connect to server with a specified transaction id.
        /// </summary>
        /// <param name="transId">The specified transaction id to connect.</param>
        public void Connect(string transId = null)
        {
            if (!this.Connected)
            {
                StateObject stateObject = null;
                try
                {
                    InitClient();
                    stateObject = new StateObject()
                    {
                        TransactionId = transId,
                        Client = Client,
                    };
                    IPAddress ipAddr = IPAddress.Parse(RemoteIP);
                    this.Client.BeginConnect(ipAddr, this.Port, new AsyncCallback(ConnectCallBack), stateObject);
                }
                catch (Exception ex)
                {
                    if (!HandleErrorOccurred(stateObject, SocketActionState.Connect, ex))
                    {
                        throw;
                    }
                }
            }
            IsClosed = false;
        }
        /// <summary>
        /// Try to directly connect to server for connection testing.
        /// </summary>
        /// <param name="ip">The server ip.</param>
        /// <param name="port">The connect port.</param>
        /// <param name="connectionTimeout">The connection timeout.</param>
        /// <param name="error">The output error if connect failure.</param>
        /// <returns>Whether connect successfully.</returns>
        public static bool TryConnectTest(string ip, int port, int connectionTimeout, out string error)
        {
            Socket client = null;
            error = null;
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ip, port);
                if (client.Connected)
                {
                    return client.Connected;
                }
                var res = client.Poll(connectionTimeout * 1000, SelectMode.SelectWrite);
                return res;
                //var result = client.BeginConnect(ip, port, null, null);
                //result.AsyncWaitHandle.WaitOne(connectionTimeout * 1000);
                //return client.Connected;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            finally
            {
                try
                {
                    client?.Close();
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
        }
        private void ConnectCallBack(IAsyncResult ar)
        {
            StateObject stateObj = null;
            Socket client = null;
            try
            {
                stateObj = (StateObject)ar.AsyncState;
                client = stateObj.Client;
                client.EndConnect(ar);
                RaiseServerConnected(client);
                this.RetriedCount = 0;
            }
            catch (Exception ex)
            {
                if (!EnabledRetry)
                {
                    if (!HandleErrorOccurred(stateObj, SocketActionState.Connect, ex))
                    {
                        throw;
                    }
                }
                else
                {
                    if (this.RetriedCount < this.MaxRetryCount)
                    {
                        this.RetriedCount++;
                        Thread.Sleep(this.RetryInterval);
                        Connect(stateObj?.TransactionId);
                    }
                    else
                    {
                        if (!HandleErrorOccurred(stateObj, SocketActionState.Connect, ex))
                        {
                            throw;
                        }
                    }
                }
                return;
            }
            var stateObjRec = new StateObject()
            {
                Client = Client,
                TransactionId = stateObj.TransactionId
            };
            try
            {
                Client.BeginReceive(stateObjRec.ReceiveBuffer, 0, stateObjRec.ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, stateObjRec);
            }
            catch (Exception ex)
            {
                if (!HandleErrorOccurred(stateObjRec, SocketActionState.Connect, ex))
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Close current tcp socket client.
        /// </summary>
        public void Close()
        {
            IsClosed = true;
            try
            {
                if (this.Connected)
                {
                    Client.Close();
                    RaiseServerDisconnected(this.RemoteIP, this.Port, null, SocketActionState.Close);
                }
            }
            catch
            {
            }
        }
        private void CloseByServerDisconnected(StateObject stateObject, SocketActionState socketActionState)
        {
            IsClosed = true;
            try
            {
                if (this.Connected)
                {
                    Client.Close();
                    RaiseServerDisconnected(this.RemoteIP, this.Port, stateObject, socketActionState);
                }
            }
            catch
            {
            }
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            if (IsClosed) return;

            StateObject stateObj = null;
            int count = -1;
            try
            {
                stateObj = (StateObject)ar.AsyncState;
                if (IsClosed)
                {
                    return;
                }
                count = stateObj.Client.EndReceive(ar);
            }
            catch (Exception ex)
            {
                if (IsClosed)
                {
                    return;
                }
                if (!HandleErrorOccurred(stateObj, SocketActionState.Receive, ex))
                {
                    throw;
                }
                return;
            }
            if (count > 0)
            {
                RaiseDataReceived(Client, stateObj.ReceiveBuffer, count);
                if (this.Connected)
                {
                    try
                    {
                        Client.BeginReceive(stateObj.ReceiveBuffer, 0, stateObj.ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, stateObj);
                    }
                    catch (Exception ex)
                    {
                        if (!HandleErrorOccurred(stateObj, SocketActionState.Receive, ex))
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {
                CloseByServerDisconnected(stateObj, SocketActionState.Receive);
            }
        }
        /// <summary>
        /// Send specified message.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        /// <param name="sendTransactionId">The transaction id to send.</param>
        public void Send(string msg, string sendTransactionId = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            Send(data, sendTransactionId);
        }
        /// <summary>
        /// Send specified message.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        /// <param name="encoding">The encoding for message to send.</param>
        /// <param name="sendTransactionId">The transaction id to send.</param>
        public void Send(string msg, Encoding encoding, string sendTransactionId = null)
        {
            byte[] data = encoding.GetBytes(msg);
            Send(data, sendTransactionId);
        }
        /// <summary>
        /// Send specified data buffer to server.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="sendTransactionId">The transaction id to send.</param>
        public void Send(byte[] data, string sendTransactionId = null)
        {
            StateObject stateObj = new StateObject()
            {
                Client = Client,
                TransactionId = sendTransactionId,
            };
            try
            {
                if (!Client.Connected)
                {
                    // When current socket is closed, if has retried to connect immeadiately, the Connected state maybe still is false, so need to sleep the thread 
                    // to wait the connection state.
                    int tryCount = 0;
                    while (tryCount < 3 && !IsClosed)
                    {
                        tryCount++;
                        System.Threading.Thread.Sleep(1000);
                        if (Client.Connected)
                        {
                            break;
                        }
                    }
                }
                Client.BeginSend(data, 0, data.Length, SocketFlags.None, out SocketError errorCode, new AsyncCallback(SendCallBack), stateObj);
            }
            catch (Exception ex)
            {
                if (!HandleErrorOccurred(stateObj, SocketActionState.Send, ex))
                {
                    throw;
                }
            }
        }
        private void SendCallBack(IAsyncResult ar)
        {
            if (IsClosed) return;
            StateObject stateObject = (StateObject)ar.AsyncState;
            try
            {
                var client = stateObject.Client;
                client.EndSend(ar);
                RaiseDataSent(client, stateObject.TransactionId);
            }
            catch (Exception ex)
            {
                if (!HandleErrorOccurred(stateObject, SocketActionState.Send, ex))
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Dispose the object to release resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose to release resources.
        /// </summary>
        /// <param name="disposing">Whether is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        this.Close();
                        if (this.Client != null)
                        {
                            this.Client = null;
                        }
                    }
                    catch
                    {
                    }
                }
                this.disposed = true;
            }
        }

        private void RaiseServerDisconnected(string remoteIPAddr, int port, StateObject stateObject, SocketActionState socketActionState)
        {
            if (this.ServerDisconnected != null)
            {
                this.ServerDisconnected(remoteIPAddr, port, stateObject, socketActionState);
            }
        }
        private void RaiseServerConnected(Socket tcpClient)
        {
            if (this.ServerConnected != null)
            {
                this.ServerConnected(tcpClient);
            }
        }
        private void RaiseDataReceived(Socket tcpClient, byte[] buffer, int receivedLength)
        {
            if (this.DataReceived != null)
            {
                this.DataReceived(tcpClient, buffer, receivedLength);
            }
        }
        private void RaiseDataSent(Socket tcpClient, string sendTransactionId)
        {
            if (this.DataSent != null)
            {
                this.DataSent(tcpClient, sendTransactionId);
            }
        }

        // if handled, return true.
        private bool HandleErrorOccurred(StateObject stateObject, SocketActionState state, Exception ex)
        {
            this.LastErrorException = ex;
            if (ErrorOccurred != null)
            {
                ErrorOccurred(stateObject, state, ex);
                return true;
            }
            return false;
        }
    }
}
