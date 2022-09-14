using Controller;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Data.Initialise();
            Data.NextRace();
            Console.WriteLine($"Op naar: {Data.CurrentRace.Track.Name}!");

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
