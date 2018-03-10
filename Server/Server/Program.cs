using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Server dang hoat dong.");
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 6789);
                EndPoint ep = ipEnd;

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                Socket sockNoti = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sock.Bind(ipEnd);
                sockNoti.Bind(ep);
                sock.Listen(100);
                //clientSock is the socket object of client, so we can use it now to transfer data to client
                Socket clientSock = sock.Accept();


                //string fileName = "test.txt";// "Your File Name";
                string filePath = @"..\\..\\Files\";//Your File Path;

                string[] folder = Directory.GetFiles(@"..\\..\\Files\");
                
                Console.WriteLine("Client da ket noi! "+folder.Length+" tep tin sau san sang tai: ");
                for (int i = 0;i<folder.Length;i++)
                {
                    string fName = Path.GetFileName(folder[i]);
                    Console.WriteLine(" - "+ fName);
                    byte[] gui = Encoding.ASCII.GetBytes(fName);
                    sockNoti.SendTo(gui,ep);
                    if (i == folder.Length)
                        break;
                    
                }

                byte[] nhan = new byte[255];
                sock.Receive(nhan);
                string r = Encoding.ASCII.GetString(nhan);
                Console.WriteLine("Client: {0}", r);
                string fileName = r;
                //-------------------------------------------------------
                byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);

                byte[] fileData = File.ReadAllBytes(filePath + fileName);
                byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                fileNameLen.CopyTo(clientData, 0);
                fileNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fileNameByte.Length);


                clientSock.Send(clientData);
                Console.WriteLine("File:{0} has been sent.", fileName);

                sockNoti.Close();
                sock.Close();
                clientSock.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File Receiving fail." + ex.Message);
            }
            /*
            byte[] nhan = new byte[1024];
            EndPoint ep = ip;
            server.ReceiveFrom(nhan, ref ep);
            string r = Encoding.ASCII.GetString(nhan);
            Console.WriteLine("Client: {0}", r);

            string s = "Hello Client!";
            byte[] gui = Encoding.ASCII.GetBytes(s);
            server.SendTo(gui, ep);
            

            //server.Close();
            */
        }
    }
}
