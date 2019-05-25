using FightLandlordServer.Concurrent;
using Protocol.Dto;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Cache
{
    /// <summary>
    /// 战斗房间缓存
    /// </summary>
    public class FightRoomCache
    {
        private ConcurrentInt concurrentInt;

        /// <summary>
        /// 战斗房间id 战斗房间模型 字典
        /// </summary>
        private Dictionary<int, FightRoomModel> idRoomDict;


        /// <summary>
        /// 玩家Id 战斗房间数据模型 字典
        /// </summary>
        private Dictionary<int, FightRoomModel> uidRoomDict;

        /// <summary>
        /// 重用 战斗房间 队列
        /// </summary>
        private Queue<FightRoomModel> fightRoomQueue;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FightRoomCache()
        {
            this.idRoomDict = new Dictionary<int, FightRoomModel>();
            this.uidRoomDict = new Dictionary<int, FightRoomModel>();
            this.fightRoomQueue = new Queue<FightRoomModel>();
            this.concurrentInt = new ConcurrentInt(-1);
        }

        /// <summary>
        /// 创建战斗房间,返回战斗房间数据模型
        /// </summary>
        /// <param name="uids">房间玩家id</param>
        /// <returns>战斗房间数据模型</returns>
        public FightRoomModel Create(List<int> uidList)
        {
            List<PlayerDto> players = new List<PlayerDto>();
            foreach (var uid in uidList)
            {
                players.Add(new PlayerDto(uid));
            }
            FightRoomModel fightRoom = null;
            if (fightRoomQueue.Count > 0)
            {
                fightRoom = fightRoomQueue.Dequeue();
                fightRoom.Init(players);
            }
            else
            {
                fightRoom = new FightRoomModel(this.concurrentInt.Add_Get(), players);
            }
            this.idRoomDict.Add(fightRoom.Id, fightRoom);

            foreach (var play in players)
            {
                this.uidRoomDict.Add(play.Uid, fightRoom);
            }

            return fightRoom;
        }

        /// <summary>
        /// 根据房间id获取房间数据模型
        /// </summary>
        /// <param name="rid">房间id</param>
        /// <returns>房间数据模型</returns>
        public FightRoomModel GetFightRoomByRoomId(int rid)
        {
            FightRoomModel fightRoom = null;
            idRoomDict.TryGetValue(rid, out fightRoom);
            if (fightRoom == null)
                throw new Exception("未获取到房间数据模型");
            return fightRoom;
        }

        /// <summary>
        /// 根据玩家id获取战斗房间数据模型
        /// </summary>
        /// <param name="uid">玩家id</param>
        /// <returns>房间数据模型</returns>
        public FightRoomModel GetFightRoomByUid(int uid)
        {
            FightRoomModel fightRoom = null;
            uidRoomDict.TryGetValue(uid, out fightRoom);
            return fightRoom;
        }


        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="fightRoom">房间数据模型</param>
        public void DestoryRoom(FightRoomModel fightRoom)
        {
            //将玩家从 玩家，战斗房间字典里面移除
            foreach (var player in fightRoom.PlayerDtos)
            {
                uidRoomDict.Remove(player.Uid);
            }

            //情空房间数据
            fightRoom.Clear();
            this.idRoomDict.Remove(fightRoom.Id);
            fightRoomQueue.Enqueue(fightRoom);
        }
    }
}
