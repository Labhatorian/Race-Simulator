﻿using GraphicVisualisation;
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

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CachingTest()
        {
            Bitmap BM;
            Bitmap BM2;

            BM = LoadResources.GetBitmap("Empty");
            BM2 = LoadResources.GetBitmap("Empty");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Straight");
            BM2 = LoadResources.GetBitmap("Straight");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("LeftCorner");
            BM2 = LoadResources.GetBitmap("LeftCorner");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("RightCorner");
            BM2 = LoadResources.GetBitmap("RightCorner");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("StartGrid");
            BM2 = LoadResources.GetBitmap("StartGrid");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Finish");
            BM2 = LoadResources.GetBitmap("Finish");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Blue");
            BM2 = LoadResources.GetBitmap("Blue");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Green");
            BM2 = LoadResources.GetBitmap("Green");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Grey");
            BM2 = LoadResources.GetBitmap("Grey");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Red");
            BM2 = LoadResources.GetBitmap("Red");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Yellow");
            BM2 = LoadResources.GetBitmap("Yellow");
            Assert.That(BM2, Is.EqualTo(BM));

            BM = LoadResources.GetBitmap("Broken");
            BM2 = LoadResources.GetBitmap("Broken");
            Assert.That(BM2, Is.EqualTo(BM));

            LoadResources.Clear();
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
            Data.Initialise();
            Data.NextRace();
            GraphicalVisualisation.DrawTrack(Data.CurrentRace, Data.CurrentRace.Track, null);
        }

        [Test]
        public void DataContextTest()
        {
        }
    }
}