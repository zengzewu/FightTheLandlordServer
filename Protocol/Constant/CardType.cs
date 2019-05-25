using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    /// <summary>
    /// 出牌类型
    /// </summary>
    public class CardType
    {
        public const int NONE = 0;
        /// <summary>
        /// 单
        /// </summary>
        public const int SINGLE = 1;
        /// <summary>
        /// 对子
        /// </summary>
        public const int DOUBLE = 2;
        /// <summary>
        /// 三不带
        /// </summary>
        public const int THREEWITHNOTHING = 3;
        /// <summary>
        /// 顺子
        /// </summary>
        public const int STRAIGHT = 4;
        /// <summary>
        /// 三带一
        /// </summary>
        public const int THREEWITHSINGLE = 5;
        /// <summary>
        /// 三带一对
        /// </summary>
        public const int THREEWITHDOUBLE = 6;
        /// <summary>
        /// 普通炸弹
        /// </summary>
        public const int BOOM = 7;
        /// <summary>
        /// 王炸
        /// </summary>
        public const int JOKERBOOM = 8;
        /// <summary>
        /// 连对
        /// </summary>
        public const int LIANDUI = 9;
        /// <summary>
        /// 飞机(不带)
        /// </summary>
        public const int PLANE = 10;

        /// <summary>
        /// 判断出牌类型是不是单
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsSingle(List<CardDto> cardDtos)
        {
            if (cardDtos.Count != 1)
                return false;
            return true;
        }
        /// <summary>
        /// 判断出牌类型是不是对
        /// </summary>
        /// <param name="cardDtos"></param>
        private static bool IsDouble(List<CardDto> cardDtos)
        {
            if (cardDtos.Count == 2 && cardDtos[0].Weight == cardDtos[1].Weight)
                return true;
            return false;
        }
        /// <summary>
        /// 判断出牌类型是不是三不带
        /// </summary>
        /// <returns></returns>
        private static bool IsThreeWithNothing(List<CardDto> cardDtos)
        {
            if (cardDtos.Count == 3 && cardDtos[0].Weight == cardDtos[1].Weight && cardDtos[0].Weight == cardDtos[2].Weight)
                return true;
            return false;
        }
        /// <summary>
        /// 判断出牌类型是不是三带一
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsThreeWithOne(List<CardDto> cardDtos)
        {
            if (cardDtos.Count != 4)
                return false;

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
            if (keyValue.Value != 3)
                return false;
            return true;
        }
        /// <summary>
        /// 判断出牌类型是不是三带一对
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsThreeWithTwo(List<CardDto> cardDtos)
        {
            if (cardDtos.Count != 5)
                return false;

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
            if (dictTemp.Count > 2)
                return false;
            var keyValue = dictTemp.FirstOrDefault(a => a.Value == 3);
            if (keyValue.Value != 3)
                return false;
            return true;
        }
        /// <summary>
        /// 判断出牌类型是不是顺子
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsStraight(List<CardDto> cardDtos)
        {
            if (cardDtos.Count < 5)
                return false;
            cardDtos.Sort((a, b) => a.Weight.CompareTo(b.Weight));
            if (cardDtos.Max(a => a.Weight) > CardWeight.ONE)
                return false;
            for (int i = 0; i < cardDtos.Count - 1; i++)
            {
                if (cardDtos[i + 1].Weight - cardDtos[i].Weight != 1)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 判断是不是连对
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool Isliandui(List<CardDto> cardDtos)
        {
            if (cardDtos.Count < 6)
                return false;
            cardDtos.Sort((a, b) => a.Weight.CompareTo(b.Weight));
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
            if (!dictTemp.Values.All(a => a == 2))
                return false;
            List<int> vs = dictTemp.Keys.ToList();
            for (int i = 0; i < vs.Count - 1; i++)
            {
                if (vs[i + 1] - vs[i] != 1)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 判断是不是普通炸弹
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsBoom(List<CardDto> cardDtos)
        {
            if (cardDtos.Count != 4)
                return false;
            int val = cardDtos[0].Weight;
            if (!cardDtos.All(a => a.Weight == val))
                return false;
            return true;
        }
        /// <summary>
        /// 判断是不是王炸
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsJokerBoom(List<CardDto> cardDtos)
        {
            if (cardDtos.Count != 2)
                return false;
            if (cardDtos[0].Weight == CardWeight.LJOKER || cardDtos[0].Weight == CardWeight.SJOKER)
            {
                if (cardDtos[1].Weight == CardWeight.LJOKER || cardDtos[1].Weight == CardWeight.SJOKER)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断是不是飞机
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        private static bool IsPlane(List<CardDto> cardDtos)
        {
            if (cardDtos.Count < 6)
                return false;
            cardDtos.Sort((a, b) => a.Weight.CompareTo(b.Weight));
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
            if (!dictTemp.All(a => a.Value == 3))
                return false;
            var list1 = dictTemp.Where(a => a.Value == 3).ToList();
            for (int i = 0; i < list1.Count - 1; i++)
            {
                if ((list1[i + 1].Key - list1[i].Key) != 1)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 获取卡牌类型
        /// </summary>
        /// <param name="cardDtos"></param>
        /// <returns></returns>
        public static int GetType(List<CardDto> cardDtos)
        {
            if (IsSingle(cardDtos))
                return CardType.SINGLE;
            if (IsDouble(cardDtos))
                return CardType.DOUBLE;
            if (IsThreeWithNothing(cardDtos))
                return CardType.THREEWITHNOTHING;
            if (IsThreeWithOne(cardDtos))
                return CardType.THREEWITHSINGLE;
            if (IsThreeWithTwo(cardDtos))
                return CardType.THREEWITHDOUBLE;
            if (IsStraight(cardDtos))
                return CardType.STRAIGHT;
            if (Isliandui(cardDtos))
                return CardType.LIANDUI;
            if (IsBoom(cardDtos))
                return CardType.BOOM;
            if (IsJokerBoom(cardDtos))
                return CardType.JOKERBOOM;
            if (IsPlane(cardDtos))
                return CardType.PLANE;
            return CardType.NONE;
        }
    }
}
