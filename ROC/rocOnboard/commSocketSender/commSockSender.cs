using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using CustomNetworking;
using System.Threading;

namespace commSockClient
{
    /// <summary>
    /// commSocks manage the client (ROC) side of a connection. They use and can connect to stringsockets but have events that can detect
    /// changes in connection and new data (strings) arriving and fire events for both. Strings can be sent/received by commSocks.
    /// </summary>
    public class commSockSender
    {
        public delegate void ConnectionChangedEventHandler(bool commSockIsConnected);

        /// <summary>
        /// fires when the connection is acquired OR lost. The boolean is true if the commSock is now connected and false if
        /// it is now disconnected.
        /// </summary>
        public event ConnectionChangedEventHandler connectionStatusChanged;

        /// <summary>
        /// fires when a new string of data has been received.
        /// </summary>
        public event Action<String> incomingLineEvent;

        /// <summary>
        /// tells whether or not the commSock is connected.
        /// </summary>
        public bool isConnected { get { return _isConnected; } 
                                    private set { _isConnected = value;
                                        if (connectionStatusChanged != null) //Fires the connectionStatusChanged event when changed.
                                            connectionStatusChanged(isConnected);
                                    }
                                }
        private volatile bool _isConnected = false; //ensure thread safety with volatile
        public string Name { get; private set; } //Name ought not to be changeable...

        private TcpClient tcpClient;
        private IPAddress destIP;
        private int destPort;
        private StringSocket SS;
        private volatile bool connecting = false; //make sure only one async connect attempt is going on at once.
        private volatile bool connectionIntended = false; //True once begin connect has been called and true until the public disconnect is called
        private volatile bool noRecentData = true;

        private Timer heartbeatTimer;

        /// <summary>
        /// Assigns the commSock a name and sets variables to defaults. A connectionStatusChanged event will fire during 
        /// constrution indicating the socket is disconnected.
        /// </summary>
        /// <param name="_Name"></param>
        public commSockSender(string _Name) {
            Name = _Name;
            heartbeatTimer = new Timer(heartbeatTimerCallback);
        }

        private void heartbeatTimerCallback(object state)
        {
            if (noRecentData && connectionIntended) //the connection was lost
            {
                internalDisconnect(); //Since connectionIntended is true, it should try to reconnect
            }
            noRecentData = true;
        }

        /// <summary>
        /// Attempts to connect to a specified IP and destination port. The callback will be called upon the conenction completing as well as the
        /// connectionStatusChanged event being fired. The boolean returns true if a connection was attempted (which happens if the commSock is NOT already
        /// conencted) and returns false if the commSock is connected and no new connection attempt was made. If it returns false you can call disconnect()
        /// then call this method.
        /// </summary>
        /// <param name="_destIP"></param>
        /// <param name="_destPort"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool beginConnect(IPAddress _destIP, int _destPort) {
            connectionIntended = true;
            if (!isConnected && !connecting) {
                destIP = _destIP;
                destPort = _destPort;
                if (tcpClient == null) {
                    tcpClient = new TcpClient();
                }
                connecting = true;
                
                tcpClient.BeginConnect(destIP, destPort, connectCallback, null);
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Attempts to disconnect the commSock IF it is not already disconnected. After this call you should be able to call beginConnect().
        /// </summary>
        /// <returns></returns>
        private bool internalDisconnect() {
            if (isConnected)
            {
                if (SS != null)
                {
                    SS.close();
                    SS = null;
                }
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
                isConnected = false;
                return true;
            }
            return false;
        }

        public void disconnect()
        {
            connectionIntended = false;
            internalDisconnect();
        }

        private void connectCallback(IAsyncResult ar) {
            try
            {
                tcpClient.EndConnect(ar);
                if (tcpClient.Connected)
                {
                    SS = new StringSocket(tcpClient.Client, UTF8Encoding.Default);
                    SS.BeginReceive(LineReceived, null);
                    isConnected = true;
                    connecting = false;

                    heartbeatTimer.Change(3000, 5000);
                    return;
                }
                else
                {
                    connecting = true;
                    tcpClient.BeginConnect(destIP, destPort, connectCallback, null);
                    return;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("ERROR: " + e.ErrorCode);
                tcpClient.BeginConnect(destIP, destPort, connectCallback, null);
            }
        }

        /// <summary>
        /// the message to be sent WITHOUT any newlines. Message sends are only attempted if the commSock is connected.
        /// </summary>
        /// <param name="line"></param>
        public void sendMessage(string line) {
            if (isConnected) {
                SS.BeginSend(line + "\n", (e, p) => { if (e != null) { /*throw e;*/} }, null); //TODO: Not sure what to do if the SS throws an exception. Maybe disconnect from it and start a reconnect routine.
            }
        }

        /// <summary>
        /// Deal with an arriving line of text.
        /// </summary>
        private void LineReceived(String s, Exception e, object p) {
            noRecentData = false; //data was received...
            if (s == null) { //The commSock lost its connection
                //DO NOT set isConnected to false here! It is done is disconnect()!!!!!!!!!!!
                internalDisconnect(); //must disconenct the socket to avoid trying to conenct on an "already connected" socket.
                s += "DISCONNECTION detected @ socket name: "+Name+"\n\n";
                if (connectionIntended)
                {
                    beginConnect(destIP, destPort); //attempt to reconnect if the commsock is supposed to be connected
                }
            }
            else if (incomingLineEvent != null) { //TODO: This used to jsut be an if... change back if it causes problems...
                incomingLineEvent(s);
            }
            if (!isConnected) { //if the socket lost its connection (which can happen is s == null) return and stopp calling more BeginReceives
                return;
            }
            SS.BeginReceive(LineReceived, null);
        } 
    }
}
