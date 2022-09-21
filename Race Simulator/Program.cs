using Controller;
using Model;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Data.Initialise();
            Visualisation.Initialise();
            Data.NextRace();

            Console.WriteLine($"Op naar: {Data.CurrentRace.Track.Name}!");
            //Console.WriteLine("De kwalificatie is al afgerond!");
            //Console.WriteLine("Hier is het circuit en de startopselling:");
            Visualisation.DrawTrack(Data.CurrentRace.Track, Data.CurrentRace);
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;

            //Hou de console open
            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
