using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightLandlordServer;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer serverPeer = new ServerPeer();

            serverPeer.SetApplication(new NetMsgCenter());

            serverPeer.Start(9999,12);

            while(true)
            {

            }
        }
    }
}
