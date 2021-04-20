using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketUtil
{
    public class Server
    {
        private Socket server, temp;
        private readonly int port;
        private readonly string host;

        public Server(string host, int port)
        {
            this.port = port;
            this.host = host;
            Console.WriteLine("等待客户端连接");
            InitialServer();
        }

        public string ReceiveMessage()
        {
            byte[] recvBytes = new byte[1024];
            int bytes = temp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
            return Encoding.UTF8.GetString(recvBytes, 0, bytes);
        }

        public void SendMessage(string sendStr)
        {
            byte[] bs = Encoding.UTF8.GetBytes(sendStr);
            temp.Send(bs, bs.Length, 0);//返回信息给客户端
        }

        private void InitialServer()
        {
            ///创建终结点（EndPoint）
            IPAddress ip = IPAddress.Parse(host);//把ip地址字符串转换为IPAddress类型的实例
            IPEndPoint ipe = new IPEndPoint(ip, port);//用指定的端口和ip初始化IPEndPoint类的新实例

            ///创建socket并开始监听
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个socket对像，如果用udp协议，则要用SocketType.Dgram类型的套接字
            server.Bind(ipe);//绑定EndPoint对像（2000端口和ip地址）
            server.Listen(0);//开始监听
            temp = server.Accept();//为新建连接创建新的socket
            Console.WriteLine("建立连接");
        }

        public void Close()
        {
            temp.Close();
            server.Close();
        }

        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 12345);
            String str = server.ReceiveMessage();
            server.SendMessage("这里是服务器");
            server.Close();
            Console.WriteLine(str);
            Console.ReadLine();
        }

    }
}