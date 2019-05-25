using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class AccountDto
    {
        public string acc { get; set; }
        public string pwd { get; set; }

        public AccountDto()
        {
            
        }

        public AccountDto(string acc, string pwd)
        {
            this.acc = acc;
            this.pwd = pwd;
        }
    }
}
