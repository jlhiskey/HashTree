using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    public class Hashtree
    {
        /// <summary>
        /// Total nodes within Hashtree
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Keeps track of the next avaliable key value for new nodes being added to tree.
        /// </summary>
        private int NextKeyValue { get; set; }
        /// <summary>
        /// Keeps track of nodes and allows for O(1) lookup of nodes position in the tree.
        /// </summary>
        private Dictionary<int, Node> Storage { get; set; } 
        /// <summary>
        /// Stores key values of nodes that have avaliable children for new nodes.
        /// </summary>
        Queue<int> CanAdd { get; set; }
        /// <summary>
        /// Keeps track of the root of the tree.
        /// </summary>
        public int Root { get; set; }

        /// <summary>
        /// Creates a new Hashtree that will have an empty Storage, an empty CanAdd queue, an initial Size of 0, and NextKeyValue that is initially set to 1 and will be used as the Key value for newly added nodes.
        /// </summary>
        public Hashtree()
        {
            Storage = new Dictionary<int, Node>();
            CanAdd = new Queue<int>();
            Size = 0;
            NextKeyValue = 1;
        }
        /// <summary>
        /// Adds a node to the binary tree with a generic value as input.
        /// </summary>
        /// <param name="value">object value</param>
        public void Add(int value)
        {          
            // Creates a temp node using Counter as the nodes key and user input value as the node's value.
            Node temp = new Node(NextKeyValue, value);

            // Handles an empty tree.
            if (Root == 0)
            {
                Root = temp.Key;
                CanAdd.Enqueue(temp.Key);
                Storage.Add(NextKeyValue, temp);
            }
            // Calls the add helper method with the temp node as input.
            else
            {
                AddHelper(temp);
            }
            // Increases the Size of the Hashtree
            Size = Size + 1;
            // Increases the Counter of the Hashtree 
            NextKeyValue = NextKeyValue + 1;
        }

        /// <summary>
        /// Accepts a node as input and inserts that node into the first avaliable nodes first avaliable child using left child as priority for insert.
        /// </summary>
        /// <param name="temp">Incoming node that will be inserted into tree.</param>
        private void AddHelper(Node temp)
        {
            // Creates a landing point for the TryGetValue from storage.
            Node current;
            // Grabs the first avaliable node from the CanAdd queue. 
            Storage.TryGetValue(CanAdd.Peek(), out current);
            // Checks to see if there is storage avaliable in the current nodes left child.
            if (current.Left == 0)
            {
                // Sets the current nodes left value to the incoming nodes key value.
                current.Left = temp.Key;
                // Sets the incoming nodes parent value to current.
                temp.Parent = current.Key;
                // Adds the incoming node to storage.
                Storage.Add(temp.Key, temp);
                // Adds the incoming node to the CanAdd queue.
                CanAdd.Enqueue(temp.Key);
            }
            // Adds temp node to current nodes right child.
            else
            {
                current.Right = temp.Key;
                temp.Parent = current.Key;
                Storage.Add(temp.Key, temp);
                
                CanAdd.Enqueue(temp.Key);
            }
            // Checks to see if there is any storage space left in the node and dequeues if the nodes children are both occupied.
            if (current.Left != 0 && current.Right != 0)
            {
                CanAdd.Dequeue();
            }
        }

        public void Remove(int value)
        {
            if (!Storage.ContainsKey(value)) return;

            Node current = null;
            Storage.TryGetValue(value, out current);

            Node parent = null;
            Storage.TryGetValue(current.Parent, out parent);

            if (parent == null) return;

            if (current.Left != 0 && current.Right == 0 && parent.Left == current.Key)
            {
                Node left;
                Storage.TryGetValue(current.Left, out left);
                parent.Left = left.Key;
                left.Parent = parent.Key;
            }

            if (current.Left == 0 && current.Right != 0 && parent.Right == current.Key)
            {
                Node right;
                Storage.TryGetValue(current.Left, out right);
                parent.Right = right.Key;
                right.Parent = parent.Key;
            }

            if (CanAdd.Contains(value))
            {
                DeleteFromQueue(value);
            }

            if (current.Left != 0 && current.Right != 0 && parent.Left == current.Key)
            {
                parent.Left = current.Left;
                current.Parent = parent.Key;

                
                Node right;
                Storage.TryGetValue(current.Right, out right);
                Storage.Remove(current.Right);
                AddToAvaliableNode(current, right);
            }

            if (current.Left != 0 && current.Right != 0 && parent.Right == current.Key)
            {
                parent.Right = current.Right;
                
                Node left;
                Storage.TryGetValue(current.Left, out left);
                Storage.Remove(current.Left);
                AddToAvaliableNode(current, left);
            }
            Size--;
            Storage.Remove(value);
           
        }

        public HashSet<int> FindNullChildren(Node root)
        {
            HashSet<int> output = new HashSet<int>();
            Queue<Node> queue = new Queue<Node>();

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current.Left == 0 || current.Right == 0)
                {
                    output.Add(current.Key);
                }

                if (current.Left != 0)
                {
                    Node left;
                    Storage.TryGetValue(current.Left, out left);
                    queue.Enqueue(left);
                }

                if (current.Right != 0)
                {
                    Node right;
                    Storage.TryGetValue(current.Right, out right);
                    queue.Enqueue(right);
                }
            }

            return output;
        }

        public void AddToAvaliableNode(Node badNode, Node addNode)
        {
            HashSet<int> badAdds = FindNullChildren(badNode);
            Queue<int> temp = new Queue<int>();

            while(badAdds.Contains(CanAdd.Peek()))
            {
                temp.Enqueue(CanAdd.Dequeue());
            }

            AddHelper(addNode);

            if (temp.Count > 0)
            {
                while (CanAdd.Count > 0)
                {
                    temp.Enqueue(CanAdd.Dequeue());
                }

                while (temp.Count > 0)
                {
                    CanAdd.Enqueue(temp.Dequeue());
                }
            }
        }

        public void DeleteFromQueue(int value)
        {
            Queue<int> temp = new Queue<int>();

            while (CanAdd.Count > 0)
            {
                int current = CanAdd.Dequeue();

                if (current != value)
                {
                    temp.Enqueue(current);
                }
            }

            while (temp.Count > 0)
            {
                CanAdd.Enqueue(temp.Dequeue());
            }
        }

    }
}
