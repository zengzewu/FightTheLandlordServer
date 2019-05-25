using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Model
{
    /// <summary>
    /// 战斗房间模型
    /// </summary>
    public class FightRoomModel
    {
        /// <summary>
        /// 战斗房间Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 房间内玩家
        /// </summary>
        public List<PlayerDto> PlayerDtos { get; set; }
        /// <summary>
        /// 逃跑玩家Id
        /// </summary>
        public List<int> EscapePlayerId { get; set; }
        /// <summary>
        /// 牌库
        /// </summary>
        public LibraryModel LibraryModel { get; set; }
        /// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> TableCardList { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }
        //回合管理类
        public RoundModel RoundModel { get; set; }

        /// <summary>
        /// 中途退出房间
        /// </summary>
        /// <param name="uid"></param>
        public void Leave(int uid)
        {
            this.EscapePlayerId.Add(uid);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerDtos"></param>
        public FightRoomModel(int id, List<PlayerDto> playerDtos)
        {
            this.Id = id;
            this.PlayerDtos = playerDtos;
            this.EscapePlayerId = new List<int>();
            this.LibraryModel = new LibraryModel();
            this.TableCardList = new List<CardDto>();
            this.Multiple = 1;
            this.RoundModel = new RoundModel();
        }
        /// <summary>
        /// 初始化房间数据
        /// </summary>
        public void Init(List<PlayerDto> players)
        {
            this.PlayerDtos = players;
        }

        /// <summary>
        /// 清空房间数据
        /// </summary>
        public void Clear()
        {
            this.PlayerDtos = null;
            this.Multiple = 1;
            this.TableCardList.Clear();
            this.LibraryModel.Init();
            this.EscapePlayerId.Clear();
        }


        /// <summary>
        /// 转换出牌
        /// </summary>
        /// <returns>下一个出牌者</returns>
        public int Turn()
        {
            int current = this.RoundModel.CurrentUId;
            int next = GetNext(current);
            this.RoundModel.Turn(next);
            return next;
        }
        /// <summary>
        /// 获取下一个出牌者
        /// </summary>
        /// <param name="current">当前出牌者</param>
        /// <returns>下一个出牌者</returns>
        public int GetNext(int current)
        {
            int index = this.PlayerDtos.FindIndex(a => a.Uid == current);
            index++;
            if(index>2)
            {
                index = 0;
            }
            return PlayerDtos[index].Uid;
        }

        /// <summary>
        /// 发牌
        /// </summary>
        public void Licensing()
        {
            for (int i = 0; i < 17; i++)
            {
                this.PlayerDtos[0].CardDtos.Add(LibraryModel.Dequeue());
                this.PlayerDtos[1].CardDtos.Add(LibraryModel.Dequeue());
                this.PlayerDtos[2].CardDtos.Add(LibraryModel.Dequeue());
            }
            for (int i = 0; i < 3; i++)
            {
                this.TableCardList.Add(LibraryModel.Dequeue());
            }
        }

        /// <summary>
        /// 获取当前房间最大出牌者Id
        /// </summary>
        /// <returns></returns>
        public int GetCurBiggstPlayerId()
        {
            return this.RoundModel.BiggestUId;
        }

        /// <summary>
        /// 卡牌排序
        /// </summary>
        /// <param name="asc">true 升序,false 倒序</param>
        public void Sort(bool asc = true)
        {
            this.PlayerDtos[0].CardDtos.Sort((a, b) =>
            {
                if (asc)
                    return a.Weight.CompareTo(b.Weight);
                else
                    return a.Weight.CompareTo(b.Weight) * -1;
            });
            this.PlayerDtos[1].CardDtos.Sort((a, b) =>
            {
                if (asc)
                    return a.Weight.CompareTo(b.Weight);
                else
                    return a.Weight.CompareTo(b.Weight) * -1;
            });
            this.PlayerDtos[2].CardDtos.Sort((a, b) =>
            {
                if (asc)
                    return a.Weight.CompareTo(b.Weight);
                else
                    return a.Weight.CompareTo(b.Weight) * -1;
            });
            this.TableCardList.Sort((a, b) =>
            {
                if (asc)
                    return a.Weight.CompareTo(b.Weight);
                else
                    return a.Weight.CompareTo(b.Weight) * -1;
            });
        }

        /// <summary>
        /// 给指定玩家的手牌排序
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="asc"></param>
        public void Sort(int uid, bool asc = true)
        {
            PlayerDto play = PlayerDtos.Find(a => a.Uid == uid);
            if (play != null)
                play.CardDtos.Sort((a, b) =>
                {
                    if (asc)
                        return a.Weight.CompareTo(b.Weight);
                    else
                        return a.Weight.CompareTo(b.Weight) * -1;
                });
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="weight">权值</param>
        /// <param name="type">卡牌类型</param>
        /// <param name="length">卡牌长度</param>
        /// <param name="uid">出牌者</param>
        /// <param name="cardDtos">出牌列表</param>\
        /// <returns>trur 可以出牌，false 不能出牌</returns>
        public bool PlayCard(int weight, int type, int length, int uid, List<CardDto> cardDtos)
        {
            bool candeal = false;

            //类型相同并且权值比上一次大
            if (type == this.RoundModel.LastCardType && weight > this.RoundModel.LastWeight)
            {
                //如果是顺子，还需判断长度
                if (type == CardType.STRAIGHT)
                {
                    if (length == this.RoundModel.LastLength)
                    {
                        candeal = true;
                    }
                }
                else
                {
                    candeal = true;
                }
            }
            //普通炸弹
            else if (type == CardType.BOOM)
            {
                candeal = true;
            }
            //王炸
            else if(type==CardType.JOKERBOOM)
            {
                candeal = true;
            }
            else if(uid==RoundModel.BiggestUId)
            {
                candeal = true;
            }
            if (candeal)
            {
                //移除玩家手牌
                List<CardDto> currentCards = getPlayerCards(uid);
                removeCards(currentCards, cardDtos);
                //如果是普通炸弹需要翻倍
                if (type == CardType.BOOM)
                {
                    this.Multiple *= 2;
                }
                //王炸翻4倍
                else if (type == CardType.JOKERBOOM)
                {
                    this.Multiple *= 4;
                }
                //改变回合信息
                this.RoundModel.Change(uid, length, type, weight);
            }

            return candeal;
        }

        /// <summary>
        /// 获取玩家现有手牌
        /// </summary>
        /// <param name="uid">玩家id</param>
        /// <returns></returns>
        private List<CardDto> getPlayerCards(int uid)
        {
            return this.PlayerDtos.Find(a => a.Uid == uid).CardDtos;
        }

        /// <summary>
        /// 从现有手牌移除卡牌
        /// </summary>
        /// <param name="current">现有手牌</param>
        /// <param name="remove">将要移除的手牌</param>
        private void removeCards(List<CardDto> current, List<CardDto> remove)
        {
            foreach (var item in remove)
            {
                current.RemoveAll(a => a.Name == item.Name);
            }
        }


        /// <summary>
        /// 设置地主身份并且给地主发底牌
        /// </summary>
        /// <param name="uid"></param>
        public void SetLandlord(int uid)
        {
            PlayerDto player = this.PlayerDtos.Find(a => a.Uid == uid);
            player.Identity = Identity.LANDLORD;
            for (int i = 0; i < 3; i++)
            {
                player.AddCard(TableCardList[i]);
            }
            //开始回合
            this.RoundModel.Start(uid);
        }
        /// <summary>
        /// 获取玩家数据模型
        /// </summary>
        /// <param name="uid">玩家id</param>
        /// <returns>玩家数据模型</returns>
        public PlayerDto GetPlayerModel(int uid)
        {
            return this.PlayerDtos.Find(a => a.Uid == uid);
        }

        /// <summary>
        /// 获取玩家身份
        /// </summary>
        /// <param name="uid">玩家id</param>
        /// <returns>玩家身份</returns>
        public int GetPlayerIdentity(int uid)
        {
            return this.PlayerDtos.Find(a => a.Uid == uid).Identity;
        }
        /// <summary>
        /// 获取相同身份玩家id
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<PlayerDto> GetSameIdentityUids(int identity)
        {
            return this.PlayerDtos.FindAll(a => a.Identity == identity);
        }
        /// <summary>
        /// 获取房间内第一个玩家的id
        /// </summary>
        /// <returns></returns>
        public int GetFirstUid()
        {
            return this.PlayerDtos[0].Uid;
        }

        /// <summary>
        /// 判断玩家是否还有手牌
        /// </summary>
        /// <param name="uid">玩家id</param>
        /// <returns>true有手牌，false没有手牌</returns>
        public bool HasCard(int uid)
        {
            return getPlayerCards(uid).Count > 0;
        }

    }
}
