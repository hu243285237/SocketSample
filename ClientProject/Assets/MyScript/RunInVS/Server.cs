using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;
using System.Text;

namespace ServerTest
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class Server
    {
        private static byte[] result = new byte[1024];
        private const int port = 8088;//端口号
        private static string IpString = "127.0.0.1";//127.0.0.1指本地机地址
        private static Socket serverSocket;//服务器socket

        //---------------------------------------------------------------------

        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(IpString);//将字符串转换为IPAddress类型
            IPEndPoint ip_end_point = new IPEndPoint(ip, port);//IPEndPoint表示主机地址和端口信息

            //创建服务器Socket对象，并设置属性为，internet，stream传输，按照tcp协议
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Protocol=协议

            //绑定ip和端口
            serverSocket.Bind(ip_end_point);
            //设置最长的链接请求队列长度
            serverSocket.Listen(10);
            Console.WriteLine("启动监听 {0} 成功", serverSocket.LocalEndPoint.ToString());

            //在新线程中监听客户端的链接
            Thread thread = new Thread(ClientConnectListen);
            thread.Start();
            Console.ReadLine();
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// 客户端连接请求监听
        /// </summary>
        private static void ClientConnectListen()
        {
            while (true)
            {
                //为新的客户端链接创建一个Socket对象
                Socket clientSocket = serverSocket.Accept();//Accept()，暂停当前线程，直到有客户端链接进来
                Console.WriteLine("客户端 {0} 成功链接", clientSocket.RemoteEndPoint.ToString());

                //向链接的客户端发送连接成功的数据方法一
                byte[] data = ASCIIEncoding.UTF8.GetBytes("成功链接服务器");//将要发送的string转化成byte[]
                clientSocket.Send(data);//将byte[]发送出去

                /*
                //向链接的客户端发送连接成功的数据方法二
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteString("Connected Server!");
                clientSocket.Send(WriteMessage(buffer.ToBytes()));
                */

                //每个客户端链接创建一个线程来接收该客户端发送的消息
                Thread thread = new Thread(RecieveMessage);
                thread.Start(clientSocket);
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// 接收指定客户端Socket的消息
        /// </summary>
        private static void RecieveMessage(object clientSocket)
        {
            Socket mClientSocket = (Socket)clientSocket;

            while (true)
            {
                try
                {
                    mClientSocket.Receive(result);
                    string data = ASCIIEncoding.UTF8.GetString(result);
                    Console.WriteLine("从客户端传来内容：{0}", data);

                    /*
                    int receiveNumber = mClientSocket.Receive(result);
                    Console.WriteLine("接收客户端{0}消息，长度为{1}", mClientSocket.RemoteEndPoint.ToString(), receiveNumber);
                    ByteBuffer buff = new ByteBuffer(result);

                    //数据长度
                    int length = buff.ReadShort();
                    //数据内容
                    string data = buff.ReadString();
                    Console.WriteLine("数据内容：{0}", data);
                    */
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);//将报错打印出来
                    mClientSocket.Shutdown(SocketShutdown.Both);
                    mClientSocket.Close();
                    break;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------

        /// <summary>
        /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据
        /// </summary>
        private static byte[] WriteMessage(byte[] message)
        {
            MemoryStream stream = null;
            //using语句，定义一个范围，在范围结束时处理对象
            using (stream = new MemoryStream())
            {
                stream.Position = 0;
                BinaryWriter writer = new BinaryWriter(stream);
                short streamLength = (short)message.Length;
                writer.Write(streamLength);
                writer.Write(message);
                writer.Flush();
                return stream.ToArray();
            }
        }

        //---------------------------------------------------------------------
    }
}