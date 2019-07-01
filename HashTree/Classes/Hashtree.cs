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
        private TreeQueue CanAdd { get; set; }
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
            CanAdd = new TreeQueue();
            Size = 0;
            NextKeyValue = 1;
        }

        //------ Add method and Add Related Methods ------------------------------------------------------------------------------------------

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
            }
            // Adds temp node to current nodes right child.
            else
            {
                current.Right = temp.Key;
                temp.Parent = current.Key;                
            }
            // Adds the incoming node to storage.
            if (!Storage.ContainsKey(temp.Key))
            {
                Storage.Add(temp.Key, temp);
            }
            CanAdd.Enqueue(temp.Key);
            // Checks to see if there is any storage space left in the node and dequeues if the nodes children are both occupied.
            if (current.Left != 0 && current.Right != 0)
            {
                CanAdd.Dequeue();
            }
        }

        //------ Remove Method and Remove Related Methods ------------------------------------------------------------------------------------------

        /// <summary>
        /// Removes a node from the tree and restuctures the tree to fill in void space.
        /// </summary>
        /// <param name="keyValue">References the node that will be removed</param>
        public void Remove(int keyValue)
        {
            // Checks to see if the node exists in the tree.
            if (!Storage.ContainsKey(keyValue)) return;

            // Creates a landing point for when the target node is accessed from Storage.
            Node current = null;

            // Retreives the node from storage using input keyValue
            Storage.TryGetValue(keyValue, out current);

            // Currently will not remove the node if it is the root. 
            if (current.Parent == 0) return;

            // Removes the node from Storage
            Storage.Remove(keyValue);

            // Checks the queue and removes the nodes key reference from the CanAdd queue if it had and null children.
            CanAdd.Remove(keyValue);

            // Creates a landing point for the target nodes parent.
            Node parent = null;

            // Retreives the parent node from storage.
            Storage.TryGetValue(current.Parent, out parent);

            // Creates a landing point for when the left node is accessed from Storage.
            Node left = null;

            // Only populates the left node if it exists.
            if (current.Left != 0)
            {
                Storage.TryGetValue(current.Left, out left);
            }

            // Creates a landing point for when the right node is accessed from Storage.
            Node right = null;

            // Only populates the right node if it exists.
            if (current.Right != 0)
            {
                Storage.TryGetValue(current.Right, out right);
            }

            // Handles deleting a left leaf.
            if (left == null && right == null && parent.Left == current.Key)
            {
                parent.Left = 0;
            }

            // Handles deleting a right leaf.
            if (left == null && right == null && parent.Right == current.Key)
            {
                parent.Right = 0;
            }

            // Handles a node that is deleted with a null right child.
            if (left != null && right == null)
            {
                if (parent.Left == keyValue)
                {
                    parent.Left = left.Key;
                }
                else
                {
                    parent.Right = left.Key;
                }
                left.Parent = parent.Key;
            }

            // Handles a node that is deleted with a null left child.
            if (left == null && right != null)
            {
                if (parent.Left == keyValue)
                {
                    parent.Left = right.Key;
                }
                else
                {
                    parent.Right = right.Key;
                }
                right.Parent = parent.Key;
            }

            // Handles all delete cases where the node has both a left and a right child.
            if (left != null && right != null)
            {
                Node replacement;
                Storage.TryGetValue(CanAdd.GrabFront(), out replacement);
                Node pReplacement;
                Storage.TryGetValue(replacement.Parent, out pReplacement);
                if (pReplacement.Left == replacement.Key)
                {
                    pReplacement.Left = 0;
                }
                else
                {
                    pReplacement.Right = 0;
                }

                // Handles a node that is a left child of the parent.
                if (parent.Left == current.Key)
                {
                    parent.Left = replacement.Key;
                }
                // Handles a node that is a right child of the parent.
                else
                {
                    parent.Right = replacement.Key;
                }
                replacement.Parent = parent.Key;
                replacement.Left = left.Key;
                replacement.Right = right.Key;
                left.Parent = replacement.Key;
                right.Parent = replacement.Key;
            }

            Size--;
        }        

        //------ Read Method --------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Retreives a Node using the Node's key and returns the nodes value.
        /// </summary>
        /// <param name="nodeKeyValue">Node's key value</param>
        /// <returns>Node's value payload (object)</returns>
        public object Read(int nodeKeyValue)
        {
            Node current = null;
            Storage.TryGetValue(nodeKeyValue, out current);

            if (current != null)
            {
                return current.Value;
            }

            return null;
        }

        //----- Update Method -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Updates a nodes value using it's key as access.
        /// </summary>
        /// <param name="nodeKeyValue">Node that will be updated's key</param>
        /// <param name="updatedValue">Value that will replace nodes current value.</param>
        public void Update(int nodeKeyValue, object updatedValue)
        {
            Node current = null;
            Storage.TryGetValue(nodeKeyValue, out current);

            if (current != null)
            {
                current.Value = updatedValue;
            }
        }

        //----- Read All Values------------------------------------------------------------------------------
        /// <summary>
        /// ReadAll returns all values in tree in a List<object>using an in-order breadth first traversal.
        /// </summary>
        /// <returns>List<object> all values in tree using in order traversal</returns>
        public List<object> ReadAll()
        {
            List<object> allValues = new List<object>();
            int current = Root;
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(current);
            while (queue.Count > 0)
            {
                current = queue.Dequeue();
                Node currentNode;
                Storage.TryGetValue(current, out currentNode);
                allValues.Add(currentNode.Value);
                if (currentNode.Left != 0)
                {
                    queue.Enqueue(currentNode.Left);
                }
                if (currentNode.Right != 0)
                {
                    queue.Enqueue(currentNode.Right);
                }
            }

            return allValues;
        }
    }
}
