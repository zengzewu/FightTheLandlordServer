﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 模块操作码
    /// </summary>
    public class OpCode
    {
        /// <summary>
        /// 账户模块
        /// </summary>
        public const int ACCOUNT = 0;
        /// <summary>
        /// 角色模块
        /// </summary>
        public const int USER = 1;
        /// <summary>
        /// 匹配模块
        /// </summary>
        public const int MATCH = 2;
        /// <summary>
        /// 聊天模块
        /// </summary>
        public const int CHAT = 3;
        /// <summary>
        /// 战斗模块
        /// </summary>
        public const int FIGHT = 4;
    }
}
