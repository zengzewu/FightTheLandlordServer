using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 匹配相关事件码
    /// </summary>
    public class MatchCode
    {
        /// <summary>
        /// 客户端匹配请求
        /// </summary>
        public const int MATCH_REQ = 0;
        /// <summary>
        /// 服务器匹配响应
        /// </summary>
        public const int MATCH_SER = 1;
        /// <summary>
        /// 客户端进入房间请求
        /// </summary>
        public const int ENTER_ROOM_REQ = 2;
        /// <summary>
        /// 客户端进入房间响应
        /// </summary>
        public const int ENTER_ROOM_BRO = 3;
        /// <summary>
        /// 客户端离开房间请求
        /// </summary>
        public const int LEAVE_ROOM_REQ = 4;
        /// <summary>
        /// 客户端离开房间的响应
        /// </summary>
        public const int LEAVE_ROOM_BRO = 4;
        /// <summary>
        /// 客户端准备的请求
        /// </summary>
        public const int READY_REQ = 5;
        /// <summary>
        /// 客户端准备的响应
        /// </summary>
        public const int READY_BRO = 6;
        /// <summary>
        /// 客户端取消准备请求
        /// </summary>
        public const int UNREADY_REQ = 8;
        /// <summary>
        /// 客户端取消准备响应
        /// </summary>
        public const int UNREADY_BRO = 9;
        /// <summary>
        /// 服务器开始游戏的响应
        /// </summary>
        public const int START_BRO = 7;
    }
}
