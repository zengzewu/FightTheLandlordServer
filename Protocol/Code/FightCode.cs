using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class FightCode
    {
        /// <summary>
        /// 抢地主请求
        /// </summary>
        public const int GRAB_LANDLORD_CREQ = 0;
        /// <summary>
        /// 抢地主广播
        /// </summary>
        public const int GRAB_LANDLORD_BRO = 1;
        /// <summary>
        /// 转换抢地主
        /// </summary>
        public const int TURN_GRAB_BRO = 2;
        /// <summary>
        /// 出牌请求
        /// </summary>
        public const int DEAL_CREQ = 3;
        /// <summary>
        /// 出牌响应
        /// </summary>
        public const int DEAL_SRES = 4;
        /// <summary>
        /// 出牌广播
        /// </summary>
        public const int DEAL_BRO = 5;
        /// <summary>
        /// 不出牌请求
        /// </summary>
        public const int PASS_CREQ = 6;
        /// <summary>
        /// 不出牌响应
        /// </summary>
        public const int PASS_SRES = 7;
        /// <summary>
        /// 转换出牌
        /// </summary>
        public const int TURN_DEAL_BRO = 8;
        /// <summary>
        /// 有玩家离开
        /// </summary>
        public const int LEAVE_BRO = 9;
        /// <summary>
        /// 游戏结束
        /// </summary>
        public const int OVER = 10;
        /// <summary>
        /// 服务器给客户端发牌
        /// </summary>
        public const int GET_CARD_SRES = 11;
    }
}
