using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Model
{
    public class AccountModel
    {
        public int id { get; set; }
        public string acc { get; set; }
        public string pwd { get; set; }

        public AccountModel()
        {

        }

        public AccountModel(int id,string acc,string pwd)
        {
            this.id = id;
            this.acc = acc;
            this.pwd = pwd;
        }
    }
}
