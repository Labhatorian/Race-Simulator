using Controller;
using Model;
using Race_Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    internal class Controller_Race
    {
        Competition competition = new Competition();

        [SetUp]
        public void Setup()
        {
            Visualisation.Initialise();
        }

        Track TrackTest;
        [Test]
        public void RaceTest()
        {
            //Simuleer een volledig race om events te testen
            //Test trouwens rest van de race. Zolang dat niet erroren, is alles prima. Moeilijk te testen
            Data.Initialise();
            Data.Debug = true;
            Data.NextRace();
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;
            Thread.Sleep(20000);
            Data.Debug = false;
        }

        [Test]
        public void LapTest()
        {
            Data.Initialise();
            Thread.Sleep(3000);
            Data.competition.Tracks.Dequeue();
            Data.competition.Tracks.Dequeue();

            SectionTypes[] sectionTypesZandvoort = new SectionTypes[3];
            //Naar boven
            sectionTypesZandvoort[0] = SectionTypes.StartGrid;
            sectionTypesZandvoort[1] = SectionTypes.Straight;
            sectionTypesZandvoort[2] = SectionTypes.Finish;

            Track TrackOne = new Track("Zandvoort", sectionTypesZandvoort);

            Data.competition.Tracks.Enqueue(TrackOne);
            Data.NextRace();

            Boolean lapped = false;
            while (!lapped)
            {
                foreach (KeyValuePair<IParticipant, int> entry in Race._participantslaps)
                {
                    Assert.GreaterOrEqual(entry.Value, 1);
                    lapped = true;
                }
            }
        }
    }
}
