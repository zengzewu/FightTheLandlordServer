using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FightLandlordServer.Timers
{
    public delegate void TimeDelegate();

    public class TimeModel
    {
        public int id { get; set; }
        public long time { get; set; }

        private TimeDelegate timeDelegate;

        public TimeModel(int id,long time,TimeDelegate timeDelegate)
        {
            this.id = id;
            this.time = time;
            this.timeDelegate = timeDelegate;
        }

        public void Run()
        {
            timeDelegate.Invoke();
        }
    }
}
