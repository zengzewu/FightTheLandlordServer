using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FightLandlordServer
{
    public interface IApplication
    {
        void OnDisconnect(ClientPeer client);

        void OnReceive(ClientPeer client, SocketMsg msg);

        void OnConnect(ClientPeer client);
    }
}
