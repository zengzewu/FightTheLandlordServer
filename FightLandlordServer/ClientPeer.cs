using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace FightLandlordServer
{
    public class ClientPeer
    {
        public delegate void ReceiveCompleted(ClientPeer clientPeer, SocketMsg value);

        public event ReceiveCompleted receiveCompleted;

        public delegate void SendDisconnect(ClientPeer clientPeer, string reason);

        public event SendDisconnect sendDisconnect;

        public Socket clientSocket { get; set; }

        private List<byte> dataCache;

        private Queue<byte[]> sendMsgQueue;

        private bool isProcess;

        private bool isSend;

        public SocketAsyncEventArgs ReceiveAsync { get; set; }

        public SocketAsyncEventArgs SendAsync { get; set; }


        public ClientPeer()
        {
            isProcess = false;
            isSend = false;
            dataCache = new List<byte>();
            sendMsgQueue = new Queue<byte[]>();
            ReceiveAsync = new SocketAsyncEventArgs();
            ReceiveAsync.SetBuffer(new byte[1024],0,1024);
            SendAsync = new SocketAsyncEventArgs();
            SendAsync.Completed += SendAsync_Completed;
            ReceiveAsync.UserToken = this;
        }

        private void SendAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            SendComplete();
        }

        public void StartReceive(byte[] byteArray)
        {
            dataCache.AddRange(byteArray);
            if (!isProcess)
            {
                ProcessReceive();
            }
        }

        private void ProcessReceive()
        {
            isProcess = true;
            byte[] data = EncodeTool.DecodeMessage(dataCache);
            if (data == null)
            {
                isProcess = false;
                return;
            }
            

            SocketMsg msg = EncodeTool.DecodeMsg(data);

            if (receiveCompleted != null)
                receiveCompleted.Invoke(this, msg);

            ProcessReceive();
        }

        /// <summary>
        /// 断开连接自身的处理
        /// </summary>
        public void Disconnect()
        {
            try
            {
                dataCache.Clear();
                sendMsgQueue.Clear();
                isProcess = false;
                isSend = false;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSocket = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }

        public void Send(int opCode, int subCode, object value)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);

            byte[] packet = EncodeTool.EncodeMessage((EncodeTool.EncodeMsg(msg)));

            sendMsgQueue.Enqueue(packet);

            if (!isSend)
            {
                SendProcess();
            }


        }

        public void Send(byte[] packet)
        {
            sendMsgQueue.Enqueue(packet);

            if (!isSend)
            {
                SendProcess();
            }
        }

        private void SendProcess()
        {
            isSend = true;

            if (sendMsgQueue.Count == 0)
            {
                isSend = false;
                return;
            }

            byte[] packet = sendMsgQueue.Dequeue();

            SendAsync.SetBuffer(packet, 0, packet.Length);

            bool res = clientSocket.SendAsync(SendAsync);

            if (!res)
            {
                SendComplete();
            }
        }

        private void SendComplete()
        {
            if(SendAsync.SocketError!= SocketError.Success)
            {
                //发送出错了，客户端断开连接
                sendDisconnect(this, SendAsync.SocketError.ToString());
                return;
            }
            SendProcess();
        }
    }


}
