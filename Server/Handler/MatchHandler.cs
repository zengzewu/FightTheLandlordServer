using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightLandlordServer;
using FightLandlordServer.Concurrent;
using Protocol.Code;
using Protocol.Dto;
using Server.Cache;
using Server.Model;

namespace Server.Handler
{
    public class MatchHandler : IHandler
    {
        /// <summary>
        /// 房间缓存
        /// </summary>
        RoomCache roomCache = Caches.RoomCache;
        /// <summary>
        /// 角色缓存
        /// </summary>
        UserModelCache userModelCache = Caches.UserModelCache;
        /// <summary>
        /// 账号缓存
        /// </summary>
        AccountCache accountCache = Caches.AccountCache;
        /// <summary>
        /// 开始战斗事件
        /// </summary>
        public event Action<List<int>> StartEvent;

        /// <summary>
        /// 断开连接时
        /// </summary>
        /// <param name="clientPeer"></param>
        public void OnDisconnect(ClientPeer clientPeer)
        {
            leave(clientPeer);
        }
        /// <summary>
        /// 收到消息时
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void OnReceive(ClientPeer clientPeer, int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.MATCH_REQ://匹配请求
                    match(clientPeer);
                    break;
                case MatchCode.READY_REQ://准备请求
                    ready(clientPeer);
                    break;
                case MatchCode.LEAVE_ROOM_REQ://离开房间请求
                    leave(clientPeer);
                    break;
                case MatchCode.UNREADY_REQ://取消准备请求
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="clientPeer"></param>
        private void leave(ClientPeer clientPeer)
        {
            SingleExcute.Instance.Excute(() =>
            {
                if (!accountCache.IsOnline(clientPeer))//判断用户是否在线
                    return;
                int accid = accountCache.GetId(clientPeer);
                if (!userModelCache.IsExistUserModel(accid))//判断是否存在角色
                    return;
                int uid = userModelCache.GetModelByAccid(accid).Id;
                if (!roomCache.IsMatching(uid))//判断当前用户已经在匹配队列
                    return;
                RoomModel roomModel = roomCache.GetRoomModelByUid(uid);
                roomCache.LeaveRoom(uid);
                roomModel.Brocast(uid, clientPeer, OpCode.MATCH, MatchCode.LEAVE_ROOM_BRO);//通知房间内玩家有用户离开
            });
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="clientPeer"></param>
        private void ready(ClientPeer clientPeer)
        {
            SingleExcute.Instance.Excute(() =>
            {
                if (!accountCache.IsOnline(clientPeer))
                    return;
                int accid = accountCache.GetId(clientPeer);
                if (!userModelCache.IsExistUserModel(accid))
                    return;
                int uid = userModelCache.GetModelByAccid(accid).Id;
                RoomModel roomModel = roomCache.GetRoomModelByUid(uid);
                roomModel.Ready(uid);
                roomModel.Brocast(uid, clientPeer, OpCode.MATCH, MatchCode.READY_BRO);
                //判断是否所有玩家都已经准备
                if (roomModel.IsAllReady())
                {
                    //向客户端发送开始游戏的响应
                    roomModel.Brocast(null, null, OpCode.MATCH, MatchCode.START_BRO);
                    StartEvent.Invoke(roomModel.GetAllReadyUserIdInRoom());
                }
            });
        }

        /// <summary>
        /// 开始匹配
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        private void match(ClientPeer clientPeer)
        {
            SingleExcute.Instance.Excute(() =>
            {
                if (!accountCache.IsOnline(clientPeer))//不在线
                    return;
                int aid = accountCache.GetId(clientPeer);
                if (!userModelCache.IsExistUserModel(aid))//当前账户下不存在角色
                    return;
                UserModel userMode = userModelCache.GetModelByAccid(aid);
                if (roomCache.IsMatching(userMode.Id))//当前用户已经在匹配队列
                    return;
                RoomModel room = roomCache.EnterRoom(userMode.Id, clientPeer);
                //通知房间内玩家有新玩家加入,将新加入玩家的数据发送到其他玩家
                UserDto userDto = new UserDto(userMode.Id, userMode.Name, userMode.Been, userMode.WinCount, userMode.FailCount, userMode.EscapeCount, userMode.Lv, userMode.Exp);
                room.Brocast(userDto, clientPeer, OpCode.MATCH, MatchCode.ENTER_ROOM_BRO);
                //通知客户端匹配成功,放回房间数据模型
                RoomDto roomDto = new RoomDto();
                //添加房间用户
                foreach (int uid in room.uidList.Keys)
                {
                    UserModel temp = userModelCache.GetModelByUid(uid);
                    UserDto userDtotemp = new UserDto(temp.Id, temp.Name, temp.Been, temp.WinCount, temp.FailCount, temp.EscapeCount, temp.Lv, temp.Exp);
                    roomDto.Add(userDtotemp);
                }
                roomDto.readyuidList = room.uidReadyList;
                clientPeer.Send(OpCode.MATCH, MatchCode.MATCH_SER, roomDto);
            });
        }
    }
}
