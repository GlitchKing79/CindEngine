using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.CodeDom.Compiler;
namespace CindEngine.Multiplayer
{
    class Networking
    {
        static List<string> connectedIp = new List<string>();
        static List<string> connectedMessages = new List<string>();
        static List<Thread> connectedThread = new List<Thread>();

        public bool broadcasting = false;
        Thread NetworkThread;

        public string ip;
        public int port;
        public void Init()
        {
            NetworkThread = new Thread(new ThreadStart(StartThread));
            NetworkThread.Start();
        }

        void StartThread()
        {
            StartServer(ip, port);
        }
        

        void StartServer(string ip,int port)
        {
            IPAddress ipAdress = IPAddress.Parse(ip);
            TcpListener tcpListner = new TcpListener(ipAdress, port);
            while (broadcasting)
            {
                TcpClient client = tcpListner.AcceptTcpClient();

                
                Thread clientThread = new Thread(() => { RecieveMessages(client); });
                clientThread.Start();
            }             
           
        }

        public bool GetMessage(string message)
        {
            bool gotMessage = connectedMessages.Contains(message);

            if (gotMessage)
            {
                connectedMessages.Remove(message);
            }

            return gotMessage;
        }

        void RecieveMessages(TcpClient client)
        {
            NetworkStream netStream = client.GetStream();
            while (true)
            {
                int i;
                byte[] bytes = new byte[256];
                while ((i = netStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    connectedMessages.Add(Encoding.ASCII.GetString(bytes,0,i));
                }
            }
        }

    }
}
