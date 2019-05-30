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
    /// 角色数据模型缓存层
    /// </summary>
    public class UserModelCache
    {
        /// <summary>
        /// 角色id，角色数据模型字典
        /// </summary>
        private Dictionary<int, UserModel> idModelDict = new Dictionary<int, UserModel>();
        /// <summary>
        /// 账户id，角色id字典
        /// </summary>
        private Dictionary<int, int> accidUidDict = new Dictionary<int, int>();
        /// <summary>
        /// 线程安全Int类型，用于生成角色自增Id
        /// </summary>
        private ConcurrentInt concurrentInt = new ConcurrentInt(-1);

      

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="accid">账户Id</param>
        /// <param name="userName">角色姓名</param>
        /// <returns>返回角色模型</returns>
        public UserModel CreateUserModel(int accid, string userName)
        {
            UserModel userModel = new UserModel(concurrentInt.Add_Get(), userName, accid);
            idModelDict.Add(userModel.Id, userModel);
            accidUidDict.Add(accid, userModel.Id);
            return userModel;
        }
        /// <summary>
        /// 根据账户Id判断此账户下是否存在角色
        /// </summary>
        /// <param name="accId">账户Id</param>
        /// <returns>true存在,false不存在</returns>
        public bool IsExistUserModel(int accId)
        {
            return accidUidDict.ContainsKey(accId);
        }
        /// <summary>
        /// 根据账户Id获取角色模型
        /// </summary>
        /// <param name="accId">账户Id</param>
        /// <returns>返回角色模型</returns>
        public UserModel GetModelByAccid(int accId)
        {
            int userId = accidUidDict[accId];
            return idModelDict[userId];
        }

        /// <summary>
        /// 根据账号ID获取角色ID
        /// </summary>
        /// <returns>角色Id</returns>
        public int GetUserIdByAid(int aid)
        {
            if (accidUidDict.ContainsKey(aid))
                return accidUidDict[aid];
            else
                return -1;

        }

        /// <summary>
        /// 根据玩家Id获取玩家数据模型
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserModel GetModelByUid(int uid)
        {
            return idModelDict[uid];
        }
        /// <summary>
        /// 根据玩家id集合获取玩家数据模型集合
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        public List<UserModel> GetModelsByUids(List<int> uids)
        {
            List<UserModel> list = new List<UserModel>();
            foreach (var uid in uids)
            {
                list.Add(idModelDict[uid]);
            }
            return list;
        }

        /// <summary>
        /// 更新玩家数据
        /// </summary>
        /// <param name="userModel">玩家模型</param>
        public void Update(UserModel userModel)
        {
            idModelDict[userModel.Id] = userModel;
        }
    }
}
