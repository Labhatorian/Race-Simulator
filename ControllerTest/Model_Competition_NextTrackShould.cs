using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest
{
    [TestFixture]
    internal class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
            _competition.Tracks = new Queue<Track>();

        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            SectionTypes[] sectionTypesSilverstone = new SectionTypes[3];
            sectionTypesSilverstone[0] = (SectionTypes)1;
            sectionTypesSilverstone[1] = (SectionTypes)3;
            sectionTypesSilverstone[2] = (SectionTypes)4;

            Track TrackTest = new Track("Silverstone", sectionTypesSilverstone);

            _competition.Tracks.Enqueue(TrackTest);

            //Nu testen
            Track result = _competition.NextTrack();
            Assert.AreEqual(TrackTest, result);
        }
    }
}
