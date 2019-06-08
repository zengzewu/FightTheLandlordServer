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
    /// 账号缓存层
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        /// 账号名,连接对象字典
        /// </summary>
        private Dictionary<string, ClientPeer> accClientPeer;
        /// <summary>
        /// 连接对象,账号名字典
        /// </summary>
        private Dictionary<ClientPeer, string> clientPeerAcc;
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public AccountCache()
        {
            accClientPeer = new Dictionary<string, ClientPeer>();
            clientPeerAcc = new Dictionary<ClientPeer, string>();
        }
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <returns>true存在,false不存在</returns>
        public bool IsExit(string acc)
        {
            object obj = MySql.Data.MySqlClient.MySqlHelper.ExecuteScalar(ConfigString.ConnStr_fight_the_landord,
                "select count(1) from account where aid=@aid", new MySqlParameter("@aid", acc));
            return Convert.ToInt32(obj) > 0;
        }
        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <param name="pwd">账号密码</param>
        public void Create(string acc, string pwd)
        {
            MySqlHelper.ExecuteNonQuery(ConfigString.ConnStr_fight_the_landord,
                "insert into account values(null,@aid,@pwd)", new MySqlParameter("@aid", acc),
                new MySqlParameter("@pwd", pwd));
        }

        /// <summary>
        /// 根据账号id获取模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountModel GetModel(int id)
        {
            DataRow dataRow = MySqlHelper.ExecuteDataRow(ConfigString.ConnStr_fight_the_landord, "select * from account where id=@id",
                new MySqlParameter("@id", id));
            int aid = Convert.ToInt32(dataRow["id"]);
            string acc = dataRow["aid"].ToString();
            string pwd = dataRow["pwd"].ToString();
            return new AccountModel(aid, acc, pwd);
        }

        /// <summary>
        /// 判断账号和密码是否匹配
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <param name="pwd">账号密码</param>
        /// <returns>true匹配成功,false匹配失败</returns>
        public bool IsMactch(string acc, string pwd)
        {
            object obj = MySql.Data.MySqlClient.MySqlHelper.ExecuteScalar(ConfigString.ConnStr_fight_the_landord,
                 "select count(1) from account where aid=@aid and pwd=@pwd", new MySqlParameter("@aid", acc),
                 new MySqlParameter("@pwd", pwd));
            return Convert.ToInt32(obj) > 0;

        }
        /// <summary>
        /// 判断用户是否在线
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <returns>true在线,false不在线</returns>
        public bool IsOnline(string acc)
        {
            return accClientPeer.ContainsKey(acc);
        }
        /// <summary>
        /// 判断用户是否在线
        /// </summary>
        /// <param name="clientPeer">连接对象</param>
        /// <returns>true在线,false不在线</returns>
        public bool IsOnline(ClientPeer clientPeer)
        {
            return clientPeerAcc.ContainsKey(clientPeer);
        }
        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="clientPeer">连接对象</param>
        public void Online(string acc, ClientPeer clientPeer)
        {
            accClientPeer.Add(acc, clientPeer);
            clientPeerAcc.Add(clientPeer, acc);
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="clientPeer">连接对象</param>
        public void Offline(ClientPeer clientPeer)
        {
            string acc = clientPeerAcc[clientPeer];
            accClientPeer.Remove(acc);
            clientPeerAcc.Remove(clientPeer);
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="acc">账号名</param>
        public void Offline(string acc)
        {
            ClientPeer clientPeer = accClientPeer[acc];
            accClientPeer.Remove(acc);
            clientPeerAcc.Remove(clientPeer);
        }
        /// <summary>
        /// 根据连接对象获取id
        /// </summary>
        /// <param name="clientPeer">连接对象</param>
        /// <returns>账号名</returns>
        public int GetId(ClientPeer clientPeer)
        {
            if (clientPeerAcc.ContainsKey(clientPeer))
            {
                string acc = clientPeerAcc[clientPeer];

                object obj = MySqlHelper.ExecuteScalar(ConfigString.ConnStr_fight_the_landord,
                     "select id from account where aid=@aid", new MySqlParameter("@aid", acc));

                return Convert.ToInt32(obj);
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 根据id获取连接对象
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public ClientPeer GetClientPeerByAcc(string accid)
        {
            if (accClientPeer.ContainsKey(accid))
            {
                return accClientPeer[accid];
            }
            return null;
        }
    }
}
