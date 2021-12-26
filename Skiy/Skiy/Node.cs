using Skiy.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skiy
{
    public class Node<T>
    {
        private static uint _randomSeed;
        public T Value { get; }

        public int NodeKey { get; }

        public MarkedReference<Node<T>>[] Next { get; }

        public int HighestPoint { get; }


        public Node(int key)
        {
            NodeKey = key;
            Next = new MarkedReference<Node<T>>[Levels.MaxLevel + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new MarkedReference<Node<T>>(null, false);
            }
            HighestPoint = Levels.MaxLevel;
        }

        public Node(T value, int key)
        {
            Value = value;
            NodeKey = key;
            var height = RandomLevel();
            Next = new MarkedReference<Node<T>>[height + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new MarkedReference<Node<T>>(null, false);
            }
            HighestPoint = height;
        }

        private static int RandomLevel()
        {
            var x = _randomSeed;
            x ^= x << 13;
            x ^= x >> 17;
            _randomSeed = x ^= x << 5;
            if ((x & 0x80000001) != 0)
            {
                return 0;
            }

            var level = 1;
            while (((x >>= 1) & 1) != 0)
            {
                level++;
            }

            return Math.Min(level, Levels.MaxLevel);
        }
    }
}
