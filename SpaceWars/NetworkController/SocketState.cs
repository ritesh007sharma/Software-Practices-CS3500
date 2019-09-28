using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// @Author: Ajay Bagali, Ritesh Sharma
    /// 12/8/18
    /// </summary>
    public class SocketState
    {
        //socket to hold the socket state
        public Socket theSocket;
        //id of the socketstate
        public int ID;
        //this is where the network connects using the socketstate to pass around the elements of the socket state
        public delegate void NetworkAction(SocketState ss);

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        //the call me referes to the delegate to pass around the consistent socket state holding our world data 
        public Action<SocketState> callMe;
        //this returns the socket state ID associated with each socket state ID
        public void setSocketStateID(int id)
        {
            this.ID = id;
        }
    }


}
