using FightLandlordServer;
using FightLandlordServer.Concurrent;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Cache
{
    /// <summary>
    /// 房间缓存类
    /// </summary>
    public class RoomCache
    {
        /// <summary>
        /// 线程安全int
        /// </summary>
        ConcurrentInt concurrentInt;
        /// <summary>
        /// 房间Id，房间模型字典
        /// </summary>
        private Dictionary<int, RoomModel> ridRoomModelDict;
        /// <summary>
        /// 角色Id，房间Id字典
        /// </summary>
        private Dictionary<int, int> uidRidDict;
        /// <summary>
        /// 未使用房价队列
        /// </summary>
        public Queue<RoomModel> roomQueue;
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public RoomCache()
        {
            ridRoomModelDict = new Dictionary<int, RoomModel>();
            uidRidDict = new Dictionary<int, int>();
            roomQueue = new Queue<RoomModel>();
            concurrentInt = new ConcurrentInt(-1);
        }
        /// <summary>
        /// 根据用户Id获取房间数据模型
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public RoomModel GetRoomModelByUid(int uid)
        {
            int rid = uidRidDict[uid];
            return ridRoomModelDict[rid];
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="uid"></param>
        public RoomModel EnterRoom(int uid,ClientPeer clientPeer)
        {
            foreach (var room in ridRoomModelDict.Values)
            {
                if (!room.IsFull())
                {
                    room.Enter(uid, clientPeer);
                    uidRidDict.Add(uid, room.Id);
                    return room;
                }
            }
            RoomModel newRoom = null;
            if (roomQueue.Count > 0)
            {
                newRoom = roomQueue.Dequeue();
            }
            else
            {
                newRoom = new RoomModel(concurrentInt.Add_Get());
            }
            ridRoomModelDict.Add(newRoom.Id, newRoom);
            uidRidDict.Add(uid, newRoom.Id);
            newRoom.Enter(uid,clientPeer);
            return newRoom;
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        public void LeaveRoom(int uid)
        {
            RoomModel room = GetRoomModelByUid(uid);
            room.Leave(uid);
            uidRidDict.Remove(uid);
            if (room.GetPerCount() == 0)
            {
                ridRoomModelDict.Remove(room.Id);
                room.Initialise();
                roomQueue.Enqueue(room);
            }
        }
        /// <summary>
        /// 玩家准备
        /// </summary>
        /// <param name="uid">角色Id</param>
        /// <returns>房间数据模型</returns>
        public RoomModel UserReady(int uid)
        {
            int rid = uidRidDict[uid];
            RoomModel room = ridRoomModelDict[rid];
            room.Ready(uid);
            return room;
        }
        /// <summary>
        /// 玩家取消准备
        /// </summary>
        /// <param name="uid"></param>
        public void UserUnReady(int uid)
        {
            int rid = uidRidDict[uid];
            RoomModel room = ridRoomModelDict[rid];
            room.UnReady(uid);
        }
        /// <summary>
        /// 判断当前用户是否正在匹配队列
        /// </summary>
        /// <returns>角色Id</returns>
        public bool IsMatching(int uid)
        {
            return uidRidDict.ContainsKey(uid);
        }
        /// <summary>
        /// 销毁房间
        /// </summary>
        /// <param name="roomModel">房间模型</param>
        public void DestoryRoom(RoomModel roomModel)
        {
            roomModel.Initialise();//初始化房间数据
            roomQueue.Enqueue(roomModel);//将房间加入重用队列
        }
    }
}
