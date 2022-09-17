using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace ControllerTest
{
    internal class Controller_Data
    {

        [SetUp]
        public void Setup()
        {
            Data.Initialise();
        }

        [Test]
        public void NextRace()
        {
            Data.NextRace();
        }
    }
}
