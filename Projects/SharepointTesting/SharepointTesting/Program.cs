using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharepointTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            SPTest test = new SPTest();
            test.TestIt();
            Console.WriteLine("press Enter to exit...");
            Console.ReadLine();
        }
    }
}
