using System.Collections;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;


/// <summary>
/// 客户端
/// </summary>
public class ClientSocket
{
    private static byte[] result = new byte[1024];//缓冲区的数据
    private static Socket clientSocket;//客户端Socket
    public bool isConnected = false;//是否已链接

    //---------------------------------------------------------------------------------------------

    /// <summary>
    /// 构造函数
    /// </summary>
    public ClientSocket()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    //---------------------------------------------------------------------------------------------

    /// <summary>
    /// 链接服务器，输入服务器的ip地址和端口号
    /// </summary>
    public void ConnectServer(string ip, int port)
    {
        IPAddress mIp = IPAddress.Parse(ip);//将string转换为IPAddress类型
        IPEndPoint ip_end_point = new IPEndPoint(mIp, port);//IPEndPoint表示主机地址和端口信息

        try
        {
            clientSocket.Connect(ip_end_point);//客户端链接服务器
            isConnected = true;
            ButtonFunction.textString += "接服务器成功\n";
        }
        catch
        {
            isConnected = false;
            ButtonFunction.textString += "链接服务器失败\n";
            return;
        }

        //读取服务器发的数据方法一
        clientSocket.Receive(result);//接收传过来的byte[]
        string data = ASCIIEncoding.UTF8.GetString(result);//将byte[]转化成string
        Array.Clear(result, 0, result.Length);
        ButtonFunction.textString += "服务器返回数据：" + data + "\n";
        
        /*
        //读取服务器发的数据方法二
        clientSocket.Receive(result);//将socket接受到的数据存入缓冲区
        ByteBuffer buffer = new ByteBuffer(result);
        buffer.ReadShort();//???
        string data = buffer.ReadString();
        ButtonFunction.textString += "服务器返回数据：" + data + "\n";
        */
    }

    //------------------------------------------------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------

    /// <summary>
    /// 发送数据给服务器
    /// </summary>
    public void SendMessage(string inputData)
    {
        if (!isConnected)//如果没有连接
        {
            return;
        }
        try
        {
            byte[] outputData = ASCIIEncoding.UTF8.GetBytes(inputData);
            clientSocket.Send(outputData);

            /*
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteString(data);
            clientSocket.Send(WriteMessage(buffer.ToBytes()));
            */
        }
        catch
        {
            isConnected = false;
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }

    //---------------------------------------------------------------------------------------------

    /// <summary>
    /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据
    /// </summary>
    private static byte[] WriteMessage(byte[] message)
    {
        MemoryStream stream = null;//创建储存区为内存的流
        using (stream = new MemoryStream())
        {
            stream.Position = 0;
            BinaryWriter writer = new BinaryWriter(stream);//写入流
            short messageLength = (short)message.Length;
            writer.Write(messageLength);
            writer.Write(message);
            writer.Flush();
            return stream.ToArray();
        }
    }

    //-----------------------------------------------------------------------------------------------
}