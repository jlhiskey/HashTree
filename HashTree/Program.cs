using System;
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

            int numNodes = 3;

            stopwatch.Start();
            for (int i = 1; i <= numNodes; i++)
            {
                test.Add(i);
                
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            test.Remove(2);

            //for (int i = numNodes; i > 1; i --)
            //{
            //    test.Remove(i);

            //}
        }
    }
}
