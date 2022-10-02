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

            Data.SetCompetition(competition);

            competition.Participants = new List<IParticipant>();

            Driver DriverOne = new Driver();
            DriverOne.Naam = "Max Verstappen";
            DriverOne.Equipment = new Car();

            competition.Participants.Add(DriverOne);

            competition.Tracks = new Queue<Track>();

            SectionTypes[] sectionTest = new SectionTypes[2];
            sectionTest[0] = SectionTypes.StartGrid;
            sectionTest[1] = SectionTypes.Finish;

            TrackTest = new Track("Test", sectionTest);
            competition.Tracks.Enqueue(TrackTest);
        }

        Track TrackTest;
        [Test]
        public void RaceTest()
        {
            //Simuleer een volledig race om events te testen
            Data.NextRace(true);
            Visualisation.DrawTrack(Data.CurrentRace.Track, Data.CurrentRace, null, null);
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;
            Thread.Sleep(10000);
            Assert.IsNull(Data.CurrentRace);
        }

    }
}
