using Protocol.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 出牌传输数据模型
    /// </summary>
    [Serializable]
    public class DealDto
    {
        /// <summary>
        /// 所出的牌
        /// </summary>
        public List<CardDto> Cards { get; set; }

        /// <summary>
        /// 出牌长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 出牌权值
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 出牌类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 出牌角色
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        /// 是否合法
        /// </summary>
        public bool IsRegular { get; set; }

        /// <summary>
        /// 剩余卡牌
        /// </summary>
        public List<CardDto> RemainCards { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DealDto()
        {

        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cardDtos">所出的牌</param>
        public DealDto(List<CardDto> cardDtos, int uid)
        {
            this.Uid = uid;
            this.Cards = cardDtos;
            this.Length = cardDtos.Count;
            this.Type = CardType.GetType(cardDtos); 
            this.Weight = CardWeight.GetWeight(cardDtos, this.Type);
            this.IsRegular = (this.Type != CardType.NONE);
        }


    }
}
