using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using WindowsFormsApplication6;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Data;

namespace RBC
{
    public class TcpIpCommunicationUnit
    {

        Boolean shutDown = false;

        //eventhandler
        public delegate void StatusChangedEventHandler(String statusMessage);
        public event StatusChangedEventHandler statusChangedEvent;

        public delegate void ErrorEventHandler(String errorMessage);
        public event ErrorEventHandler errorEvent;

        public delegate void MessageReceivedEventHandler(String[] receivedMessage);
        public event MessageReceivedEventHandler messageReceivedEvent;

        public delegate void MessageTunnelingHandler(String receivedMessage);
        public event MessageTunnelingHandler tunnelingMessage;

        public delegate void UpdateTextCallback(string text);
        public delegate void UpdateChartCallback(string[] msg1, string msg3);

        String communicationName = "Unknown";

        System.Threading.Thread clientListenerThread, clientRemoverThread;
        System.Threading.Thread clientRobotControllerThread, clientPhoneThread;

        System.Net.Sockets.TcpListener tcpserver;


        public String configfilename = "";
        public NetworkConfig networkConfig = null;
        public bool newDataAvailable = false;

        Dictionary<string, TcpClient> clientList = new Dictionary<string, TcpClient>();
        private bool clientReceiveThread_stop = false;
        private bool clientRobotControllerThreadStarted = false;
        private bool clientPhoneThreadStarted = false;

        private bool shutDown_clientListener = false;
        private bool shutDown_clientRemover = false;
        private int phoneClientCounter;
        private IPAddress[] ipAddress;
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream networkStream;
        private GlobalDataSet globalDataSet;

        public TcpIpCommunicationUnit(String pCommunicationName, GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
            this.communicationName = pCommunicationName;
            this.configfilename = pCommunicationName + "_NetworkConfig.xml";
            this.loadNetworkConfiguration();
            this.tunnelingMessage += new MessageTunnelingHandler(tcpDiagnoseServer_tunnelingMessage);
        }

        public bool clientInit()
        {
            //String ip = "192.168.1.4";
            String ip = "127.0.0.1";
            int port = 4555;
            ipAddress = Dns.GetHostAddresses(ip);

            try
            {
                if (globalDataSet.DebugMode) Debug.Write("Start client" + "\n");
                tcpClient.Connect(ip, port);

                networkStream = tcpClient.GetStream();


                if (tcpClient.Connected)
                {
                    if (globalDataSet.ShowProgramDuration)
                    {
                        globalDataSet.Timer_programExecution.Start();
                        globalDataSet.TimerValue = globalDataSet.Timer_programExecution.ElapsedMilliseconds;
                    }

                    if (globalDataSet.DebugMode) Debug.Write("Connected" + "\n");
                    startReceiverThread();
                    // Start server
                    // this.tcpserver = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Parse(this.networkConfig.ipAddress), this.networkConfig.port);
                    // this.tcpserver.Start();

                    // Start thread to listening for new clients
                    // this.clientListenerThread = new System.Threading.Thread(new System.Threading.ThreadStart(clientListener));
                    // this.clientListenerThread.Start();

                    // Start thread to listening on removed clients
                    //this.clientRemoverThread = new System.Threading.Thread(new System.Threading.ThreadStart(clientRemover));
                    // this.clientRemoverThread.Start();

                    //if (this.statusChangedEvent != null)
                    //    this.statusChangedEvent(this.communicationName + ": Server started. Listening for Clients at Port - " + this.networkConfig.port);
                    return true;
                }
                else
                {
                    if (globalDataSet.DebugMode) Debug.Write("Client not connected" + "\n");
                    return false;
                }

            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (globalDataSet.DebugMode) Debug.Write("Error in clientServerInit: " + ex);
                return false;
            }
        }

        // public void clientListener()
        public void startReceiverThread()
        {
            // Create threads for robot controller and phones
            this.clientRobotControllerThread = new System.Threading.Thread(new System.Threading.ThreadStart(receiveFromRobotController));
            ////this.clientPhoneThread = new System.Threading.Thread(new System.Threading.ThreadStart(sendToPhone));
            //phoneClientCounter = 0;
            //while (this.shutDown_clientListener == false)
            //{
            //    if(globalDataSet.DebugMode) Debug.Write("Listening for new clients" + "\n");
            //    // Listening for new client               
            //    TcpClient tcpClientTemp = tcpserver.AcceptTcpClient();

            //    // Receive name of client and add client to dictonary
            //    // If name already exist create another one
            //    string clientName = receiveClientName(tcpClientTemp);
            //    if(globalDataSet.DebugMode) Debug.Write("clientName: " + clientName + "\n");

            //    if (clientName.Equals("Phone"))
            //    {
            //        // Send answer to client that clientname is received
            //        if(globalDataSet.DebugMode) Debug.Write("Send handshake to phone" + "\n");
            //        sendToClient(":p:1;", tcpClientTemp);

            //        // Modify client name from "phone" to "phoneX" i.e. "phone1", "phone2", etc.
            //        clientName = clientName + phoneClientCounter.ToString();
            //        if(globalDataSet.DebugMode) Debug.Write("clientName modified: " + clientName + "\n");
            //        clientList.Add(clientName, tcpClientTemp);
            //        phoneClientCounter += 1;
            //    }
            //    if (clientName.Equals("RobotController"))
            //    {
            //        // Send answer to client that clientname is received
            //        if(globalDataSet.DebugMode) Debug.Write("Send handshake to robot controller" + "\n");
            //        sendToClient(":p:1;", tcpClientTemp);

            //        // Add name to dictonary
            //        clientList.Add(clientName, tcpClientTemp);
            //    }

            // Start new thread for robot controller
            //if (clientList.ContainsKey("RobotController") && !(this.clientRobotControllerThread.ThreadState == System.Threading.ThreadState.Running))
            if (!(this.clientRobotControllerThread.ThreadState == System.Threading.ThreadState.Running))
            {
                this.clientRobotControllerThread.Start();
                if (globalDataSet.DebugMode) Debug.Write("clientRobotControllerThread started" + "\n");
            }

            // Start new thread for phones
            //if (clientList.ContainsKey("Phone") && !(this.clientPhoneThread.ThreadState == System.Threading.ThreadState.Running))
            //{
            //if(globalDataSet.DebugMode) Debug.Write("clientPhoneThread started" + "\n");
            //this.clientPhoneThread.Start();
            // }
            //}

            if (this.statusChangedEvent != null)
                this.statusChangedEvent(this.communicationName + ": New client connected. Start receiving");
        }

        public void clientRemover()
        {
            TcpClient tcpClient;
            while (this.shutDown_clientRemover == false)
            {
                if (clientList.ContainsKey("RobotController"))
                {
                    clientList.TryGetValue("RobotController", out tcpClient);
                    // When thread is running and has no connection then abort it
                    if (!SocketIsConnected(tcpClient.Client)) if (clientRobotControllerThread != null) if (clientRobotControllerThread.ThreadState == System.Threading.ThreadState.Running)
                            {
                                clientList.Remove("RobotController");
                                clientRobotControllerThread.Abort();
                            }
                }
                if (clientList.ContainsKey("Phone"))
                {
                    clientList.TryGetValue("Phone", out tcpClient);
                    // When thread is running and has no connection then abort it
                    if (!SocketIsConnected(tcpClient.Client)) if (clientPhoneThread != null) if (clientPhoneThread.ThreadState == System.Threading.ThreadState.Running)
                            {
                                clientList.Remove("Phone");
                                clientPhoneThread.Abort();
                            }
                }
            }
        }

        public string receiveClientName(TcpClient tcpClient)
        {
            String incomingMessage = "";
            String clientName = "";
            bool isRunning = true;

            if (globalDataSet.DebugMode) Debug.Write("Receive name of client" + "\n");
            while (isRunning)
            {
                try
                {
                    System.Net.Sockets.NetworkStream ns = tcpClient.GetStream();
                    byte[] buffer = new byte[8192];

                    int data = ns.Read(buffer, 0, 1);
                    incomingMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, data);
                    if (incomingMessage.Length != 0)
                    {
                        if (!incomingMessage.Equals(";"))
                        {
                            if (globalDataSet.DebugMode) Debug.Write("Name: " + clientName + "\n");
                            clientName += incomingMessage;
                        }
                        else
                        {
                            isRunning = false;
                            return clientName;
                        }
                    }
                }
                catch (System.IO.IOException ex)
                {
                    isRunning = false;
                    if (globalDataSet.DebugMode) Debug.Write("Error in receiveClientName: " + ex);
                    return "";
                }
            }
            return "";
        }

        public void sendToPhone(string msg)
        {
            TcpClient tcpClientPhone;

            // TODO: Maybe do this in another thread
            if (globalDataSet.DebugMode) Debug.Write("Message for phone: " + msg);
            for (int i = 0; i <= phoneClientCounter - 1; i++)
            {
                if (globalDataSet.DebugMode) Debug.Write("Send to client: " + i.ToString() + "\n");
                clientList.TryGetValue("Phone" + i.ToString(), out tcpClientPhone);
                sendToClient(msg, tcpClientPhone);
            }
            //while (this.shutDown == false)
            //{
            //    // Send data when new data is available
            //    if (newDataAvailable)
            //    {

            //        newDataAvailable = false;
            //    }
            //}
        }

        public void receiveFromRobotController()
        {
            String incomingMessage = "";
            String incomingMessageTemp = "";
            String[] msgArray = new String[] { "", "" };
            bool commandMessage = false;
            bool normalMessage = false;
            bool itsACommand = false;
            TcpClient tcpClientRobot;

            //clientList.TryGetValue("RobotController", out tcpClientRobot);            
            System.Net.Sockets.NetworkStream ns = networkStream; //tcpClientRobot.GetStream();
            byte[] buffer = new byte[8192];


            while (this.shutDown == false)
            {
                try
                {
                    //if(globalDataSet.DebugMode) Debug.Write("Received from server: " + incomingMessage + "\n");
                    int data = ns.Read(buffer, 0, 1);
                    incomingMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, data);

                    if (incomingMessage.Length != 0)
                    {
                        incomingMessageTemp += incomingMessage;
                        if (!incomingMessage.Equals(":") && commandMessage)
                        {
                            itsACommand = true;
                            msgArray[1] += incomingMessage;
                        }

                        if (!incomingMessage.Equals(":") && !commandMessage) normalMessage = true;
                        // Check if msg should be read as command
                        if (incomingMessage.Equals(":") && !commandMessage)
                        {
                            commandMessage = true;
                        }
                        else if (incomingMessage.Equals(":") && commandMessage)
                        {
                            if (itsACommand) normalMessage = false;
                            if (!itsACommand) normalMessage = true;
                            commandMessage = false;
                        }

                        // Read message as normal characters
                        if (!incomingMessage.Equals(";") && !commandMessage && normalMessage)
                        {
                            msgArray[0] += incomingMessage;
                            itsACommand = false;
                        }
                        else if (incomingMessage.Equals(";") && !commandMessage && normalMessage)
                        {
                            normalMessage = false;
                            if (this.statusChangedEvent != null)
                                this.statusChangedEvent(this.communicationName + ": Message received from Client - " + incomingMessage);

                            // TODO: Validate the length of incoming message and the content to request resend

                            // Fire the receive event and save sensor values to database
                            if (this.messageReceivedEvent != null) this.messageReceivedEvent(msgArray);
                            //if (this.tunnelingMessage != null) this.tunnelingMessage(incomingMessageTemp);
                            // Test to use one thread for tunneling to client and to filtering...
                            //sendToPhone(incomingMessageTemp);
                            //if(globalDataSet.DebugMode) Debug.Write("Message received: " + incomingMessageTemp + "\n");
                            msgArray = new String[] { "", "" };
                            incomingMessageTemp = "";
                        }

                    }

                }
                catch (System.IO.IOException ex)
                {
                    // TODO Validate shutdown implementation
                    this.shutDown = true;
                }

            }
        }

        public void closeAllConnections()
        {
            shutDown_clientListener = true;
            shutDown_clientRemover = true;

            if (this.clientListenerThread != null) if (this.clientListenerThread.ThreadState == System.Threading.ThreadState.Running) this.clientListenerThread.Abort();
            if (this.clientRemoverThread != null) if (this.clientRemoverThread.ThreadState == System.Threading.ThreadState.Running) this.clientRemoverThread.Abort();
            if (this.clientRobotControllerThread != null) if (this.clientRobotControllerThread.ThreadState == System.Threading.ThreadState.Running) this.clientRobotControllerThread.Abort();
            if (this.clientPhoneThread != null) if (this.clientPhoneThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    this.clientPhoneThread.Abort();
                    this.tcpserver.Stop();
                }
        }

        private bool SocketIsConnected(Socket s)
        {

            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        public void saveNetworkConfiguration()
        {
            String savefilename = this.configfilename;

            System.IO.FileStream outFile = System.IO.File.Create(savefilename);
            System.Xml.Serialization.XmlSerializer formatter = new System.Xml.Serialization.XmlSerializer(typeof(NetworkConfig));
            formatter.Serialize(outFile, this.networkConfig);
            outFile.Close();
        }

        public void loadNetworkConfiguration()
        {
            String loadfilename = this.configfilename;

            //if (System.IO.File.Exists(loadfilename))
            //{
            //    System.Xml.Serialization.XmlSerializer formatter = new System.Xml.Serialization.XmlSerializer(typeof(NetworkConfig));
            //    System.IO.FileStream aFile = new System.IO.FileStream(loadfilename, System.IO.FileMode.Open);
            //    byte[] buffer = new byte[aFile.Length];
            //    aFile.Read(buffer, 0, (int)aFile.Length);
            //    System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
            //    this.networkConfig = ((NetworkConfig)formatter.Deserialize(stream));
            //    aFile.Close();
            //    stream.Close();
            //}
            //else
            //{
            //if there is no data available clean it up
            this.networkConfig = new NetworkConfig();
            //this.networkConfig.ipAddress = "192.168.1.3";
            this.networkConfig.ipAddress = "0.0.0.0";
            this.networkConfig.port = 4555;
            //save the stadardconfig
            this.saveNetworkConfiguration();

            //}
        }

        void tcpDiagnoseServer_tunnelingMessage(string receivedMessage)
        {
            newDataAvailable = true;
        }

        public void sendToClient(string msg, TcpClient tcpClient)
        {
            try
            {
                System.Net.Sockets.NetworkStream ns = tcpClient.GetStream();
                byte[] sendbuffer = System.Text.Encoding.ASCII.GetBytes(msg);
                ns.Write(sendbuffer, 0, sendbuffer.Length);
                //if(globalDataSet.DebugMode) Debug.Write("To phone: " + msg + "\n");
            }
            catch (Exception ex)
            {
                if (globalDataSet.DebugMode) Debug.Write("Error in sendToSmartphone: " + ex);
            }
        }
    }
}
