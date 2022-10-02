using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest
{
    [TestFixture]
    internal class Model
    {
        private Competition _competition;

        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
            _competition.Tracks = new Queue<Track>();

        }

        #region Tests
        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            SectionTypes[] sectionTypesSilverstone = new SectionTypes[0];

            Track TrackTest = new Track("Silverstone", sectionTypesSilverstone);

            _competition.Tracks.Enqueue(TrackTest);

            //Nu testen
            Track result = _competition.NextTrack();
            Assert.AreEqual(TrackTest, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] sectionTypesSilverstone = new SectionTypes[0];

            Track TrackTest = new Track("Silverstone", sectionTypesSilverstone);

            _competition.Tracks.Enqueue(TrackTest);

            //Nu testen
            Track result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            SectionTypes[] sectionTypesSilverstone = new SectionTypes[0];
            SectionTypes[] sectionTypesZandvoort = new SectionTypes[0];

            Track TrackTest = new Track("Silverstone", sectionTypesSilverstone);
            Track TrackTestTwo = new Track("Zandvoort", sectionTypesZandvoort);

            _competition.Tracks.Enqueue(TrackTest);
            _competition.Tracks.Enqueue(TrackTestTwo);

            //Check of de gequeed tracks in goede volgorde zijn
            Track[] TracksQueued = new Track[3];
            TracksQueued[0] = _competition.NextTrack();
            Assert.AreEqual(TracksQueued[0], TrackTest);
            TracksQueued[1] = _competition.NextTrack();
            Assert.AreEqual(TracksQueued[1], TrackTestTwo);
            TracksQueued[2] = _competition.NextTrack();
            Assert.IsNull(TracksQueued[2]);
        }

        [Test]
        public void ModelSetTester()
        {
            SectionData sectionDataTest = new SectionData();
            sectionDataTest.DistanceLeft = 30;
            sectionDataTest.DistanceRight = 20;

            Assert.AreEqual(sectionDataTest.DistanceLeft, 30);
            Assert.AreEqual(sectionDataTest.DistanceRight, 20);

            Driver driverTest = new Driver();
            driverTest.Naam = "Max?";
            Assert.AreEqual(driverTest.Naam, "Max?");

            Car carTest = new Car();
            carTest.Speed = 10;
            carTest.Performance = 30;
            carTest.Quality = 20;
            carTest.IsBroken = true;

            //Check of alle gegevens kloppen die zijn geset
            Assert.AreEqual(carTest.Speed, 10);
            Assert.AreEqual(carTest.Performance, 30);
            Assert.AreEqual(carTest.Quality, 20);
            Assert.AreEqual(carTest.IsBroken, true);

        }
        #endregion
    }
}
