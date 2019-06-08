using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ConfigString
    {
        public static readonly string ConnStr_fight_the_landord;

        static ConfigString()
        {
            ConnStr_fight_the_landord = ConfigurationManager.AppSettings["ConnStr_fight_the_landord"];
        }
    }
}
