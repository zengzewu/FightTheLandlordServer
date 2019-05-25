using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class OverDto
    {
        public int WinIdentity { get; set; }
        public List<int> WinUidList { get; set; }
        public int BeenCount { get; set; }
    }
}
