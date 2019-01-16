using System.IO;
using System.Text;
using System;

namespace ServerTest
{
    /// <summary>
    /// 字节流操作类
    /// </summary>
    public class ByteBuffer
    {
        MemoryStream stream = null;//内存流
        BinaryWriter writer = null;//写入流    //Binary二进制
        BinaryReader reader = null;//读取流

        //-------------------------------------------------------

        /// <summary>
        /// 构造函数1
        /// </summary>
        public ByteBuffer()
        {
            stream = new MemoryStream();//初始化内存流
            writer = new BinaryWriter(stream);//初始化写入流
        }

        //-------------------------------------------------------

        /// <summary>
        /// 构造函数2，传入字节参数
        /// </summary>
        public ByteBuffer(byte[] data)
        {
            if (data != null)//如果传入的字节不为空
            {
                stream = new MemoryStream(data);//初始化内存流
                reader = new BinaryReader(stream);//初始化读取流
            }
            else
            {
                stream = new MemoryStream();//初始化内存流
                writer = new BinaryWriter(stream);//初始化写入流
            }
        }

        //--------------------------------------------------------

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (writer != null)
            {
                writer.Close();//如果写入流不等于空，则关闭它
            }
            if (reader != null)
            {
                reader.Close();//如果读取流不等于空，则关闭它
            }

            stream.Close();//关闭内存流
            writer = null;
            reader = null;
            stream = null;
        }

        //------------------------------------------------------

        /// <summary>
        /// 写入一个字节进写入流
        /// </summary>
        public void WriteByte(byte v)
        {
            writer.Write(v);
        }

        //------------------------------------------------------

        /// <summary>
        /// 写入整型进写入流
        /// </summary>
        public void WriteInt(int v)
        {
            writer.Write((int)v);
        }

        //------------------------------------------------------

        /// <summary>
        /// 写入短整型进写入流
        /// </summary>
        public void WriteShort(short v)
        {
            writer.Write((short)v);
        }

        //------------------------------------------------------

        /// <summary>
        /// 写入长整型进写入流
        /// </summary>
        public void WriteLong(long v)
        {
            writer.Write((long)v);
        }

        //------------------------------------------------------

        /// <summary>
        /// 写入浮点型进写入流
        /// </summary>
        public void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);////先把传入的float转换成byte数组      Converter转换器
            Array.Reverse(temp);//反转数组中元素的顺序    Reverse反转
            writer.Write(BitConverter.ToSingle(temp, 0));//写入,   Single就是float类型，32位单精度浮点数。而byte是1个字节，8位。那么4个byte就是32位，能转化成1个float类型
        }

        //------------------------------------------------------

        /// <summary>
        /// 写入双精度进写入流
        /// </summary>
        public void WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);////先把传入的double转换成byte数组
            Array.Reverse(temp);//反转数组中元素的顺序
            writer.Write(BitConverter.ToDouble(temp, 0));//写入double类型
        }

        //-------------------------------------------------------

        /// <summary>
        /// 写入字符串进写入流
        /// </summary>
        public void WriteString(string v)
        {
            byte[] temp = Encoding.UTF8.GetBytes(v);//将string转换为byte数组
            writer.Write((short)temp.Length);//写入byte数组的长度
            writer.Write(temp);//写入string转换后的byte数组
        }

        //-------------------------------------------------------

        /// <summary>
        /// 写入字节数组进写入流
        /// </summary>
        public void WriteBytes(byte[] v)
        {
            writer.Write((int)v.Length);//写入字节数组的长度
            writer.Write(v);//写入字节数组
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取一个字节
        /// </summary>
        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取整型
        /// </summary>
        public int ReadInt()
        {
            return (int)reader.ReadInt32();
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取Short
        /// </summary>
        public short ReadShort()
        {
            return (short)reader.ReadInt16();
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取Long
        /// </summary>
        public long ReadLong()
        {
            return (long)reader.ReadInt64();
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取Float
        /// </summary>
        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取Double
        /// </summary>
        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取String
        /// </summary>
        public string ReadString()
        {
            short lenght = ReadShort();
            byte[] buffer = new byte[lenght];
            buffer = reader.ReadBytes(lenght);
            return Encoding.UTF8.GetString(buffer);
        }

        //-------------------------------------------------------

        /// <summary>
        /// 读取Bytes数组
        /// </summary>
        public byte[] ReadBytes()
        {
            short lenght = ReadShort();
            return reader.ReadBytes(lenght);
        }

        //-------------------------------------------------------

        /// <summary>
        /// 转换为bytes数组类型
        /// </summary>
        public byte[] ToBytes()
        {
            writer.Flush(); //强制将缓冲里的数据写入流里
            return stream.ToArray();
        }

        //-------------------------------------------------------

        /// <summary>
        /// 强制将缓冲里的数据写入流里
        /// </summary>
        public void Flush()
        {
            writer.Flush();
        }

        //-------------------------------------------------------
    }
}