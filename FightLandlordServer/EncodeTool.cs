using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FightLandlordServer
{
    public static class EncodeTool
    {
        /// <summary>
        /// 构造数据包，包头+包尾
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncodeMessage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())//内存流对象
            {
                using (BinaryWriter bw = new BinaryWriter(ms))//二进制写对象
                {
                    bw.Write(data.Length);
                    bw.Write(data);

                    byte[] byteArray = new byte[(int)ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)byteArray.Length);

                    return byteArray;
                }
            }
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="dataCache"></param>
        /// <returns></returns>
        public static byte[] DecodeMessage(List<byte> dataCache)
        {
            if (dataCache.Count < 4)
                return null;

            using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))//内存流对象
            {
                using (BinaryReader br = new BinaryReader(ms))//二进制读对象
                {
                    int lenth = br.ReadInt32();
                    int surplus = (int)(ms.Length - ms.Position);

                    if (lenth > surplus)
                        return null;

                    byte[] data = br.ReadBytes(lenth);
                    dataCache.Clear();
                    dataCache.AddRange(br.ReadBytes(surplus));

                    return data;
                }
            }
        }

        /// <summary>
        /// 将SocketMsg消息转化为字节数组
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(SocketMsg msg)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(msg.OpCode);
            bw.Write(msg.SubCode);
            if (msg.Value != null)
            {
                byte[] valueBytes = EncodeObj(msg.Value);
                bw.Write(valueBytes);
            }
            byte[] data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
            bw.Close();
            ms.Close();
            return data;
        }


        /// <summary>
        /// 将字节数组转化为SocketMsg消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SocketMsg DecodeMsg(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);

            SocketMsg msg = new SocketMsg();

            int opCode = br.ReadInt32();
            int subCode = br.ReadInt32();

            msg.OpCode = opCode;
            msg.SubCode = subCode;

            if(ms.Length>ms.Position)
            {

                byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));

                object value=DecodeObj(valueBytes);

                msg.Value = value;
            }

            br.Close();
            ms.Close();

            return msg;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <returns></returns>
        public static object DecodeObj(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();

            object value= bf.Deserialize(ms);

            ms.Close();
            return value;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] EncodeObj(object value)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, value);

            byte[] valueBytes = new byte[ms.Length];

            Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);

            ms.Close();

            return valueBytes;
        }
    }
}