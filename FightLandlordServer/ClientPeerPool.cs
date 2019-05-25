using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FightLandlordServer
{
    /// <summary>
    /// 客户端连接池
    /// </summary>
    public class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeersQueue;

        public ClientPeerPool(int capacity)
        {
            clientPeersQueue = new Queue<ClientPeer>(capacity);
        }

        public void Enqueue(ClientPeer clientPeer)
        {
            clientPeersQueue.Enqueue(clientPeer);
        }

        public ClientPeer Dequeue()
        {
            return clientPeersQueue.Dequeue();
        }
    }
}
