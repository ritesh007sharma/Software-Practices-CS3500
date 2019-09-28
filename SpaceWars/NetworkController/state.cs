using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// This class holds the connection state to hold the call me delegate and the listener
/// @Author: Ajay Bagali, Ritesh Sharma
/// 12/8/18
/// </summary>
namespace NetworkController
{
    public class ConnectionState
    {
        public Action<SocketState> callMe;

        public TcpListener listener;

    }
}
