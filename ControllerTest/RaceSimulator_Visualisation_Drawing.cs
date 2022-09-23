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
        private static Competition competition { get; set; }

        [SetUp]
        public void Setup()
        {
            //Maak nieuw competitie en voeg circuit en deelnemers toe
            competition = new Competition();

            //Data.Initialise();
            //Data.NextRace();
            Visualisation.Initialise();
        }

        //[Test]
        //public void VisualisationTest()
        //{
        //    SectionTypes[] sectionTypesTest = new SectionTypes[5];
        //    sectionTypesTest[3] = (SectionTypes)3;
        //    sectionTypesTest[0] = (SectionTypes)0;
        //    sectionTypesTest[1] = (SectionTypes)1;
        //    sectionTypesTest[2] = (SectionTypes)2;
        //    sectionTypesTest[4] = (SectionTypes)4;

        //    competition.Participants = new List<IParticipant>();

        //    Driver DriverOne = new Driver();
        //    DriverOne.Naam = "Max Verstappen";
        //    DriverOne.Equipment = new Car();

        //    Driver DriverTwo = new Driver();
        //    DriverTwo.Naam = "Lewis Hamilton";
        //    DriverTwo.Equipment = new Car();

        //    competition.Participants.Add(DriverOne);
        //    competition.Participants.Add(DriverTwo);

        //    Track TrackTest = new Track("VisualisationTest", sectionTypesTest);
        //    Race RaceTest = new Race(TrackTest, competition.Participants);

        //    Visualisation.DrawTrack(TrackTest, RaceTest);

        //    competition.Participants = new List<IParticipant>();
        //    competition.Participants.Add(DriverOne);
        //    RaceTest = new Race(TrackTest, competition.Participants);
        //    Visualisation.DrawTrack(TrackTest, RaceTest);
        //}

        [Test]
        public void VisualisationMovementTest()
        {
            Data.Initialise();
            Visualisation.Initialise();
            Data.NextRace();

            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Thread.Sleep(5000);
            
        }
    }
}
