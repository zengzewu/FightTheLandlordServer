using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 账户模块子操作码
    /// </summary>
    public class AccountSubCode
    {
        /// <summary>
        /// 客户端注册请求
        /// </summary>
        public const int REGISTE_CREQ= 0;
        /// <summary>
        /// 服务器注册响应
        /// </summary>
        public const int REGISTE_SRES = 1;
        /// <summary>
        /// 客户端登录请求
        /// </summary>
        public const int LOGIN_CREQ = 2;
        /// <summary>
        /// 服务器登陆响应
        /// </summary>
        public const int LOGIN_SRES = 3;
    }
}
