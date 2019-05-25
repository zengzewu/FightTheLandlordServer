using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Cache
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public class Caches
    {
        /// <summary>
        /// 账号缓存
        /// </summary>
        public static AccountCache AccountCache { get; set; }
        /// <summary>
        /// 角色缓存
        /// </summary>
        public static UserModelCache UserModelCache { get; set; }
        /// <summary>
        /// 匹配房间缓存
        /// </summary>
        public static RoomCache RoomCache { get; set; }
        /// <summary>
        /// 战斗房间缓存
        /// </summary>
        public static FightRoomCache FightRoomCache { get; set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Caches()
        {
            AccountCache = new AccountCache();
            UserModelCache = new UserModelCache();
            RoomCache = new RoomCache();
            FightRoomCache = new FightRoomCache();
        }
    }
}
