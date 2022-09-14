﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Race_Simulator;

namespace ControllerTest
{
    [TestFixture]
    internal class RaceSimulator_Visualisation_Drawing
    {
        [SetUp]
        public void Setup()
        {
            Visualisation.Initialise();
        }

        [Test]
        public void VisualisationTest()
        {
            SectionTypes[] sectionTypesTest = new SectionTypes[5];
            sectionTypesTest[0] = (SectionTypes)0;
            sectionTypesTest[1] = (SectionTypes)1;
            sectionTypesTest[2] = (SectionTypes)2;
            sectionTypesTest[3] = (SectionTypes)3;
            sectionTypesTest[4] = (SectionTypes)4;

            Track TrackTest = new Track("VisualisationTest", sectionTypesTest);

            Visualisation.DrawTrack(TrackTest);
        }
    }
}