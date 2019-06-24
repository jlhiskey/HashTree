using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    public class Hashtree
    {
        public int Size { get; set; }
        public int Counter { get; set; }
        Dictionary<int, Node> Storage { get; set; } 
        Queue<int> CanAdd { get; set; }
        int Root { get; set; }

        public Hashtree()
        {
            Storage = new Dictionary<int, Node>();
            CanAdd = new Queue<int>();
            Size = 0;
            Counter = 1;
        }
        /// <summary>
        /// Adds a uniquely valued node to the binary tree.
        /// </summary>
        /// <param name="value">int value</param>
        public void Add(int value)
        {          
            
            Node temp = new Node(Counter, value);
            if (Root == 0)
            {
                Root = temp.Key;
                CanAdd.Enqueue(temp.Key);
                Storage.Add(Counter, temp);
            }
            else
            {
                AddHelper(temp);
            }
            Size = Size + 1;
            Counter = Counter + 1;
        }

        public void AddHelper(Node temp)
        {
            Node current;
            Storage.TryGetValue(CanAdd.Peek(), out current);
            if (current.Left == 0)
            {
                current.Left = temp.Key;
                temp.Parent = current.Key;
                Storage.Add(temp.Key, temp);
              
                CanAdd.Enqueue(temp.Key);
            }
            else
            {
                current.Right = temp.Key;
                temp.Parent = current.Key;
                Storage.Add(temp.Key, temp);
                
                CanAdd.Enqueue(temp.Key);
            }
            if (current.Left != 0 && current.Right != 0)
            {
                CanAdd.Dequeue();
            }
        }

        public void Remove(int value)
        {
            if (!Storage.ContainsKey(value)) return;

            Node current;
            Storage.TryGetValue(value, out current);

            Node parent; 
            Storage.TryGetValue(current.Parent, out parent);

            if (current.Left != 0 && current.Right == 0 && parent.Left == current.Key)
            {
                    parent.Left = current.Left;
            }

            if (current.Left == 0 && current.Right != 0 && parent.Right == current.Key)
            {
                parent.Right = current.Right;
            }

            if (CanAdd.Contains(value))
            {
                DeleteFromQueue(value);
            }

            if (current.Left != 0 && current.Right != 0 && parent.Left == current.Key)
            {
                parent.Left = current.Left;

                Storage.Remove(current.Right);
                Node right;
                Storage.TryGetValue(current.Right, out right);
                AddToAvaliableNode(current, right);
            }

            if (current.Left != 0 && current.Right != 0 && parent.Right == current.Key)
            {
                parent.Right = current.Right;
                Storage.Remove(current.Left);
                Node left;
                Storage.TryGetValue(current.Left, out left);
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
