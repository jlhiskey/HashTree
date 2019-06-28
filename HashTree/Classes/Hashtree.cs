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
        private Queue<int> CanAdd { get; set; }
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
            DeleteFromQueue(keyValue);

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
                // Handles a node that is a left child of the parent.
                if (parent.Left == current.Key)
                {
                    parent.Left = left.Key;
                    left.Parent = parent.Key;
                    AddToAvaliableNode(right.Key);
                }
                // Handles a node that is a right child of the parent.
                else
                {
                    parent.Right = left.Key;
                    left.Parent = parent.Key;
                    AddToAvaliableNode(right.Key);
                }
                
            }

            Size--;
        }

        /// <summary>
        /// Private method that is used by AddAvaliableNodes method to determine which nodes to not add a new node to using a breadth first traversal.
        /// </summary>
        /// <param name="nodeKeyValue">Represents the node key value that will be used for the start of the traversal.</param>
        /// <returns>A hashSet of values that represent nodes that shouldnt be used to add a new node to.</returns>
        private HashSet<int> FindNullChildren(int nodeKeyValue)
        {
            // Creates a hashset that will eventually hold node keys that will represent nodes that have avaliable children.
            HashSet<int> nullChildren = new HashSet<int>();

            // Will be used temporarily store CanAdd ints while the queue is being searched for bad values.ds
            Queue<int> queue = new Queue<int>();

            //Adds the first node to the queue.
            queue.Enqueue(nodeKeyValue);

            // Traverses the tree
            while (queue.Count > 0)
            {
                Node current;
                Storage.TryGetValue(queue.Dequeue(), out current);

                //Checks to see if the current node has avaliable add space for its children
                if (current.Left == 0 || current.Right == 0)
                {
                    // Adds that nodes key to the null children HashSet
                    nullChildren.Add(current.Key);
                }

                // Checks to see if the current node has children

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

        /// <summary>
        /// Retreives a list of badNodes that shouldn't be used when adding a new node using the FindNullChildren method. Then reprioritized the CanAddQueue to filter out all of the nodes that shouldnt be used to add a node then add the incoming node to the tree using the AddHelper method.
        /// </summary>
        /// <param name="nodeKeyValue">Represents the node that will be added to the tree.</param>
        private void AddToAvaliableNode(int nodeKeyValue)
        {
            // Grabs the node that will be added.
            Node temp;
            Storage.TryGetValue(nodeKeyValue, out temp);
            HashSet<int> badAdds = FindNullChildren(nodeKeyValue);
            
            // Accesses the first avaliable node from the CanAdd queue.
            int currentNode = CanAdd.Peek();

            // Checks to see if there are any conflicts and if there are not then it will just add the node to the first avaliable and return. Happy Path
            if (!badAdds.Contains(currentNode))
            {
                AddHelper(temp, false);
                return;
            }

            // Creates a copy of the CanAdd queue.
            Queue<int> queue = CanAdd;
            // Clears the CanAdd queue.
            CanAdd = new Queue<int>();
            // Temp queue that will collect any CanAdd values that are found within the badAdds hashset.
            Queue<int> addAfter = new Queue<int>();

            // Traverses the queue and filters nodeKeys
            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                // If nodeKey is not a badAdd adds value back into CanAdd.
                if (!badAdds.Contains(currentNode))
                {
                    AddToQueue(currentNode, true); 
                }
                // Otherwise it adds it to the addAfter queue.
                else
                {
                    addAfter.Enqueue(currentNode);
                }
            }

            // Adds all keyValues from addAfter back into CanAdd queue.
            while (addAfter.Count > 0)
            {
                AddToQueue(addAfter.Dequeue(), true);               
            }
            // Adds Node back onto tree.
            AddHelper(temp, false);
        }

        /// <summary>
        /// Removes a value from CanAdd queue.
        /// </summary>
        /// <param name="nodeKeyValue"></param>
        private void DeleteFromQueue(int nodeKeyValue)
        {
            if (!CanAdd.Contains(nodeKeyValue)) return;

            Queue<int> temp = new Queue<int>();

            while (CanAdd.Count > 0)
            {
                int current = CanAdd.Dequeue();

                if (current != nodeKeyValue)
                {
                    temp.Enqueue(current);
                }
            }

            while (temp.Count > 0)
            {
                AddToQueue(temp.Dequeue(), false);                
            }
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

        //------ Shared Methods -----------------------------------------------------------------------------------------------------
        // Adds a value to the CanAdd queue.
        private void AddToQueue(int nodeKeyValue, bool isNewNode)
        {
            if (isNewNode)
            {
                CanAdd.Enqueue(nodeKeyValue);
                return;
            }

            Node current = null;
            Storage.TryGetValue(nodeKeyValue, out current);

            if (!CanAdd.Contains(nodeKeyValue))
            {
                if (current.Left == 0 || current.Right == 0)
                {
                    CanAdd.Enqueue(nodeKeyValue);
                }
            }
        }
    }
}
