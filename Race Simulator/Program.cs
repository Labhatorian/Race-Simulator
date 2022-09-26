using Controller;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Data.Initialise();
            Visualisation.Initialise();
            Data.NextRace();

            //TODO Verplaats naar NextRace
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
