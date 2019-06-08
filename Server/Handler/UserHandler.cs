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
    /// <summary>
    /// 角色模块逻辑层
    /// </summary>
    public class UserHandler : IHandler
    {
        /// <summary>
        /// 账号缓存
        /// </summary>
        AccountCache accountCache = Caches.AccountCache;
        /// <summary>
        /// 角色缓存
        /// </summary>
        UserModelCache userModelCache = Caches.UserModelCache;
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="clientPeer"></param>
        public void OnDisconnect(ClientPeer clientPeer)
        {

        }
        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void OnReceive(ClientPeer clientPeer, int subCode, object value)
        {
            switch (subCode)
            {
                case UserSubCode.CREATE_USER_CREQ:
                    createUser(clientPeer, value.ToString());
                    break;
                case UserSubCode.GET_USER_INFO_CREQ:
                    getUserInfo(clientPeer);
                    break;
            }
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        private void createUser(ClientPeer client, string name)
        {
            if (!accountCache.IsOnline(client))
            {
                return;
            }
            int accid = accountCache.GetId(client);
            if (userModelCache.IsExistUserModel(accid))
            {
                return;
            }
            UserModel userModel = userModelCache.CreateUserModel(accid, name);
            UserDto userDto = new UserDto(userModel.Id,userModel.Name, userModel.Been, userModel.Win, userModel.Fail, userModel.Escape, userModel.Lv, userModel.Exp);
            client.Send(OpCode.USER, UserSubCode.CREATE_USER_SRES, userDto);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        private void getUserInfo(ClientPeer client)
        {
            if (!accountCache.IsOnline(client))
            {
                throw new IndexOutOfRangeException("账号不在线，不能获取角色信息");
            }
            int accid = accountCache.GetId(client);
            if (!userModelCache.IsExistUserModel(accid))
            {
                client.Send(OpCode.USER, UserSubCode.GET_USER_INFO_SRES, null);
                return;
            }
            UserModel userModel = userModelCache.GetModelByAccid(accid);
            UserDto userDto = new UserDto(userModel.Id,userModel.Name, userModel.Been, userModel.Win, userModel.Fail, userModel.Escape, userModel.Lv, userModel.Exp);
            client.Send(OpCode.USER, UserSubCode.GET_USER_INFO_SRES, userDto);
            return;
        }
    }
}
