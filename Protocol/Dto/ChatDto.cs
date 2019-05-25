using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class ChatDto
    {
        /// <summary>
        /// 发起聊天的角色Id
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 聊天信息Id
        /// </summary>
        public int ChatType { get; set; }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ChatDto()
        {

        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="uid">角色Id</param>
        /// <param name="chatType">聊天文字id</param>
        public ChatDto(int uid,int chatType)
        {
            this.Uid = uid;
            this.ChatType = chatType;
        }
    }
}
