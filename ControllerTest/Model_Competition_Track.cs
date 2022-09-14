using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest
{
    [TestFixture]
    internal class Model_Competition_Track
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

            Track[] TracksQueued = new Track[3];
            TracksQueued[0] = _competition.NextTrack();
            Assert.AreEqual(TracksQueued[0], TrackTest);
            TracksQueued[1] = _competition.NextTrack();
            Assert.AreEqual(TracksQueued[1], TrackTestTwo);
            TracksQueued[2] = _competition.NextTrack();
            Assert.IsNull(TracksQueued[2]);
        }
        #endregion
    }
}
