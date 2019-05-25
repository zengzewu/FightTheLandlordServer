using FightLandlordServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Handler;
using Protocol.Code;

namespace Server
{
    public class NetMsgCenter : IApplication
    {
        /// <summary>
        /// 账户逻辑层
        /// </summary>
        private IHandler accountHandler = new AccountHandler();
        /// <summary>
        /// 角色逻辑层
        /// </summary>
        private IHandler userHandler = new UserHandler();
        /// <summary>
        /// 匹配逻辑层
        /// </summary>
        private MatchHandler matchHandler = new MatchHandler();
        /// <summary>
        /// 聊天逻辑层
        /// </summary>
        private IHandler chartHandler = new ChatHandler();
        /// <summary>
        /// 战斗逻辑层
        /// </summary>
        private FightHandler fightHandler = new FightHandler();

        /// <summary>
        /// 构造函数
        /// </summary>
        public NetMsgCenter()
        {
            matchHandler.StartEvent += fightHandler.StartFight;
        }

        /// <summary>
        /// 有新的客户端加入时调用
        /// </summary>
        /// <param name="client"></param>
        public void OnConnect(ClientPeer client)
        {

        }
        /// <summary>
        /// 客户端断开连接调用
        /// </summary>
        /// <param name="client"></param>
        public void OnDisconnect(ClientPeer client)
        {
            fightHandler.OnDisconnect(client);
            chartHandler.OnDisconnect(client);
            matchHandler.OnDisconnect(client);
            userHandler.OnDisconnect(client);
            accountHandler.OnDisconnect(client);

        }
        /// <summary>
        /// 收到客户端发来的消息分发给各个模块
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    accountHandler.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.USER:
                    userHandler.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.MATCH:
                    matchHandler.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.CHAT:
                    chartHandler.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.FIGHT:
                    fightHandler.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
