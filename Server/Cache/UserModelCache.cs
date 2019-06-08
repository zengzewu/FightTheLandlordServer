using FightLandlordServer;
using FightLandlordServer.Concurrent;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Server.Cache
{
    /// <summary>
    /// 角色数据模型缓存层
    /// </summary>
    public class UserModelCache
    {

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="accid">账户Id</param>
        /// <param name="userName">角色姓名</param>
        /// <returns>返回角色模型</returns>
        public UserModel CreateUserModel(int accid, string userName)
        {
            int res = MySqlHelper.ExecuteNonQuery(ConfigString.ConnStr_fight_the_landord, "insert into user(aid,name) values(@aid,@name)", new MySqlParameter("@aid", accid),
                new MySqlParameter("@name", userName));

            DataRow dataRow = MySqlHelper.ExecuteDataRow(ConfigString.ConnStr_fight_the_landord, "select * from user where aid=@aid",
                new MySqlParameter("@aid", accid));

            int id = Convert.ToInt32(dataRow["id"]);
            int aid = accid;
            string name = dataRow["name"].ToString();
            int been = Convert.ToInt32(dataRow["been"]);
            int exp = Convert.ToInt32(dataRow["exp"]);
            int lv = Convert.ToInt32(dataRow["lv"]);
            int win = Convert.ToInt32(dataRow["win"]);
            int fail = Convert.ToInt32(dataRow["fail"]);
            int escape = Convert.ToInt32(dataRow["escape"]);

            UserModel model = new UserModel();
            model.Id = id;
            model.Aid = aid;
            model.Name = name;
            model.Been = been;
            model.Exp = exp;
            model.Lv = lv;
            model.Win = win;
            model.Fail = fail;
            model.Escape = escape;

            return model;


        }
        /// <summary>
        /// 根据账户Id判断此账户下是否存在角色
        /// </summary>
        /// <param name="accId">账户Id</param>
        /// <returns>true存在,false不存在</returns>
        public bool IsExistUserModel(int accId)
        {
            object obj = MySql.Data.MySqlClient.MySqlHelper.ExecuteScalar(ConfigString.ConnStr_fight_the_landord,
                 "select count(1) from user where aid=@aid", new MySqlParameter("@aid", accId));
            return Convert.ToInt32(obj) > 0;
        }
        /// <summary>
        /// 根据账户Id获取角色模型
        /// </summary>
        /// <param name="accId">账户Id</param>
        /// <returns>返回角色模型</returns>
        public UserModel GetModelByAccid(int accId)
        {
            DataRow dataRow = MySqlHelper.ExecuteDataRow(ConfigString.ConnStr_fight_the_landord, "select * from user where aid=@aid",
                new MySqlParameter("@aid", accId));

            int id = Convert.ToInt32(dataRow["id"]);
            int aid = accId;
            string name = dataRow["name"].ToString();
            int been = Convert.ToInt32(dataRow["been"]);
            int exp = Convert.ToInt32(dataRow["exp"]);
            int lv = Convert.ToInt32(dataRow["lv"]);
            int win = Convert.ToInt32(dataRow["win"]);
            int fail = Convert.ToInt32(dataRow["fail"]);
            int escape = Convert.ToInt32(dataRow["escape"]);

            UserModel model = new UserModel();
            model.Id = id;
            model.Aid = aid;
            model.Name = name;
            model.Been = been;
            model.Exp = exp;
            model.Lv = lv;
            model.Win = win;
            model.Fail = fail;
            model.Escape = escape;

            return model;

        }

        /// <summary>
        /// 根据账号ID获取角色ID
        /// </summary>
        /// <returns>角色Id</returns>
        public int GetUserIdByAid(int aid)
        {
            object obj = MySqlHelper.ExecuteScalar(ConfigString.ConnStr_fight_the_landord, "select id from user where aid=@aid", new MySqlParameter("@aid", aid));

            if (obj == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }

        }

        /// <summary>
        /// 根据玩家Id获取玩家数据模型
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserModel GetModelByUid(int uid)
        {
            DataRow dataRow = MySqlHelper.ExecuteDataRow(ConfigString.ConnStr_fight_the_landord, "select * from user where id=@id",
                new MySqlParameter("@id", uid));
            int id = uid;
            int aid = Convert.ToInt32(dataRow["aid"]);
            string name = dataRow["name"].ToString();
            int been = Convert.ToInt32(dataRow["been"]);
            int exp = Convert.ToInt32(dataRow["exp"]);
            int lv = Convert.ToInt32(dataRow["lv"]);
            int win = Convert.ToInt32(dataRow["win"]);
            int fail = Convert.ToInt32(dataRow["fail"]);
            int escape = Convert.ToInt32(dataRow["escape"]);
            UserModel model = new UserModel();
            model.Id = id;
            model.Aid = aid;
            model.Name = name;
            model.Been = been;
            model.Exp = exp;
            model.Lv = lv;
            model.Win = win;
            model.Fail = fail;
            model.Escape = escape;
            return model;
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
                DataRow dataRow = MySqlHelper.ExecuteDataRow(ConfigString.ConnStr_fight_the_landord, "select * from user where id=@id",
                    new MySqlParameter("@id", uid));
                int id = uid;
                int aid = Convert.ToInt32(dataRow["aid"]);
                string name = dataRow["name"].ToString();
                int been = Convert.ToInt32(dataRow["been"]);
                int exp = Convert.ToInt32(dataRow["exp"]);
                int lv = Convert.ToInt32(dataRow["lv"]);
                int win = Convert.ToInt32(dataRow["win"]);
                int fail = Convert.ToInt32(dataRow["fail"]);
                int escape = Convert.ToInt32(dataRow["escape"]);
                UserModel model = new UserModel();
                model.Id = id;
                model.Aid = aid;
                model.Name = name;
                model.Been = been;
                model.Exp = exp;
                model.Lv = lv;
                model.Win = win;
                model.Fail = fail;
                model.Escape = escape;
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 更新玩家数据
        /// </summary>
        /// <param name="userModel">玩家模型</param>
        public void Update(UserModel userModel)
        {
            MySql.Data.MySqlClient.MySqlHelper.ExecuteNonQuery(ConfigString.ConnStr_fight_the_landord,
                "update user set been=@been,exp=@exp,lv=@lv,win=@win,fail=@fail,escape=@escape where id=@id",
                new MySqlParameter("@been", userModel.Been), new MySqlParameter("@exp", userModel.Exp),
                new MySqlParameter("@lv", userModel.Lv), new MySqlParameter("@win", userModel.Win),
                new MySqlParameter("@fail", userModel.Fail), new MySqlParameter("@escape", userModel.Escape), new MySqlParameter("@id", userModel.Id));
        }
    }
}
