using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketUtil
{
    public class Client
    {
        private Socket client;
        private readonly int port;
        private readonly string host;

        public Client(string host, int port)
        {
            this.port = port;
            this.host = host;
        }

        public string Get(String sendStr)
        {
            string recvStr = "";
            try
            {
                InitialSocket();
                SendMessage(sendStr);
                recvStr = ReceiveMessage();
                ///一定记着用完socket后要关闭
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("argumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
            return recvStr;
        }

        private void InitialSocket()
        {
            ///创建终结点EndPoint
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例

            ///创建socket并连接到服务器
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            Console.WriteLine("Conneting…");
            client.Connect(ipe);//连接到服务器
        }

        private void SendMessage(string sendStr)
        {
            ///向服务器发送信息
            byte[] bs = Encoding.UTF8.GetBytes(sendStr);//把字符串编码为字节
            client.Send(bs, bs.Length, 0);//发送信息
        }

        private string ReceiveMessage()
        {
            ///接受从服务器返回的信息
            byte[] recvBytes = new byte[1024];
            int bytes = client.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
            string recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);
            return recvStr;
        }

        //static void Main(string[] args)
        //{
        //    Client client = new Client("127.0.0.1", 12345);
        //    String a = client.Get("你好");
        //    Console.WriteLine(a);
        //    Console.ReadLine();
        //}
    }
}
