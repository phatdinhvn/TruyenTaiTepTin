using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6789);
                EndPoint ep = ip;

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                Socket clientNoti = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                client.Connect(ip);
                clientNoti.Bind(ep);
                Console.WriteLine("Ket noi den Server thanh cong!");
                
                /*
                string s = "Hello server!";
                byte[] gui = Encoding.ASCII.GetBytes(s);
                client.Send(gui);
                */
                byte[] nhan = new byte[255];
                Console.WriteLine("Server ton tai cac files:");
                while (true)
                {
                    clientNoti.ReceiveFrom(nhan,ref ep);
                    string r = Encoding.ASCII.GetString(nhan);
                    if (r == "end")
                        break;
                    
                } 
                

                byte[] clientData = new byte[1024 * 5000];
                Console.WriteLine("Nhap duong dan luu file: \nVi du: C:/ABC/XYZ/");
                string receivedPath = Console.ReadLine();

                int receivedBytesLen = client.Receive(clientData);

                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                Console.WriteLine("Client:Ket noi thanh cong.\nFile {0} bat dau nhan.", fileName);

                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append)); ;
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

                Console.WriteLine("{0} da duoc luu vao duong dan: {1}", fileName, receivedPath);

                bWrite.Close();
                client.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi gui file: |" + ex.Message +"|");
            }
        }
    }
}
