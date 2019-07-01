using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    public class TreeQueue
    {
        LinkedList<int> Storage { get; set; }

        public TreeQueue()
        {
            Storage = new LinkedList<int>();
        }

        public void Enqueue(int keyValue)
        {
            Storage.AddFirst(keyValue);
        }

        public void Dequeue()
        {
            Storage.RemoveLast();
        }

        public int Peek()
        {
            return Storage.Last.Value;
        }

        public int GrabFront()
        {
            int keyValue = Storage.First.Value;

            Storage.RemoveFirst();

            return keyValue;
        }
    }
}
