using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        private const int listenPort = 11000;

        private static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for the clients...");
                    byte[] bytes = listener.Receive(ref groupEP);
                    string messageReceive = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"Received message from Client{groupEP} :");
                    Console.WriteLine($" {messageReceive}");

                    //if (messageReceive.Contains("DATA"))
                    //{
                    //    Thread sender = new Thread(new ThreadStart(Sender));

                    //    sender.Start();
                    //}
                    if (messageReceive.Contains("DATA"))
                    {

                        Sender();
                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }    
        public static void Main()
        {
            StartListener();
        }
        private static void Sender()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress clientIPAddress = IPAddress.Parse("10.0.0.103");
            //Enviar isso tudo, criptografado com a chave publica
            byte[] sendbuf = Encoding.ASCII.GetBytes("DATA|Chave Simetrica: X|Endereco Multicast");
            IPEndPoint ep = new IPEndPoint(clientIPAddress, 11100);

            s.SendTo(sendbuf, ep);

            Console.WriteLine("Message sent to the client with the data to join multicast");
        }
    }
    
}
