using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightLandlordServer;
using Protocol.Code;
using Protocol.Dto;
using Server.Cache;
using Server.Model;

namespace Server.Handler
{
    public class ChatHandler : IHandler
    {
        public void OnDisconnect(ClientPeer clientPeer)
        {

        }

        public void OnReceive(ClientPeer clientPeer, int subCode, object value)
        {
            switch (subCode)
            {
                case ChatCode.DEFAULT:
                    ChartInfoForword(clientPeer, value);
                    break;
            }
        }
        /// <summary>
        /// 聊天消息转发
        /// </summary>
        /// <param name="clientPeer">发起消息的客户端连接对象</param>
        /// <param name="value">消息内容</param>
        private void ChartInfoForword(ClientPeer clientPeer, object value)
        {
            //根据客户端连接对象获取账号id
            int accid = Caches.AccountCache.GetId(clientPeer);
            //根据账号id获取角色id
            int uid = Caches.UserModelCache.GetModelByAccid(accid).Id;
            //根据角色id获取房间信息
            RoomModel room = Caches.RoomCache.GetRoomModelByUid(uid);
            //对房间内玩家发送了聊天信息
            room.Brocast(new ChatDto(uid, (int)value), clientPeer, OpCode.CHAT, ChatCode.DEFAULT);
            
        }

    }
}
