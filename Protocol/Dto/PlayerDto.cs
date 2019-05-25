using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 战斗玩家传输模型
    /// </summary>
    public class PlayerDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 身份
        /// </summary>
        public int Identity { get; set; }
        /// <summary>
        /// 手牌
        /// </summary>
        public List<CardDto> CardDtos { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlayerDto(int uid)
        {
            this.Identity = Protocol.Constant.Identity.FARMER;
            this.Uid = uid;
            this.CardDtos = new List<CardDto>();
        }
        /// <summary>
        /// 添加手牌
        /// </summary>
        /// <param name="cardDto"></param>
        public void AddCard(CardDto cardDto)
        {
            this.CardDtos.Add(cardDto);
        }
        /// <summary>
        /// 是否有手牌
        /// </summary>
        /// <returns>true有手牌,false没有手牌</returns>
        public bool HasCard()
        {
            return CardDtos.Count > 0;
        }

        /// <summary>
        /// 获取手牌数量
        /// </summary>
        /// <returns></returns>
        public int CardCount()
        {
            return this.CardDtos.Count;
        }
        /// <summary>
        /// 移除卡牌
        /// </summary>
        /// <param name="cardDto"></param>
        public void Remove(CardDto cardDto)
        {
            CardDtos.Remove(cardDto);
        }
    }
}
