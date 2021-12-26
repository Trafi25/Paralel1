using Skiy.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Skiy
{
    class Program
    {

      

        static void Main(string[] args)
        {
            Random random = new Random();
            List<int> pusher = new List<int>();
            SkipList<int> skipList = new SkipList<int>();
            List<int> deleter = new List<int>(); 
            object randomLock = new object();
            ConcurrentStack<Node<int>> elements = new ConcurrentStack<Node<int>>();
            int key;
            lock (randomLock)
            {
                key =random.Next(0, 100000);
            }
         
            var node = new Node<int>(10, key);
            var added = skipList.Add(node);
            if (added)
            {
                elements.Push(node);
                pusher.Add(node.Value);
            }


            if (!elements.TryPop(out var output)) return;
            if (skipList.Remove(output))
            {
                deleter.Add(output.Value);
            }
           
        }

         private static void PrintSkipListForm<T>(SkipList<T> target) where T : IComparable<T>
        {
            for (int i = Levels.MaxLevel; i >= 0; i--)
            {
               
                bool marked = false;
                //var node = target._head.Next[i].Get(ref marked);
               // while (node != target._tail)
                {
                //    Console.Write(node.HighestPoint >= i ? $"{node.NodeValue.Value} " : " ");
                 //   node = node.Next[i].Get(ref marked);
                }

                Console.WriteLine();
            }

            Console.WriteLine("----------------------------");
        }
    }
}
