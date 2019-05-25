using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FightLandlordServer.Concurrent
{
    /// <summary>
    /// 一个执行的方法
    /// </summary>
    public delegate void ExcuteDelegate();

    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExcute
    {
        private static SingleExcute instance;

        public static SingleExcute Instance
        {
            get
            {
                lock(o)
                {
                    if (instance == null)
                        instance = new SingleExcute();
                    return instance;
                }
            }
        }

        private static object o = new object();

        private Mutex mutex;

        private SingleExcute()
        {
            mutex = new Mutex();
        }


        public void Excute(ExcuteDelegate excuteDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();

                excuteDelegate();

                mutex.ReleaseMutex();
            }
        }
    }
}
