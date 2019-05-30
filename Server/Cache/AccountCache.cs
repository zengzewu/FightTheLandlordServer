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
    /// 账号缓存层
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        /// 账号名,账号模型字典
        /// </summary>
        private Dictionary<string, AccountModel> accModelDict;
        /// <summary>
        /// 账号名,连接对象字典
        /// </summary>
        private Dictionary<string, ClientPeer> accClientPeer;
        /// <summary>
        /// 连接对象,账号名字典
        /// </summary>
        private Dictionary<ClientPeer, string> clientPeerAcc;
        /// <summary>
        /// 账号Id,客户端连接对象
        /// </summary>
        private Dictionary<int, AccountModel> idModelDict;
        /// <summary>
        /// 线程安全Int类型
        /// </summary>
        private ConcurrentInt concurrentInt;
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public AccountCache()
        {
            accModelDict = new Dictionary<string, AccountModel>();
            accClientPeer = new Dictionary<string, ClientPeer>();
            clientPeerAcc = new Dictionary<ClientPeer, string>();
            idModelDict = new Dictionary<int, AccountModel>();
            concurrentInt = new ConcurrentInt(-1);
        }
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <returns>true存在,false不存在</returns>
        public bool IsExit(string acc)
        {
            return accModelDict.ContainsKey(acc);
        }
        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <param name="pwd">账号密码</param>
        public void Create(string acc, string pwd)
        {
            AccountModel model = new AccountModel(concurrentInt.Add_Get(), acc, pwd);
            accModelDict.Add(model.acc, model);
            idModelDict.Add(model.id, model);
        }
        /// <summary>
        /// 根据账号获取模型
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <returns>账号模型</returns>
        public AccountModel GetModel(string acc)
        {
            return accModelDict[acc];
        }

        /// <summary>
        /// 根据账号id获取模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountModel GetModel(int id)
        {
            return idModelDict[id];
        }

        /// <summary>
        /// 判断账号和密码是否匹配
        /// </summary>
        /// <param name="acc">账号名</param>
        /// <param name="pwd">账号密码</param>
        /// <returns>true匹配成功,false匹配失败</returns>
        public bool IsMactch(string acc, string pwd)
        {
            AccountModel model = accModelDict[acc];
            return model.pwd == pwd;
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
            if(clientPeerAcc.ContainsKey(clientPeer))
            {
                string acc = clientPeerAcc[clientPeer];
                AccountModel model = accModelDict[acc];
                return model.id;
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
            if(accClientPeer.ContainsKey(accid))
            {
                return accClientPeer[accid];
            }
            return null;
        }
    }
}
