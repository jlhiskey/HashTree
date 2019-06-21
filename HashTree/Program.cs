using System;
using HashTree.Classes;

namespace HashTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtree test = new Hashtree();

            for (int i = 1; i <= 2147483647; i++)
            {
                test.Add(i);
                
            }

            
        }
    }
}
