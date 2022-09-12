using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Controller.Data.Initialise();
            Controller.Data.NextRace();
            Console.WriteLine($"Op naar: {Controller.Data.CurrentRace.Track.Name}!");

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
