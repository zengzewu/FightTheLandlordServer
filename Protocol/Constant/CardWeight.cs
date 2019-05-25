using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    /// <summary>
    /// 权值常量类
    /// </summary>
    public class CardWeight
    {
        /// <summary>
        /// 3
        /// </summary>
        public const int THREE = 3;
        /// <summary>
        /// 4
        /// </summary>
        public const int FOUR = 4;
        /// <summary>
        /// 5
        /// </summary>
        public const int FIVE = 5;
        /// <summary>
        /// 6
        /// </summary>
        public const int SIX = 6;
        /// <summary>
        /// 7
        /// </summary>
        public const int SEVEN = 7;
        /// <summary>
        /// 8
        /// </summary>
        public const int EIGHT = 8;
        /// <summary>
        /// 9
        /// </summary>
        public const int NINE = 9;
        /// <summary>
        /// 10
        /// </summary>
        public const int TEN = 10;
        /// <summary>
        /// J
        /// </summary>
        public const int JACK = 11;
        /// <summary>
        /// Q
        /// </summary>
        public const int QUEEN = 12;
        /// <summary>
        /// K
        /// </summary>
        public const int KING = 13;
        /// <summary>
        /// 1
        /// </summary>
        public const int ONE = 14;
        /// <summary>
        /// 2
        /// </summary>
        public const int TWO = 15;
        /// <summary>
        /// 小王
        /// </summary>
        public const int SJOKER = 16;
        /// <summary>
        /// 大王
        /// </summary>
        public const int LJOKER = 17;

        /// <summary>
        /// 根据权值获取字符串
        /// </summary>
        /// <param name="weight">权值</param>
        /// <returns>字符串</returns>
        public static string GetString(int weight)
        {
            switch (weight)
            {
                case THREE:
                    return "Three";
                case FOUR:
                    return "Four";
                case FIVE:
                    return "Five";
                case SIX:
                    return "Six";
                case SEVEN:
                    return "Seven";
                case EIGHT:
                    return "Eight";
                case NINE:
                    return "Nine";
                case TEN:
                    return "Ten";
                case JACK:
                    return "Jack";
                case QUEEN:
                    return "Queen";
                case KING:
                    return "King";
                case ONE:
                    return "One";
                case TWO:
                    return "Two";
                case SJOKER:
                    return "SJoker";
                case LJOKER:
                    return "LJoker";
                default:
                    return "";
            }
        }


        /// <summary>
        /// 获取出牌的权值
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        public static int GetWeight(List<CardDto> cardDtos, int type)
        {
            //计算三不带，三带一，三代一对的权值
            if (type == CardType.THREEWITHDOUBLE || type == CardType.THREEWITHNOTHING || type == CardType.THREEWITHSINGLE)
            {
                Dictionary<int, int> dictTemp = new Dictionary<int, int>();
                foreach (var card in cardDtos)
                {
                    if (!dictTemp.ContainsKey(card.Weight))
                        dictTemp.Add(card.Weight, 1);
                    else
                    {
                        int count = dictTemp[card.Weight];
                        dictTemp.Remove(card.Weight);
                        dictTemp.Add(card.Weight, count + 1);
                    }
                }
                var keyValue = dictTemp.FirstOrDefault(a => a.Value == 3);
                return keyValue.Key * 3;
            }
            //其他牌型
            else
            {
                int total = 0;
                foreach (var card in cardDtos)
                {
                    total += card.Weight;
                }
                return total;
            }
        }
    }
}
