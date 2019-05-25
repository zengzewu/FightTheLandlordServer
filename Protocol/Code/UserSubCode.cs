using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 用户模块子操作码
    /// </summary>
    public class UserSubCode
    {
        /// <summary>
        /// 客户端创建角色请求
        /// </summary>
        public const int CREATE_USER_CREQ = 0;
        /// <summary>
        /// 服务器创建角色响应
        /// </summary>
        public const int CREATE_USER_SRES = 1;
        /// <summary>
        /// 客户端获取角色信息请求
        /// </summary>
        public const int GET_USER_INFO_CREQ = 2;
        /// <summary>
        /// 服务器获取角色信息响应
        /// </summary>
        public const int GET_USER_INFO_SRES = 3;
    }
}
