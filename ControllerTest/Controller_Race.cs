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
            Data.Initialise();
            Data.Debug = true;
            Data.NextRace();
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
            Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;
            Thread.Sleep(20000);
            Data.Debug = false;
        }

    }
}
