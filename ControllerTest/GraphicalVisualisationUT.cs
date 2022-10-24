using GraphicVisualisation;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Model;
using Controller;

namespace UnitTests
{
    [TestFixture]
    internal class GraphicalVisualisationUT
    {
        //Niet aangeraden om calls van de windows te proberen in tests
        //De compiler vindt het niet leuk als er threading calls wordt gedaan terwijl dat eigenlijk niet mag
        [SetUp]
        public void Setup()
        {

        }

        [TestCase("Empty")]
        [TestCase("Straight")]
        [TestCase("LeftCorner")]
        [TestCase("RightCorner")]
        [TestCase("StartGrid")]
        [TestCase("Finish")]
        [TestCase("Blue")]
        [TestCase("Green")]
        [TestCase("Grey")]
        [TestCase("Red")]
        [TestCase("Yellow")]
        [TestCase("Broken")]
        [TestCase("Pitstop")]
        [TestCase("Tree")]
        public void CachingTest(string bitmap)
        {
            Bitmap BM;
            Bitmap BM2;

            BM = LoadResources.GetBitmap("Empty");
            BM2 = LoadResources.GetBitmap("Empty");
            Assert.That(BM2, Is.EqualTo(BM));

            LoadResources.Clear();
        }

        [Test]
        public void CacheClearTest()
        {
            Bitmap BM;
            Bitmap BM2;

            BM = LoadResources.GetBitmap("Empty");

            LoadResources.Clear();

            BM2 = LoadResources.GetBitmap("Empty");

            Assert.AreNotEqual(BM, BM2);
        }

        [Test]
        public void BitmapSourceTest()
        {
            Bitmap BM;
            BitmapSource BMS;
            BM = LoadResources.GetBitmap("Empty");
            BMS = LoadResources.CreateBitmapSourceFromGdiBitmap(BM);
            Assert.IsNotNull(BMS);
        }

        [Test]
        public void RotateImageTest()
        {
            Bitmap BM = LoadResources.GetBitmap("Empty");
            GraphicalVisualisation.RotateImage(BM, 90);
            Assert.IsNotNull(BM);
        }

        [Test]
        public void CircuitTest()
        {
            //Moeilijk te testen want je moet het wel kunnen zien
            //Testen op errors dus eigenlijk
            Data.Initialise();
            Data.NextRace();
            GraphicalVisualisation.DrawTrack(Data.CurrentRace.Track);
        }

        [Test]
        public void DataContextTest()
        {
            //Test alle datacontext mogelijk
            //Maak race aan
            //Haal data op
            //Kijk of data klopt met gegevens die er zijn.

            Data.Initialise();
            Data.NextRace();
            RaceSimDataContext raceSimDataContext = new();

            Assert.That(Data.CurrentRace.Track.Name, Is.EqualTo(raceSimDataContext.Trackname));
            Assert.IsNotNull(raceSimDataContext.CompetitionStats[0]);
            Assert.IsNotNull(raceSimDataContext.RaceDrivers[0]);
        }
    }
}
