using Controller;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, (Formula 1) World (Championship)!");
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
