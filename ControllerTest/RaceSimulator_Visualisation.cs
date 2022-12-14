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

        }

        [Test]
        public void VisualisationEventTest()
        {
            Data.Initialise();
            Visualisation.Initialise();
            Data.NextRace();
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;

            try
            {
                Visualisation.DrawTrack(Data.CurrentRace.Track, null, null);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        [Test]
        public void VisualisationTest()
        {
            //Maak circuit aan en probeer dat te laten zien
            //Maak leeg circuit aan
            SectionTypes[] sectionTypesZandvoort = new SectionTypes[3];
            //Naar boven
            sectionTypesZandvoort[0] = SectionTypes.StartGrid;
            sectionTypesZandvoort[1] = SectionTypes.Straight;
            sectionTypesZandvoort[2] = SectionTypes.Finish;

            Track TrackOne = new Track("Zandvoort", sectionTypesZandvoort);

            Visualisation.DrawTrack(TrackOne, null, null);
            Visualisation.DrawTrack(null, null, null);

        }
    }
}
