using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Model
{
    /// <summary>
    /// 牌库
    /// </summary>
    public class LibraryModel
    {
        /// <summary>
        /// 所有牌
        /// </summary>
        private Queue<CardDto> cardDtos;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LibraryModel()
        {
            //创建牌
            create();
            //洗牌
            shuffle();
        }
        /// <summary>
        /// 洗牌
        /// </summary>
        private void shuffle()
        {
            List<CardDto> newList = new List<CardDto>();
            Random r = new Random();
            foreach (CardDto card in cardDtos)
            {
                int index = r.Next(0, newList.Count + 1);
                newList.Insert(index, card);
            }
            cardDtos.Clear();
            foreach (CardDto card in newList)
            {
                cardDtos.Enqueue(card);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            //创建牌
            create();
            //洗牌
            shuffle();
        }

        /// <summary>
        /// 创建牌
        /// </summary>
        private void create()
        {
            cardDtos = new Queue<CardDto>();
            for (int color = CardColor.CLUE; color <= CardColor.SQUARE; color++)
            {
                for (int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    string cardName = CardColor.GetString(color) + CardWeight.GetString(weight);
                    CardDto cardDto = new CardDto(color, weight, cardName);
                    this.cardDtos.Enqueue(cardDto);
                }
            }
            //大王小王
            CardDto sJoker = new CardDto(CardColor.NONE, CardWeight.SJOKER, "SJoker");
            CardDto lJoker = new CardDto(CardColor.NONE, CardWeight.LJOKER, "LJoker");
            this.cardDtos.Enqueue(sJoker);
            this.cardDtos.Enqueue(lJoker);
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        public CardDto Dequeue()
        {
            return cardDtos.Dequeue();
        }
    }
}
