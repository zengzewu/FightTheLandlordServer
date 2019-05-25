using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightLandlordServer;
using FightLandlordServer.Concurrent;
using Protocol.Code;
using Protocol.Dto;
using Server.Cache;

namespace Server.Handler
{
    public class AccountHandler : IHandler
    {
        private AccountCache accountCache = Caches.AccountCache;

        public void OnDisconnect(ClientPeer clientPeer)
        {
            if(accountCache.IsOnline(clientPeer))
                accountCache.Offline(clientPeer);
        }

        public void OnReceive(ClientPeer clientPeer, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountSubCode.REGISTE_CREQ:
                    {
                        AccountDto accountDto = value as AccountDto;
                        Register(clientPeer, accountDto.acc, accountDto.pwd);
                        break;
                    }
                case AccountSubCode.LOGIN_CREQ:
                    {
                        AccountDto accountDto = value as AccountDto;
                        Login(clientPeer, accountDto.acc, accountDto.pwd);
                        break;
                    }
                default:
                    break;

            }
        }
        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        private void Register(ClientPeer clientPeer, string acc, string pwd)
        {
            SingleExcute.Instance.Excute(() =>
            {
                if (accountCache.IsExit(acc))//账号是否存在
                {
                    clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.REGISTE_SRES, "账号已经存在");
                    return;
                }


                if (string.IsNullOrEmpty(acc))
                {
                    clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.REGISTE_SRES, "输入的账号不合法");
                    return;
                }

                if (pwd.Length < 4 || pwd.Length > 16)
                {
                    clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.REGISTE_SRES, "密码长度不合法");
                    return;
                }

                accountCache.Create(acc, pwd);
                clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.REGISTE_SRES, "注册成功");
            });



        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        public void Login(ClientPeer clientPeer, string acc, string pwd)
        {
            SingleExcute.Instance.Excute(() =>
            {
                if (!accountCache.IsExit(acc))
                {
                    //账号不存在
                    clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.LOGIN_SRES, "账号不存在");
                    return;
                }
                if (accountCache.IsOnline(acc))
                {
                    //账号在线
                    clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.LOGIN_SRES, "账号在线");
                    return;
                }

                if (!accountCache.IsMactch(acc, pwd))
                {
                    //账号密码不匹配
                    clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.LOGIN_SRES, "密码错误");
                    return;
                }

                accountCache.Online(acc, clientPeer);
                clientPeer.Send(OpCode.ACCOUNT, AccountSubCode.LOGIN_SRES, "登录成功");
            });


        }
    }
}