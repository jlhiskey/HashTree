using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    /// <summary>
    /// Node that is used by a Hashtree. 
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Key reference of the node's parent.
        /// </summary>
        public int Parent { get; set; }
        /// <summary>
        /// Key reference used to allow the node to be stored in Hashtree's Storage.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Generic payload of Node.
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        /// Key reference of node's Left child.
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// Key reference of node's Right child.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Constructor that takes in a key reference and a payload value.
        /// </summary>
        /// <param name="key">Integer that will be used to reference nodes position in Hashtree.</param>
        /// <param name="value">Generic variable that is the payload of the node.</param>
        public Node(int key, object value)
        {
            Key = key;
            Value = value;
            Left = 0;
            Right = 0;
            Parent = 0;
        }  
    }
}
