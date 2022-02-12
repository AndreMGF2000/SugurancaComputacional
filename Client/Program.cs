using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        
        public static void Main()
        {
            Thread listener = new Thread(new ThreadStart(Listener));
            listener.Start();

            CommunicationDataSender();

            while (true)
            {
                StartMessageSender();
            }

        }

        private static void CommunicationDataSender()
        {
            Console.WriteLine("Digite seu nome de usuário: ");
            String username = Console.ReadLine();
            Console.WriteLine("Digite seu sobrenome de usuário: ");
            String sobrenome = Console.ReadLine();
            //2. Chave Publica
            //3. IPAddress (no caso do teste em ambiente aqui vai ser sempre o mesmo)
            //4. Porta que ficará ouvindo a resposta (precisamos ouvir em portas difentes,
            //já que usaremos um pc apenas para simulação)
            List<string> dataCommunication = new List<string>();
            dataCommunication.Add(username);
            dataCommunication.Add(sobrenome);
            StringBuilder allData = new StringBuilder("|");
            foreach (string data in dataCommunication)
            {
                Console.WriteLine(data);
                allData.Append(data + "|");              
            }
            StartMessageSender(allData.ToString());
        }

        private static void StartMessageSender(String dataCommunication = "")
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress ServerAddress = IPAddress.Parse("10.0.0.103");

            Console.WriteLine("Write yout message to the server");
            IPEndPoint ep = new IPEndPoint(ServerAddress, 11000);
            String message = Console.ReadLine();
            if (!String.IsNullOrEmpty(dataCommunication))
            {
                byte[] sendbufDatas = Encoding.ASCII.GetBytes("DATA: " + dataCommunication);
                s.SendTo(sendbufDatas, ep);
            }
            
            byte[] sendbuf = Encoding.ASCII.GetBytes("Message: " + message);
            s.SendTo(sendbuf, ep);
            Console.WriteLine("Message sent to the server address");
        }

        private const int listenPort = 11100;
        public static void Listener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    //Console.WriteLine("Waiting for the response of the server...");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received message from the server {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
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
    }
}
