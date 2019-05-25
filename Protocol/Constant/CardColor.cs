using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    public class CardColor
    {
        public const int NONE = 0;
        /// <summary>
        /// 梅花
        /// </summary>
        public const int CLUE = 1;
        /// <summary>
        /// 红桃
        /// </summary>
        public const int HEART = 2;
        /// <summary>
        /// 黑桃
        /// </summary>
        public const int SPADE = 3;
        /// <summary>
        /// 方块
        /// </summary>
        public const int SQUARE = 4;
        /// <summary>
        /// 根据花色获取字符串
        /// </summary>
        /// <param name="color">花色</param>
        /// <returns>字符串</returns>
        public static string GetString(int color)
        {
            switch (color)
            {
                case NONE:
                    return "NONE";
                case CLUE:
                    return "CLub";
                case HEART:
                    return "HEART";
                case SPADE:
                    return "SPADE";
                case SQUARE:
                    return "SQUARE";               
                default:
                    return "";
            }
        }
    }
}
