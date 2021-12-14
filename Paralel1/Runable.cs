using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Paralel1
{
    class Runable
    {

        private int OldVal;
        private int NewVal;
        private int value;
        private static object locker = new object();

        public Runable(int processVal) 
        {
           value = processVal;
        }

        public void setValuses(int OldVal, int NewVal) {
            this.OldVal = OldVal;
            this.NewVal = NewVal;
        }
        public Runable(int processVal, int OldVal, int NewVal)
        {
            value = processVal;           
        }

        public void Run() {
            Random rand = new Random();
            for (int i = 1; i < 11; i++) {                
                Thread thread = new Thread(new ParameterizedThreadStart(Operation));               
                thread.Name = $"stream {i}";
                thread.Start();
            }
        }

        public void Operation(Object Obj)
        {
            bool isLock = false;
            try
            {
                Random rand = new Random();
                Console.WriteLine($"{Thread.CurrentThread.Name}:start");
                Monitor.Enter(locker, ref isLock);                
                int oldVal = rand.Next(1, 7), newVal = rand.Next(1, 7) ;
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Stream name is:{Thread.CurrentThread.Name} has two value:\nOld value-|{oldVal}| and " +
                    $"New value-|{newVal}|\nThis value with which we will compare-|{value}|");
                CAS(ref value, oldVal, newVal);
            }
            finally {
                Console.WriteLine($"{Thread.CurrentThread.Name}: Finish");
                if (isLock) {
                    Monitor.Exit(locker);
                }
            }
        }

        public void Operation2(Object Obj)
        {
            bool isLock = false;
            try
            {
                Random rand = new Random();
                Console.WriteLine($"{Thread.CurrentThread.Name}:start");
                Monitor.Enter(locker, ref isLock);                
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Stream name is:{Thread.CurrentThread.Name} has two value:\nOld value-|{OldVal}| and " +
                    $"New value-|{NewVal}|\nThis value with which we will compare-|{value}|");
                CAS(ref value, OldVal, NewVal);
            }
            finally
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: Finish");
                if (isLock)
                {
                    Monitor.Exit(locker);
                }
            }
        }



        void CAS(ref int val, int oldVal, int newVal)
        {

            if (val == oldVal)
            {
                val = newVal;
                Console.WriteLine($"Change the value to |{newVal}|");
            }
        }

    }
}
