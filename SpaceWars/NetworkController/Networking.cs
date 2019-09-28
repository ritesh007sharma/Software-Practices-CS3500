using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static NetworkController.SocketState;

namespace NetworkController
{
    /// <summary>
    /// this class establishes the connections between the server and our client to send and receive data on the same addres line.
    /// @Author:Ajay Bagali, Ritesh Sharma,
    /// 12/8/18 
    /// </summary>
    public static class Networking
    {
        //default port to connect to the server
        public const int DEFAULT_PORT = 11000;

        //this delegate is called when we expect an exception whereour client cannot connect to the provided server
        public delegate void ExceptionEventHandler();
        //this is the event that handles the network exception when our client cannot connect to the provided server
        private static event ExceptionEventHandler networkException;

        /// <summary>
        /// this method takes in our even handler overloads our exception
        /// </summary>
        /// <param name="e"></param>
        public static void registerNetwork(ExceptionEventHandler e)
        {
            //this overload is dealing with the null socket exception when connecting to a wrong server 
            networkException += e;
        }

        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {

            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    //this gets the host name from the hanshaje
                    ipHostInfo = Dns.GetHostEntry(hostName);

                    //this follows the ip protocol
                    bool foundIPV4 = false;
                    //for each host that we connected to in our server address, we make sure they follow the protocol
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);

                        throw new ArgumentException("Invalid address");

                    }
                }
                catch (Exception)
                {
                    //we parse the host name when encountering an exception
                    ipAddress = IPAddress.Parse(hostName);

                }


                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;


            }

            catch (Exception e)
            {
                //we catch a network expcetion, when we encounter a null socket when nothing was retreived by our socket
                networkException();


            }

        }



        /// <summary>
        /// Start attempting to connect to the server
        /// Move this function to a standalone networking library.
        /// </summary>
        /// <param name="hostName"> server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(Action<SocketState> _call, string hostName)
        {
            //display which server we are trying to connect to
            System.Diagnostics.Debug.WriteLine("connecting to " + hostName);

            Socket socket;
            IPAddress ipAddress;

            //we make a socket that connects to the given server address 
            Networking.MakeSocket(hostName, out socket, out ipAddress);


            SocketState ss = new SocketState();
            //we use that call me reference the passed in socket state
            ss.callMe = _call;
            ss.theSocket = socket;
            //this establishes our connection and handles any exception returned by the server
            IAsyncResult sync
             = socket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedCallback, ss);

            //we wait for our client to wait on the exception and handle it afterwards for the user to understand 
            sync.AsyncWaitHandle.WaitOne(5000, true);


            return socket;





        }

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges the connect request
        /// Move this function to a standalone networking library.
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult stateAsArObject)
        {
            SocketState ss = (SocketState)stateAsArObject.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(stateAsArObject);
                ss.callMe(ss);
            }
            catch (Exception e)
            {
                networkException();
                //System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

        }



        /// <summary>
        /// this method gets the data from the socket state 
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            //this being receive establishes the handshake 
            try
            {

                state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// This function is "called" by the operating system when data arrives on the socket
        /// Move this function to a standalone networking library. 
        /// </summary>
        /// <param name="stateAsArObject"></param>
        private static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            SocketState ss = (SocketState)stateAsArObject.AsyncState;
            int bytesRead = 0;
            //this gets the number of bytes passed in by the socket 
            try
            {
                bytesRead = ss.theSocket.EndReceive(stateAsArObject);
            }
            catch (Exception e)
            {
                ss.theSocket.Shutdown(SocketShutdown.Both);
            }

            // If the socket is still open
            if (bytesRead > 0)
            {
                string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                // Append the received data to the growable buffer.
                // It may be an incomplete message, so we need to start building it up piece by piece
                ss.sb.Append(theMessage);
                //we call on our call me to finish the handshake to start using that data retreived 
                ss.callMe(ss);
            }

            // Continue the "event loop" that was started on line 100.
            // Start listening for more parts of a message, or more new messages
            //  ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);

        }

        /// <summary>
        /// this method sends the data to our server 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static bool Send(Socket socket, string data)
        {
            //we set up a byte array 
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            try
            {
                if (messageBytes.Length != 0)
                {
                    //while its active, we send in whatever data to the server

                    socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, socket);
                }
                return true;

            }
            catch (Exception e)
            {
                return false;

            }


        }

        /// <summary>
        /// A callback invoked when a send operation completes
        /// Move this function to a standalone networking library. 
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            s.EndSend(ar);
        }

        /// <summary>
        /// this method sets up the call me delegate for each client
        /// </summary>
        /// <param name="callMe"></param>
        public static void ServerAwaitingClientLoop(Action<SocketState> callMe)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            listener.Start();
            ConnectionState connecState = new ConnectionState();
            //setting up its listener
            connecState.listener = listener;
            //setting up its event delegate
            connecState.callMe = callMe;
            //receiving data
            listener.BeginAcceptSocket(AcceptNewClient, connecState);
        }
        /// <summary>
        /// Adds a new client connection to our server
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            ConnectionState connecState = (ConnectionState)ar.AsyncState;
            Socket socket = connecState.listener.EndAcceptSocket(ar);
            SocketState ss = new SocketState();
            //setting up its socket
            ss.theSocket = socket;
            //setting up its event delegate
            ss.callMe = connecState.callMe;
            ss.callMe(ss);
            //starting accepting data from the client
            connecState.listener.BeginAcceptSocket(AcceptNewClient, connecState);
        }

    }


}
