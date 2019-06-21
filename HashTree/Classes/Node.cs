using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    public class Node
    {
        public int Parent { get; set; }
        public int Value { get; set; }

        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }

        
    }
}
