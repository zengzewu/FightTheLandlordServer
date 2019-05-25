using FightLandlordServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Handler
{
    public interface IHandler
    {
        void OnReceive(ClientPeer clientPeer, int subCode,object value);

        void OnDisconnect(ClientPeer clientPeer);
    }
}
