using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Model
{
    /// <summary>
    /// 角色数据模型
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 豆子数量
        /// </summary>
        public int Been { get; set; }
        /// <summary>
        /// 胜场
        /// </summary>
        public int WinCount { get; set; }
        /// <summary>
        /// 负场
        /// </summary>
        public int FailCount { get; set; }
        /// <summary>
        /// 逃跑场
        /// </summary>
        public int EscapeCount { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Lv { get; set; }
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp { get; set; }
        /// <summary>
        /// 账户ID(外键)
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UserModel()
        {

        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="id">角色Id(自增)</param>
        /// <param name="name">角色名字</param>
        /// <param name="accountId">账户Id</param>
        public UserModel(int id,string name,int accountId)
        {
            this.Id = id;
            this.Name = name;
            this.Been = 10000;
            this.WinCount = 0;
            this.FailCount = 0;
            this.EscapeCount = 0;
            this.Lv = 0;
            this.Exp = 0;
            this.AccountId = accountId;
        }

        /// <summary>
        /// 增加经验
        /// </summary>
        /// <param name="addExp"></param>
        public void AddExp(int addExp)
        {
            Exp += addExp;
            if (Exp > Lv * 100)
            {
                Lv++;
            }
        }
    }
}
