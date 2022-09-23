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

        [SetUp]
        public void Setup()
        {
            Visualisation.Initialise();
        }

        [Test]
        public void VisualisationMovementTest()
        {
            Data.Initialise();
            Data.NextRace();

            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Thread.Sleep(5000);
            
        }
    }
}
