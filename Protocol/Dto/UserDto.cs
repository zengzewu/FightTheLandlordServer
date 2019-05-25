using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 角色数据传输模型
    /// </summary>
    [Serializable]
    public class UserDto
    {
        /// <summary>
        /// Id
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
        /// 无参构造函数
        /// </summary>
        public UserDto()
        {

        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="been">豆子数量</param>
        /// <param name="winCount">胜场</param>
        /// <param name="failCount">负场</param>
        /// <param name="escapeCount">逃跑场</param>
        /// <param name="lv">等级</param>
        /// <param name="exp">经验</param>
        public UserDto(int id,string name,int been,int winCount,int failCount,int escapeCount,int lv,int exp)
        {
            this.Id = id;
            this.Name = name;
            this.Been = been;
            this.WinCount = winCount;
            this.FailCount = failCount;
            this.EscapeCount = escapeCount;
            this.Lv = lv;
            this.Exp = exp;
        }
    }
}
