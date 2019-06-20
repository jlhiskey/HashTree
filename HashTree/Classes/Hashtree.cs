using System;
using System.Collections.Generic;
using System.Text;

namespace HashTree.Classes
{
    public class Hashtree
    {
        int Size { get; set; }
        Dictionary<int, Node> Storage { get; set; }
        Queue<int> CanAdd { get; set; }
        Node Root { get; set; }

        public Hashtree()
        {
            Storage = new Dictionary<int, Node>();
            CanAdd = new Queue<int>();
            Size = 0;
        }
        /// <summary>
        /// Adds a uniquely valued node to the binary tree.
        /// </summary>
        /// <param name="value">int value</param>
        public void Add(int value)
        {
            if (Storage.ContainsKey(value)) return;
            
            Node temp = new Node(value);
            if (Root == null)
            {
                Root = temp;
                CanAdd.Enqueue(temp.Value);
                Storage.Add(value, temp);
            }
            else
            {
                Node current;
                Storage.TryGetValue(CanAdd.Peek(), out current);
                if(current.Left == null)
                {
                    current.Left = temp;
                    current.Left.Parent = current;
                    CanAdd.Enqueue(temp.Value);
                    Storage.Add(value, current.Left);
                }
                else
                {
                    current.Right = temp;
                    current.Right.Parent = current;
                    CanAdd.Enqueue(temp.Value);
                    Storage.Add(value, current.Right);
                }
                if (current.Left != null && current.Right != null)
                {
                    CanAdd.Dequeue();
                }
            }
            Size = Size + 1;
        }

        public void Remove(int value)
        {
            if (!Storage.ContainsKey(value)) return;

            Node current;
            Storage.TryGetValue(value, out current);
            Node parent = current.Parent;

            if(parent.Left == current)


            if (current.Left != null && current.Right == null && parent.Left == current)
            {
                    parent.Left = current.Left;
            }

            if (current.Left == null && current.Right != null && parent.Right == current)
            {
                parent.Right = current.Right;
            }

            if (CanAdd.Contains(value))
            {
                DeleteFromQueue(value);
            }

            if (current.Left != null && current.Right != null && parent.Left == current)
            {
                parent.Left = current.Left;

                // Add current.Right to something
                AddToAvaliableNode(current, current.Right);
            }

            if (current.Left != null && current.Right != null && parent.Right == current)
            {
                parent.Right = current.Right;

                AddToAvaliableNode(current, current.Right);
            }

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

                if (current.Left == null || current.Right == null)
                {
                    output.Add(current.Value);
                }

                if (current.Left != null)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    queue.Enqueue(current.Right);
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
                //TODO:
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
