using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FightLandlordServer
{
    public class ServerPeer
    {

        /// <summary>
        /// 服务器Socket
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// 限制客户端连接数量的信号量
        /// </summary>
        private Semaphore semaphore;


        /// <summary>
        /// 客户端连接池
        /// </summary>
        private ClientPeerPool clientPeerPool;

        /// <summary>
        /// 应用层
        /// </summary>
        private IApplication application;

        /// <summary>
        /// 设置应用层
        /// </summary>
        /// <param name="app"></param>
        public void SetApplication(IApplication app)
        {
            this.application = app;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServerPeer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="maxCount">最大连接数</param>
        public void Start(int port, int maxCount)
        {
            try
            {
                clientPeerPool = new ClientPeerPool(maxCount);
                ClientPeer temp = null;
                for (int i = 0; i < maxCount; i++)
                {
                    temp = new ClientPeer();
                    temp.ReceiveAsync.Completed += ReceiveAsync_Completed;
                    temp.receiveCompleted += Temp_receiveCompleted;
                    temp.sendDisconnect += Disconnect;
                    clientPeerPool.Enqueue(temp);
                }

                semaphore = new Semaphore(maxCount, maxCount);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);
                Console.WriteLine("服务器开启成功...");
                //开始接受客户端连接
                AcceptClientConnecte(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Temp_receiveCompleted(ClientPeer clientPeer, SocketMsg msg)
        {
            application.OnReceive(clientPeer, msg);
        }

        private void ReceiveAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }

        /// <summary>
        /// 接收客户端连接
        /// </summary>
        private void AcceptClientConnecte(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += E_Completed;
            }

            semaphore.WaitOne();

            bool res = serverSocket.AcceptAsync(e);

            if (!res)
            {
                //客户端连接成功，开始处理
                ProcessClientConnecte(e);
            }
        }

        /// <summary>
        /// 有客户端成功连接时候自动触发该方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessClientConnecte(e);
        }


        /// <summary>
        /// 客户端连接成功后的处理方法
        /// </summary>
        /// <param name="e"></param>
        private void ProcessClientConnecte(SocketAsyncEventArgs e)
        {
            ClientPeer clientPeer = clientPeerPool.Dequeue();
            clientPeer.clientSocket = e.AcceptSocket;

            Console.WriteLine("客户端：" + clientPeer.clientSocket.RemoteEndPoint.ToString() + "连接成功...");

            application.OnConnect(clientPeer);//通知应用层，有客户端加入连接

            StartReceive(clientPeer);//ClientPeer接受数据

            e.AcceptSocket = null;
            AcceptClientConnecte(e);//ServerPeer继续接受客户端连接
        }

        private void StartReceive(ClientPeer clientPeer)
        {

            try
            {
                bool res = clientPeer.clientSocket.ReceiveAsync(clientPeer.ReceiveAsync);
                if (!res)
                {
                    ProcessReceive(clientPeer.ReceiveAsync);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs receiveAsync)
        {
            ClientPeer clientPeer = receiveAsync.UserToken as ClientPeer;

            //判断网络消息是否接收成功
            if (clientPeer.ReceiveAsync.SocketError == SocketError.Success && clientPeer.ReceiveAsync.BytesTransferred > 0)
            {
                byte[] packet = new byte[clientPeer.ReceiveAsync.BytesTransferred];

                Buffer.BlockCopy(clientPeer.ReceiveAsync.Buffer, 0, packet, 0, clientPeer.ReceiveAsync.BytesTransferred);

                clientPeer.StartReceive(packet);

                StartReceive(clientPeer);
            }
            else if (clientPeer.ReceiveAsync.BytesTransferred == 0)
            {
                if (clientPeer.ReceiveAsync.SocketError == SocketError.Success)//主动断开连接
                {
                    Disconnect(clientPeer, "客户端主动断开连接");
                }
                else//由于异常断开连接
                {
                    Disconnect(clientPeer, "客户端异常断开连接");
                }
            }
        }

        /// <summary>
        /// 断开连接操作
        /// </summary>
        public void Disconnect(ClientPeer client, string reason)
        {
            if (client == null )
            {
                return;
            }

            Console.WriteLine("客户端：" + client.clientSocket.RemoteEndPoint.ToString() + "断开连接    原因:" + reason);

            application.OnDisconnect(client);

            client.Disconnect();

            clientPeerPool.Enqueue(client);

            semaphore.Release();
        }
    }
}
