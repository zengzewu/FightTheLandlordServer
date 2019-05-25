using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FightLandlordServer.Concurrent
{
    public class ConcurrentInt
    {
        private int value;

        public ConcurrentInt()
        {

        }

        public ConcurrentInt(int value)
        {
            this.value = value;
        }

        public int Add_Get()
        {
            lock(this)
            {
                this.value++;
            }
            return this.value;
        }

        public int Reduce_Get()
        {
            lock (this)
            {
                this.value--;
            }
            return this.value;
        }

        public int GetValue()
        {
            return this.value;
        }
    }
}
