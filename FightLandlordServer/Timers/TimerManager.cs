using FightLandlordServer.Concurrent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace FightLandlordServer.Timers
{
    public class TimerManager
    {
        private TimerManager instance;
        public TimerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TimerManager();

                return instance;
            }
        }

        private Timer timer;

        private ConcurrentInt id;

        private ConcurrentDictionary<int, TimeModel> idModelDict;

        private List<int> removeList;

        private TimerManager()
        {
            id = new ConcurrentInt(-1);
            timer = new Timer(10);
            timer.Elapsed += Timer_Elapsed;
            idModelDict = new ConcurrentDictionary<int, TimeModel>();
            removeList = new List<int>();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (removeList)
            {
                TimeModel timeModel = null;
                foreach (var item in removeList)
                {
                    idModelDict.TryRemove(item, out timeModel);//移除已经执行过的对象
                }

                removeList.Clear();
            }

            foreach (var item in idModelDict.Values)
            {
                if (item.time < DateTime.Now.Ticks)
                {
                    item.Run();
                    removeList.Add(item.id);//运行完以后加入移除列表
                }
            }
        }


        public void AddTimeEvent(DateTime dateTime, TimeDelegate timeDelegate)
        {
            long delayTime = dateTime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0)
                return;
            AddTimeEvent(delayTime, timeDelegate);
        }

        public void AddTimeEvent(long delayTime, TimeDelegate timeDelegate)
        {
            TimeModel model = new TimeModel(id.Add_Get(), DateTime.Now.Ticks + delayTime, timeDelegate);
            idModelDict.TryAdd(model.id, model);
        }
    }
}
