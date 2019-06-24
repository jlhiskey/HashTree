using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    public class Node
    {
        public int Parent { get; set; }
        public int Key { get; set; }

        public Object Value { get; set; }

        public int Left { get; set; }
        public int Right { get; set; }

        public Node(int counter, int value)
        {
            Key = counter;
            Value = value;
            Left = 0;
            Right = 0;
            Parent = 0;
        }

        
    }
}
