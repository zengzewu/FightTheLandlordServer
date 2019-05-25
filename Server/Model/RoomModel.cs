using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightLandlordServer;
using Protocol.Code;
using Protocol.Dto;

namespace Server.Model
{
    /// <summary>
    /// 房间数据模型
    /// </summary>
    public class RoomModel
    {
        /// <summary>
        /// 房间自增ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 房间内玩家的ID
        /// </summary>
        public Dictionary<int, ClientPeer> uidList { get; set; }
        /// <summary>
        /// 房间内准备玩家的ID
        /// </summary>
        public List<int> uidReadyList { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rid">房间Id</param>
        public RoomModel(int rid)
        {
            this.Id = rid;
            uidList = new Dictionary<int, ClientPeer>();
            uidReadyList = new List<int>();
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="uid">玩家Id</param>
        public void Enter(int uid, ClientPeer clientPeer)
        {
            uidList.Add(uid, clientPeer);
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="uid">玩家Id</param>
        public void Leave(int uid)
        {
            //判断是否已经准备了，如果准备了，从准备列表里面移除
            if(uidReadyList.Contains(uid))
            {
                uidReadyList.Remove(uid);
            }
            //移除房间列表
            uidList.Remove(uid);
            
        }
        /// <summary>
        /// 判断房间是否满了
        /// </summary>
        /// <returns>true表示满了,false表示没满</returns>
        public bool IsFull()
        {
            return uidList.Count >= 3;
        }
        /// <summary>
        /// 玩家准备
        /// </summary>
        /// <param name="uid"></param>
        public void Ready(int uid)
        {
            uidReadyList.Add(uid);
        }
        /// <summary>
        /// 玩家取消准备
        /// </summary>
        /// <param name="uid"></param>
        public void UnReady(int uid)
        {
            uidReadyList.Remove(uid);
        }
        /// <summary>
        /// 初始化房间
        /// </summary>
        public void Initialise()
        {
            uidList.Clear();
            uidReadyList.Clear();
        }
        /// <summary>
        /// 获取房间人数
        /// </summary>
        /// <returns></returns>
        public int GetPerCount()
        {
            return uidList.Count;
        }
        /// <summary>
        /// 判断房间内玩家是否都准备了
        /// </summary>
        /// <returns></returns>
        public bool IsAllReady()
        {
            return uidReadyList.Count >= 3;
        }
        /// <summary>
        /// 获取房间内所有玩家的Id
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllUserInRoom()
        {
            return uidList.Keys.ToList();
        }
        /// <summary>
        /// 获取房间内所有已准备玩家的Id
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllReadyUserIdInRoom()
        {
            return uidReadyList;
        }

        /// <summary>
        /// 向房间内玩家广播消息
        /// </summary>
        /// <param name="clientPeer">不需要通知的客户端连接对象</param>
        public void Brocast(object msg, ClientPeer clientPeer, int opcode,int subCode)
        {
            foreach (var item in uidList.Values)
            {
                if (item == clientPeer)
                    continue;
                item.Send(opcode, subCode, msg);
            }
        }
        /// <summary>
        /// 判断房间是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return uidList.Count == 0;
        }
    }
}
