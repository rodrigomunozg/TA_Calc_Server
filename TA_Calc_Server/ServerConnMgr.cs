/*
 Developed by Rodrigo Muñoz for Rockstar Games interview process. May2021.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TA_Calc_Server
{
    /// <summary>
    /// Code adapted from the Microsoft Documentation of Synchronous Server Socket.
    /// This is the class that will manage the socket connection.
    /// Is the only class that has relation with Calc_Client solution.
    /// </summary>
    public class ServerConnMgr
    {

        // Incoming data from the client.  
        public static string data = null;
        private static ServerCalc sc;
        public static IPHostEntry host;
        public static IPAddress ipAddress;
        public static IPEndPoint localEndPoint;
        public static Socket listener;
        public static byte[] bytes;

        internal static ServerCalc Sc { get => sc; set => sc = value; }

        public static void StartListening()
        {
            bytes = new Byte[1024];
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000); 
            Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    Console.WriteLine('\n'+"Waiting for Client_Calc math operation..."+'\n');
                    Socket handler = listener.Accept();
                    data = null;
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);               //Mathematical operation receiver from the client
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    Console.WriteLine("Math operation received : {0}", data.Replace("<EOF>",""));
                    byte[] msg = Encoding.ASCII.GetBytes(""+Sc.evaluateMathOperator(data)); //Mathematical operation sent to the ServerCalc instance to solve it, the result is stored in a byte array
                    handler.Send(msg);                                                      //The result of the mathematical operation is sent to the connection manager from the client side.
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        /// <summary>
        /// Start point of the server side of the solution. When started it creates the instance of ServerCalc (the one that solves all the mathematical operations)
        /// </summary>
        /// <param name="args">main parameters</param>
        /// <returns>System exit status</returns>
        public static int Main(String[] args)
        {
            Sc = new ServerCalc();
            StartListening();
            return 0;
        }
    }
}