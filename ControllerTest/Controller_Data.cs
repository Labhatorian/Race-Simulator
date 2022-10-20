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
        public void DataAddParticipantTest()
        {
            IParticipant participant = Data.Competition.Participants[0];
            Assert.That("Max Verstappen", Is.EqualTo(participant.Naam));
        }

        [Test]
        public void DataAddTrackTest()
        {
            Track track = Data.Competition.Tracks.Peek();
            Assert.That("Zandvoort", Is.EqualTo(track.Name));
        }

        [Test]
        public void NextRace()
        {
            Track zandvoorttrack = Data.Competition.Tracks.Peek();
            Data.NextRace();
            Assert.That(zandvoorttrack, Is.EqualTo(Data.CurrentRace.Track));
        }

        [Test]
        public void CompetitionFinished()
        {
            Data.NextRace();
            Data.NextRace();
            Data.NextRace();
            Assert.IsNull(Data.CurrentRace);
        }   
    }
}
