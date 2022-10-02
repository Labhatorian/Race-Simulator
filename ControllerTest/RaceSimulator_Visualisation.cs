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
    internal class RaceSimulator_Visualisation
    {
        Race racetest;
        Track TrackTest;

        [SetUp]
        public void Setup()
        {
            //Visualisation.Initialise();


            //List<IParticipant> participants = new List<IParticipant>();

            //Driver DriverOne = new Driver();
            //DriverOne.Naam = "Max Verstappen";
            //DriverOne.Equipment = new Car();

            //Driver DriverTwo = new Driver();
            //DriverTwo.Naam = "Lewis Hamilton";
            //DriverTwo.Equipment = new Car();

            //participants.Add(DriverOne);
            //participants.Add(DriverTwo);

            //SectionTypes[] sectionTest = new SectionTypes[3];
            //sectionTest[0] = SectionTypes.StartGrid;
            //sectionTest[1] = SectionTypes.Straight;
            //sectionTest[1] = SectionTypes.RightCorner;
            //sectionTest[1] = SectionTypes.LeftCorner;
            //sectionTest[2] = SectionTypes.Finish;

            //TrackTest = new Track("Test", sectionTest);
            //racetest = new Race(TrackTest, participants, 500);
        }

        [Test]
        public void VisualisationTest()
        {
            Data.Initialise();
            Visualisation.Initialise();
            Data.NextRace();
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;

            try
            {
                Visualisation.DrawTrack(Data.CurrentRace.Track, Data.CurrentRace, null, null);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
           // Assert.AreEqual(TrackTest.Name, Data.CurrentRace.Track.Name);
        }
    }
}
