using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 房间数据传输模型
    /// </summary>
    [Serializable]
    public class RoomDto
    {
        /// <summary>
        /// 角色Id，角色数据传输模型
        /// </summary>
        public Dictionary<int, UserDto> uidlistDict { get; set; }
        /// <summary>
        /// 房间内已准备玩家的Id
        /// </summary>
        public List<int> readyuidList { get; set; }
        /// <summary>
        /// 记录玩家进入房间的顺序
        /// </summary>
        public List<int> uidList { get; set; }
        /// <summary>
        /// 左边玩家的Id
        /// </summary>
        public int LeftId { get; set; }
        /// <summary>
        /// 右边玩家的Id
        /// </summary>
        public int RightId { get; set; }
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public RoomDto()
        {
            uidlistDict = new Dictionary<int, UserDto>();
            readyuidList = new List<int>();
            uidList = new List<int>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoomDto(List<UserDto> userDtoList, List<int> readyList)
        {
            foreach (var userDto in userDtoList)
            {
                uidlistDict.Add(userDto.Id, userDto);
            }
            readyuidList = readyList;
        }
        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="userDto"></param>
        public void Add(UserDto userDto)
        {
            this.uidlistDict.Add(userDto.Id, userDto);
            this.uidList.Add(userDto.Id);
        }
        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="uid"></param>
        public void Leave(int uid)
        {
            //是否准备
            if (readyuidList.Contains(uid))
            {
                readyuidList.Remove(uid);
            }

            this.uidlistDict.Remove(uid);
            this.uidList.Remove(uid);
        }
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="uid"></param>
        public void Ready(int uid)
        {
            this.readyuidList.Add(uid);
        }
        /// <summary>
        /// 重置玩家位置
        /// </summary>
        public void ResetPosition(int myUid)
        {
            LeftId = -1;
            RightId = -1;
            if (uidList.Count == 1)
            {
                return;
            }
            if (uidList.Count == 2)
            {
                if (uidList[0] == myUid)
                {
                    RightId = uidList[1];
                }
                else
                {
                    LeftId = uidList[0];
                }
            }
            if (uidList.Count == 3)
            {
                if (uidList[0] == myUid)
                {
                    RightId = uidList[1];
                    LeftId = uidList[2];
                }
                if (uidList[1] == myUid)
                {
                    RightId = uidList[2];
                    LeftId = uidList[0];
                }
                if (uidList[2] == myUid)
                {
                    RightId = uidList[0];
                    LeftId = uidList[1];
                }
            }
        }
    }
}
