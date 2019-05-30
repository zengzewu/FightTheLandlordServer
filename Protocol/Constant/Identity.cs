using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    /// <summary>
    /// 身份类
    /// </summary>
    public class Identity
    {
        public const int NONE = 0;
        /// <summary>
        /// 农民
        /// </summary>
        public const int FARMER = 1;
        /// <summary>
        /// 地主
        /// </summary>
        public const int LANDLORD = 2;

        /// <summary>
        /// 根据身份数字获取身份文字
        /// </summary>
        /// <returns></returns>
        public static string GetString(int identity)
        {
            switch(identity)
            {
                case 1:
                    return "农民";
                case 2:
                    return "地主";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 获取相反身份
        /// </summary>
        /// <returns></returns>
        public static string GetOpposite(int identity)
        {
            switch (identity)
            {
                case 1:
                    return "地主";
                case 2:
                    return "农民";
                default:
                    return "未知";
            }
        }
    }
}
