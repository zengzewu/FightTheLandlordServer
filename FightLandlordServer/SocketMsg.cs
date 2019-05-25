using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FightLandlordServer
{
    /// <summary>
    /// 客户端与服务器之间的通信类
    /// </summary>
    public class SocketMsg
    {
        /// <summary>
        /// 模块码
        /// </summary>
        public int OpCode { get; set; }
        /// <summary>
        /// 事件码
        /// </summary>
        public int SubCode { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SocketMsg()
        {

        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="opCode">模块码</param>
        /// <param name="subCode">事件码</param>
        /// <param name="value">消息体</param>
        public SocketMsg(int opCode, int subCode, object value)
        {
            OpCode = opCode;
            SubCode = subCode;
            Value = value;
        }
    }
}
