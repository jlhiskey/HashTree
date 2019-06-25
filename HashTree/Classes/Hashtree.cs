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
                AddToQueue(temp.Key, true);
                Storage.Add(NextKeyValue, temp);
            }
            // Calls the add helper method with the temp node as input.
            else
            {
                AddHelper(temp, true);
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
        private void AddHelper(Node temp, bool isNewNode)
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

                AddToQueue(temp.Key, isNewNode);

                // Adds the incoming node to storage.
                if (!Storage.ContainsKey(temp.Key))
                {
                    Storage.Add(temp.Key, temp);
                }
                // Adds the incoming node to the CanAdd queue.
                

            }
            // Adds temp node to current nodes right child.
            else
            {
                current.Right = temp.Key;
                temp.Parent = current.Key;
                if (!Storage.ContainsKey(temp.Key))
                {
                    Storage.Add(temp.Key, temp);
                }

                AddToQueue(temp.Key, isNewNode);
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

            if (current.Parent == 0) return;

            Storage.Remove(value);

            DeleteFromQueue(value);

            Node parent = null;

            Storage.TryGetValue(current.Parent, out parent);

            Node left = null;

            if (current.Left != 0)
            {
                Storage.TryGetValue(current.Left, out left);
            }

            Node right = null;

            if (current.Right != 0)
            {
                Storage.TryGetValue(current.Right, out right);
            }

            if (left == null && right == null && parent.Left == current.Key)
            {
                parent.Left = 0;
            }

            if (left == null && right == null && parent.Right == current.Key)
            {
                parent.Right = 0;
            }

            if (left != null && right == null)
            {
                if (parent.Left == value)
                {
                    parent.Left = left.Key;
                }
                else
                {
                    parent.Right = left.Key;
                }
                left.Parent = parent.Key;
            }

            if (left == null && right != null)
            {
                if (parent.Left == value)
                {
                    parent.Left = right.Key;
                }
                else
                {
                    parent.Right = right.Key;
                }
                right.Parent = parent.Key;
            }

            if (left != null && right != null)
            {
                if (parent.Left == current.Key)
                {
                    parent.Left = left.Key;
                    left.Parent = parent.Key;
                    AddToAvaliableNode(right.Key);
                }
                else
                {
                    parent.Right = left.Key;
                    left.Parent = parent.Key;
                    AddToAvaliableNode(right.Key);
                }
                
            }



            Size--;
        }

        private HashSet<int> FindNullChildren(int root)
        {
            HashSet<int> nullChildren = new HashSet<int>();

            Queue<int> queue = new Queue<int>();

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Node current;
                Storage.TryGetValue(queue.Dequeue(), out current);

                if (current.Left == 0 || current.Right == 0)
                {
                    nullChildren.Add(current.Key);
                }

                if (current.Left != 0)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != 0)
                {
                    queue.Enqueue(current.Right);
                }

            }

            return nullChildren;
        }

        private void AddToAvaliableNode(int node)
        {
            Node temp;
            Storage.TryGetValue(node, out temp);
            HashSet<int> badAdds = FindNullChildren(node);
            
            int currentNode = CanAdd.Peek();

            if (!badAdds.Contains(currentNode))
            {
                AddHelper(temp, false);
                return;
            }


            Queue<int> queue = CanAdd;
            CanAdd = new Queue<int>();
            Queue<int> addAfter = new Queue<int>();

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                if (!badAdds.Contains(currentNode))
                {
                    AddToQueue(currentNode, false);
                }
                else
                {
                    addAfter.Enqueue(currentNode);
                }
            }

            while (addAfter.Count > 0)
            {
                AddToQueue(addAfter.Dequeue(), false);               
            }

            AddHelper(temp, false);
        }

        private void DeleteFromQueue(int value)
        {
            if (!CanAdd.Contains(value)) return;

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
                AddToQueue(temp.Dequeue(), false);                
            }
        }

        private void AddToQueue(int value, bool isNewNode)
        {
            if (isNewNode)
            {
                CanAdd.Enqueue(value);
                return;
            }

            Node current = null;
            Storage.TryGetValue(value, out current);



            if (!CanAdd.Contains(value))
            {
                if (current.Left == 0 || current.Right == 0)
                {
                    CanAdd.Enqueue(value);
                }
            }
        }

    }
}
