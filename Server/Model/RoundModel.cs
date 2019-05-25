using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Model
{
    /// <summary>
    /// 回合管理类
    /// </summary>
    public class RoundModel
    {
        /// <summary>
        /// 当前回合最大出牌者
        /// </summary>
        public int BiggestUId { get; set; }
        /// <summary>
        /// 当前的出牌者
        /// </summary>
        public int CurrentUId { get; set; }
        /// <summary>
        /// 上一次出牌的长度
        /// </summary>
        public int LastLength { get; set; }
        /// <summary>
        /// 上一次出牌类型
        /// </summary>
        public int LastCardType { get; set; }
        /// <summary>
        /// 上一次出牌的权值
        /// </summary>
        public int LastWeight { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoundModel()
        {
            this.BiggestUId = -1;
            this.CurrentUId = -1;
            this.LastLength = -1;
            this.LastWeight = -1;
            this.LastCardType = -1;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.BiggestUId = -1;
            this.CurrentUId = -1;
            this.LastLength = -1;
            this.LastWeight = -1;
            this.LastCardType = -1;
        }

        /// <summary>
        /// 开始出牌
        /// </summary>
        public void Start(int userId)
        {
            this.CurrentUId = userId;
            this.BiggestUId = userId;
        }

        /// <summary>
        /// 改变回合信息
        /// </summary>
        /// <param name="biggestUId"></param>
        /// <param name="lastLength"></param>
        /// <param name="lastCardType"></param>
        /// <param name="lastWeight"></param>
        public void Change(int biggestUId,int lastLength,int lastCardType,int lastWeight)
        {
            this.BiggestUId = biggestUId;
            this.LastLength = LastLength;
            this.LastCardType = lastCardType;
            this.LastWeight = lastWeight;
        }
        /// <summary>
        /// 改变出牌者
        /// </summary>
        /// <param name="uid"></param>
        public void Turn(int uid)
        {
            this.CurrentUId = uid;
        }
        
    }
}
