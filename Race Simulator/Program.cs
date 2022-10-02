using Controller;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main()
        {
            //Maak de competitie en race aan
            Console.CursorVisible = false;
            Data.Initialise();
            Visualisation.Initialise();
            Data.NextRace();

            //Voeg voor de eerste keer event handlers toe en laat het circuit zien.
            Visualisation.DrawTrack(Data.CurrentRace.Track, Data.CurrentRace);
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;

            //Hou de console open
            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
