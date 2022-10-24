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
        //Ook hier zijn we afhanklijk van de race en haar timer functie waardoor dit vrij simpel test is
        public void VisualisationTest()
        {
            //Maak circuit aan en probeer dat te laten zien
            //Maak leeg circuit aan
            
        }
    }
}
