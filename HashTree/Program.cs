using System;
using System.Collections.Generic;
using System.Diagnostics;
using HashTree.Classes;

namespace HashTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            
            Hashtree test = new Hashtree();

            List<object> result;

            int numNodesAdd = 10000000;
            int numNodesRemove = 10000000;
            Console.WriteLine($"Adding {numNodesAdd} nodes.");
            stopwatch.Start();
            for (int i = 1; i <= numNodesAdd; i++)
            {
                test.Add(i);
                
            }
            stopwatch.Stop();
            Console.WriteLine($"Add Complete +{stopwatch.Elapsed}");
            
            stopwatch.Reset();
            Console.WriteLine($"Removing {numNodesRemove} nodes.");
            stopwatch.Start();
            for (int i = numNodesRemove; i > 1; i--)
            {
                test.Remove(i);
            }

            result = test.ReadAll();

            stopwatch.Stop();

            result = test.ReadAll();

            Console.WriteLine($"Remove Complete +{stopwatch.Elapsed}");        
        }
    }
}
