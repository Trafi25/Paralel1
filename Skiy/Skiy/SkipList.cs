using System;
using Skiy.Util;

namespace Skiy
{
    public class SkipList<T>
    {
        private Node<T> head { get; } = new Node<T>(int.MinValue);

        private Node<T> tail { get; } = new Node<T>(int.MaxValue);

        public SkipList()
        {
            for (var i = 0; i < head.Next.Length; ++i)
            {
                head.Next[i] = new MarkedReference<Node<T>>(tail, false);
            }
        }
        

        public bool Add(Node<T> node)
        {
            var previousItem = new Node<T>[Levels.MaxLevel + 1];
            var NextItem = new Node<T>[Levels.MaxLevel + 1];

            while (true)
            {
                if (Find(node, ref previousItem, ref NextItem))
                {
                    return false;
                }
                var highestPoint = node.HighestPoint;

                for (var level = Levels.MinLevel; level <= highestPoint; level++)
                {
                    var tempElem = NextItem[level];
                    node.Next[level] = new MarkedReference<Node<T>>( tempElem, false);
                }

                var currentPrevious = previousItem[Levels.MinLevel];
                var currentNext = NextItem[Levels.MinLevel];

                node.Next[Levels.MinLevel] = new MarkedReference<Node<T>>(currentNext, false);


                for (var level = 1; level <= highestPoint; level++)
                {
                    while (true)
                    {
                        currentPrevious = previousItem[level];
                        currentNext = NextItem[level];

                        if (currentPrevious.Next[level].CompareAndExchange(node, false, currentNext, false))
                        {
                            break;
                        }

                        Find(node, ref previousItem, ref NextItem);
                    }
                }

                return true;
            }
        }
        public bool Remove(Node<T> node)
        {
            var previousItem = new Node<T>[Levels.MaxLevel + 1];
            var NextItem = new Node<T>[Levels.MaxLevel + 1];

            while (true){

                
                if (!Find(node, ref previousItem, ref NextItem))
                {
                    return false;
                }

                Node<T> currentNext;
                for (var level = node.HighestPoint; level > Levels.MinLevel; level--)
                {
                    var isMarked = false;
                    currentNext = node.Next[level].Get(ref isMarked);

                    while (!isMarked)
                    {
                        node.Next[level].CompareAndExchange(currentNext, true, currentNext, false);
                        currentNext = node.Next[level].Get(ref isMarked);
                    }
                }

                var marked = false;
                currentNext = node.Next[Levels.MinLevel].Get(ref marked);

                while (true)
                {
                    var iMarkedIt = node.Next[Levels.MinLevel].CompareAndExchange(currentNext, true, currentNext, false);
                    currentNext = NextItem[Levels.MinLevel].Next[Levels.MinLevel].Get(ref marked);

                    if (iMarkedIt)
                    {
                        Find(node, ref previousItem, ref NextItem);
                        return true;
                    }

                    if (marked)
                    {
                        return false;
                    }
                }
            }
        }
        private bool Find(Node<T> node, ref Node<T>[] previousItem, ref Node<T>[] NextItem)
        {
            var marked = false;
            var isRetryNeeded = false;
            Node<T> searchPoint = null;

            while (true)
            {
                var currentPrevious = head;
                for (var level = Levels.MaxLevel; level >= Levels.MinLevel; level--)
                {
                    searchPoint = currentPrevious.Next[level].Value;
                    while (true)
                    {
                        var currentNext = searchPoint.Next[level].Get(ref marked);
                        while (marked)
                        {
                            var snip = currentPrevious.Next[level].CompareAndExchange(currentNext, false, searchPoint, false);
                            if (!snip)
                            {
                                isRetryNeeded = true;
                                break;
                            }

                            searchPoint = currentPrevious.Next[level].Value;
                            currentNext = searchPoint.Next[level].Get(ref marked);
                        }

                        if (isRetryNeeded)
                        {
                            break;
                        }

                        if (searchPoint.NodeKey < node.NodeKey)
                        {
                            currentPrevious = searchPoint;
                            searchPoint = currentNext;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (isRetryNeeded)
                    {
                        isRetryNeeded = false;
                        continue;
                    }

                    previousItem[level] = currentPrevious;
                    NextItem[level] = searchPoint;
                }

                return searchPoint != null && searchPoint.NodeKey == node.NodeKey;
            }
        }
      
       
    }
}