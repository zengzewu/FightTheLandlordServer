using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    //抢地主传输数据模型
    /// </summary>
    [Serializable]
    public class GrabDto
    {
        /// <summary>
        /// 抢地主的角色ID
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 底牌数据
        /// </summary>
        public List<CardDto> TableCards { get; set; }

        /// <summary>
        /// 地主玩家手牌
        /// </summary>
        public List<CardDto> PlayerCards { get; set; }

        public GrabDto()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uid">角色Id</param>
        /// <param name="cardDtos">底牌列表</param>
        public GrabDto(int uid, List<CardDto> cardDtos,List<CardDto> playerCards)
        {
            this.Uid = uid;
            this.TableCards = cardDtos;
            this.PlayerCards = playerCards;
        }
    }
}
