using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using CustomNetworking;
using CC;
using System.Threading;

namespace commSockServer
{
    public class commSockReceiver
    {
        private TcpListener server;
        private int port;
        private ChatClientModel connection;
        private Timer heartbeatTimer;
        private volatile bool recentData;

        public event Action<bool> newConnection;
        public event Action<String> IncomingLine;
        public event Action connectionLost;

        public commSockReceiver(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            heartbeatTimer = new Timer(heartbeatTimerCallback);
        }

        private void heartbeatTimerCallback(object state)
        {
            if (!recentData)
            {
                try
                {
                    connection.SendMessage("µheartBeatµ");
                }
                catch
                {
                    //do nothing here, connection is either not connected or having issues...
                }
            }
            recentData = false;
        }

        public void beginAccept()
        {
            server.Start();
            server.BeginAcceptSocket(ConnectionRequested, null);
        }

        private void ConnectionRequested(IAsyncResult ar)
        {
            connection = new ChatClientModel(server.EndAcceptSocket(ar));
            connection.IncomingLineEvent += connection_IncomingLineEvent;
            if (newConnection != null)
            {
                newConnection(true);
            }
            server.BeginAcceptSocket(ConnectionRequested, null);
            heartbeatTimer.Change(1000, 1000);
        }

        private void connection_IncomingLineEvent(string obj)
        {
            if (obj == null) {
                if (connectionLost != null) {
                    connectionLost();
                }
            }
            if (IncomingLine != null) {
                IncomingLine(obj);
            }
        }

        public void write(String data)
        {
            recentData = true;
            if (connection != null) {
                connection.SendMessage(data);
            }
        }

    }
}
