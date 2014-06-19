using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using CustomNetworking;

namespace rocNetworkManager
{
    public class rocNetworkManager
    {

        private static rocNetworkManager Instance;
        StringSocket PrimaryProgSocket;
        private TcpClient client;
        private IPAddress MCIP;
        private int MCPort;

        

        /// <summary>
        /// Returns whether or not the rocNetworkManager is conencted to the primary program in Mission Control
        /// </summary>
        public bool isConnectedToPrimProg { get { return _isConnectedToPrimProg; } private set { _isConnectedToPrimProg = value; } }
        private static volatile bool _isConnectedToPrimProg = false; //ensure thread safety

        private rocNetworkManager(IPAddress IP,int port) {
            isConnectedToPrimProg = false;
            MCIP = IP;
            MCPort = port;
            client = new TcpClient();

            client.BeginConnect(MCIP, MCPort, MCConnect, null); //start trying to connect to mission control PP
            while (isConnectedToPrimProg == false) {
                //TODO: Currently we do nothing here, jsut waiting for it to connect. Maybe do other work?
            }
        }

        /// <summary>
        /// Callback for trying to connect to Mission Control's PP. Will continually try to connect to reconnect until successful.
        /// </summary>
        /// <param name="ar"></param>
        private void MCConnect(IAsyncResult ar) {
            if (client.Connected) {
                PrimaryProgSocket = new StringSocket(client.Client, UTF8Encoding.Default);
                isConnectedToPrimProg = true;
            }
            else {
                isConnectedToPrimProg = false;
                client.BeginConnect(MCIP, MCPort, MCConnect, null);
            }
        }

        /// <summary>
        /// Returns an instance (THE instance actually) of the rocNetworkManager. Provide the public IP of the router in Mission
        /// Control as well as the port that is forwarded to the primary program.
        /// </summary>
        /// <param name="MCPrimaryProgIP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static rocNetworkManager getInstance(IPAddress MCPrimaryProgIP,int MCPrimaryProgPort){
            if (Instance == null) {
                Instance = new rocNetworkManager(MCPrimaryProgIP, MCPrimaryProgPort);
            }
            return Instance;
        }
    }
}
