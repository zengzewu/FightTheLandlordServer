using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightLandlordServer;
using FightLandlordServer.Concurrent;
using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto;
using Server.Cache;
using Server.Model;

namespace Server.Handler
{
    public class FightHandler : IHandler
    {
        private ConcurrentInt concurrentInt { get; set; }

        private FightRoomCache fightRoomCache = Caches.FightRoomCache;

        private UserModelCache userModelCache = Caches.UserModelCache;

        private AccountCache accountCache = Caches.AccountCache;

        public void OnDisconnect(ClientPeer clientPeer)
        {
            Leave(clientPeer);
        }

        /// <summary>
        /// 玩家中退出房间
        /// </summary>
        /// <param name="clientPeer"></param>
        private void Leave(ClientPeer clientPeer)
        {
            int aid = accountCache.GetId(clientPeer);
            int uid = userModelCache.GetUserIdByAid(aid);
            FightRoomModel room = fightRoomCache.GetFightRoomByUid(uid);
            if(room!=null)
            {
                //战斗还未结束，可以获取到房间数据
                room.Leave(uid);
                //如果房间逃跑用户有三个则直接摧毁房间
                if(room.EscapePlayerId.Count==3)
                {
                    fightRoomCache.DestoryRoom(room);
                }
            }
            else
            {
                //战斗已结束，无法获取到房间数据，无需处理
            }  
        }

        public void OnReceive(ClientPeer clientPeer, int subCode, object value)
        {
            switch (subCode)
            {
                case FightCode.DEAL_CREQ:
                    Deal(clientPeer, (DealDto)value);
                    break;
                case FightCode.GRAB_LANDLORD_CREQ:
                    Grab(clientPeer, (bool)value);
                    break;
                case FightCode.PASS_CREQ:
                    Pass(clientPeer);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 不出牌
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        private void Pass(ClientPeer clientPeer)
        {
            int aid = accountCache.GetId(clientPeer);
            int uid = userModelCache.GetModelByAccid(aid).Id;
            FightRoomModel room = fightRoomCache.GetFightRoomByUid(uid);
            if (room.GetCurBiggstPlayerId() == uid)
            {

            }
            else
            {
                //转换出牌者
                Turn(room, uid);
            }
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="value"></param>
        private void Deal(ClientPeer clientPeer, DealDto dealDto)
        {
            SingleExcute.Instance.Excute(
                delegate ()
                {
                    //出牌者id
                    int uid = dealDto.Uid;
                    FightRoomModel room = fightRoomCache.GetFightRoomByUid(uid);
                    if (room.EscapePlayerId.Contains(uid))
                    {
                        //玩家已经退出房间
                        Turn(room, uid);
                    }
                    else
                    {
                        //玩家没有退出房间,判断玩家是否可以压过上一家出牌者
                        if (room.PlayCard(dealDto.Weight, dealDto.Type, dealDto.Length, dealDto.Uid, dealDto.Cards))
                        {
                            //给玩家发送出牌成功消息
                            clientPeer.Send(OpCode.FIGHT, FightCode.DEAL_SRES, 0);
                            //广播出牌成功消息

                            dealDto.RemainCards = room.GetPlayerModel(uid).CardDtos.Except(dealDto.Cards).ToList();

                            Brocast(room, OpCode.FIGHT, FightCode.DEAL_BRO, dealDto);

                            //判断玩家是否还有手牌
                            if (!room.HasCard(uid))
                            {
                                //没有手牌，结束游戏
                                GameOver(room, uid);
                            }
                            else
                            {
                                //还有手牌,转换出牌
                                Turn(room, uid);
                            }


                        }
                        else
                        {
                            //无法压过上一家的牌,出牌失败的处理
                            clientPeer.Send(OpCode.FIGHT, FightCode.DEAL_SRES, -1);
                        }
                    }
                }
            );
        }

        /// <summary>
        /// 结束游戏
        /// </summary>
        /// <param name="room">房间模型</param>
        /// <param name="uid">获胜角色id</param>
        private void GameOver(FightRoomModel room, int uid)
        {
            //获取获胜者身份
            int identity = room.GetPlayerIdentity(uid);
            if (identity == Identity.LANDLORD)
            {
                //地主获胜
                #region 更新地主玩家的信息，增加豆子，增加经验
                UserModel userModel = userModelCache.GetModelByUid(uid);
                userModel.Been += room.Multiple * 100 * 2;
                userModel.Exp += 20;
                userModelCache.Update(userModel);
                #endregion

                #region 更新农民玩家的信息，减少豆子，增加经验
                room.GetSameIdentityUids(Identity.FARMER).ForEach(u =>
                {
                    UserModel user = userModelCache.GetModelByUid(u.Uid);
                    user.Been -= room.Multiple * 100;
                    user.Exp += 10;
                    userModelCache.Update(user);
                });
                #endregion
            }
            else
            {
                //农民获胜
                #region 更新农民玩家的信息，增加豆子，增加经验
                room.GetSameIdentityUids(Identity.FARMER).ForEach(u =>
                {
                    UserModel user = userModelCache.GetModelByUid(u.Uid);
                    user.Been += room.Multiple * 100;
                    user.Exp += 20;
                    userModelCache.Update(user);
                });
                #endregion

                #region 更新地主玩家的信息，减少豆子，增加经验
                room.GetSameIdentityUids(Identity.LANDLORD).ForEach(u =>
                {
                    UserModel user = userModelCache.GetModelByUid(u.Uid);
                    user.Been -= room.Multiple * 100 * 2;
                    user.Exp += 20;
                    userModelCache.Update(user);
                });
                #endregion
            }

            //惩罚逃跑者
            userModelCache.GetModelsByUids(room.EscapePlayerId).ForEach(u =>
            {
                u.Been -= room.Multiple * 100;
                u.Exp -= 20;
                userModelCache.Update(u);

            });


            OverDto overDto = new OverDto();
            overDto.WinIdentity = identity;
            List<PlayerDto> playerDtos = room.GetSameIdentityUids(identity);
            List<int> uids = new List<int>();
            foreach (var item in playerDtos)
            {
                uids.Add(item.Uid);
            }
            overDto.WinUidList = uids;
            overDto.BeenCount = room.Multiple * 100;
            //广播游戏结束消息
            Brocast(room, OpCode.FIGHT, FightCode.OVER, overDto);

            //摧毁战斗房间
            fightRoomCache.DestoryRoom(room);
        }

        /// <summary>
        /// 转换出牌者
        /// </summary>
        /// <param name="room">房间</param>
        /// <param name="uid">当前出牌者id</param>
        /// <returns>下一个出牌者id</returns>
        private int Turn(FightRoomModel room, int uid)
        {
            while(true)
            {
                int next = room.Turn();
                if (room.EscapePlayerId.Contains(next))
                {
                    continue;
                }
                else
                {
                    Brocast(room, OpCode.FIGHT, FightCode.TURN_DEAL_BRO, next);
                    return next;
                }
            }
        }

        /// <summary>
        /// 抢地主
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        /// <param name="value">请求参数</param>
        private void Grab(ClientPeer clientPeer, bool value)
        {
            SingleExcute.Instance.Excute(() =>
            {
                if (accountCache.IsOnline(clientPeer) == false)
                    return;
                int aid = accountCache.GetId(clientPeer);
                int uid = userModelCache.GetUserIdByAid(aid);
                FightRoomModel fightRoom = fightRoomCache.GetFightRoomByUid(uid);
                if (value == true)
                {
                    fightRoom.SetLandlord(uid);
                    fightRoom.Sort(uid);
                    //发送抢地主消息（包括抢地主的角色Id和底牌数据）给每个客户端发消息
                    Brocast(fightRoom, OpCode.FIGHT, FightCode.GRAB_LANDLORD_BRO, new GrabDto(uid, fightRoom.TableCardList,fightRoom.GetPlayerModel(uid).CardDtos));
                    //通知该玩家出牌
                    Brocast(fightRoom, OpCode.FIGHT, FightCode.TURN_DEAL_BRO, uid);
                }
                else
                {
                    //发送不抢地主的消息
                    int nextUid = fightRoom.GetNext(uid);
                    Brocast(fightRoom, OpCode.FIGHT, FightCode.TURN_GRAB_BRO, nextUid);
                }
            });

        }

        public FightHandler()
        {
            this.concurrentInt = new ConcurrentInt(-1);
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        /// <param name="uidList"></param>
        public void StartFight(List<int> uidList)
        {
            //创建战斗房间
            FightRoomModel fightRoom = fightRoomCache.Create(uidList);
            //发牌
            fightRoom.Licensing();
            //排序
            fightRoom.Sort();
            //给每个客户端发送牌的数据
            foreach (var player in fightRoom.PlayerDtos)
            {
                UserModel userModel = userModelCache.GetModelByUid(player.Uid);
                AccountModel accountModel = accountCache.GetModel(userModel.AccountId);
                ClientPeer clientPeer = accountCache.GetClientPeerByAcc(accountModel.acc);
                clientPeer.Send(OpCode.FIGHT, FightCode.GET_CARD_SRES, player.CardDtos);
            }
            //发送抢地主消息
            int first = fightRoom.GetFirstUid();
            Brocast(fightRoom, OpCode.FIGHT, FightCode.TURN_GRAB_BRO, first, null);
        }


        /// <summary>
        /// 向房间内玩家广播消息
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="fightCode"></param>
        /// <param name="value"></param>
        /// <param name="clientPeer"></param>
        public void Brocast(FightRoomModel fightRoom, int opCode, int fightCode, object value, ClientPeer exClientPeer = null)
        {
            SocketMsg socketMsg = new SocketMsg(opCode, fightCode, value);
            byte[] vs = EncodeTool.EncodeMsg(socketMsg);
            byte[] packet = EncodeTool.EncodeMessage(vs);
            foreach (var player in fightRoom.PlayerDtos)
            {
                UserModel userModel = userModelCache.GetModelByUid(player.Uid);
                AccountModel accountModel = accountCache.GetModel(userModel.AccountId);
                ClientPeer clientPeer = accountCache.GetClientPeerByAcc(accountModel.acc);
                if (clientPeer != exClientPeer)
                    clientPeer.Send(packet);
            }
        }

    }
}
