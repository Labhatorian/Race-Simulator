using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Model;
using Race_Simulator;
using System.Data;

namespace ControllerTest
{
    [TestFixture]
    //Verander naam naar project
    internal class RaceSimulator_Visualisation_Drawing
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

            Driver DriverTwo = new Driver();
            DriverTwo.Naam = "Lewis Hamilton";
            DriverTwo.Equipment = new Car();

            Driver DriverThree = new Driver();
            DriverThree.Naam = "Charles Leclerc";
            DriverThree.Equipment = new Car();

            competition.Participants.Add(DriverOne);
            competition.Participants.Add(DriverTwo);
            competition.Participants.Add(DriverThree);

            competition.Tracks = new Queue<Track>();

            SectionTypes[] sectionTest = new SectionTypes[3];
            sectionTest[0] = (SectionTypes)3;
            sectionTest[1] = (SectionTypes)3;
            sectionTest[2] = (SectionTypes)4;

            Track TrackTest = new Track("Test", sectionTest);
            competition.Tracks.Enqueue(TrackTest);
        }


        [Test]
        public void VisualisationMovementTest()
        {
            Data.NextRace();

            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Thread.Sleep(5000);
            
        }


        [Test]
        public void VisualisationFinishedTest()
        {
            Setup();
            Data.NextRace();
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;
            Thread.Sleep(30000);
            Assert.AreEqual(Data.CurrentRace, null);
        }
    }
}
