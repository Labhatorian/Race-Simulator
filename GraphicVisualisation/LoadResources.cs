using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace GraphicVisualisation
{
    public static class LoadResources
    {
        private static Dictionary<string, Bitmap> Bitmaps = new Dictionary<string, Bitmap>();
        
        /// <summary>
        /// Haalt Bitmap op uit de cache. Als die niet bestaat, maak een nieuwe en voeg toe aan de cache
        /// </summary>
        /// <param name="bitmapString"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetBitmap(string bitmapString)
        {
                if (!Bitmaps.ContainsKey(bitmapString))
                {
                    switch (bitmapString)
                    {
                        case "Straight":
                            Bitmap StraightLineBM = new(Straight);
                            Bitmaps.Add(bitmapString, StraightLineBM);
                            break;
                        case "LeftCorner":
                            Bitmap LeftCornerBM = new(LeftCorner);
                        Bitmaps.Add(bitmapString, LeftCornerBM);
                            break;
                        case "RightCorner":
                            Bitmap RightCornerBM = new(RightCorner);
                        Bitmaps.Add(bitmapString, RightCornerBM);
                            break;
                        case "StartGrid":
                            Bitmap StartGridBM = new(StartGrid);
                        Bitmaps.Add(bitmapString, StartGridBM);
                            break;
                        case "Finish":
                            Bitmap FinishBM = new(Finish);
                        Bitmaps.Add(bitmapString, FinishBM);
                            break;
                        case "Blue":
                        Bitmap BlueBM = new(Blue);
                        Bitmaps.Add(bitmapString, BlueBM);
                        break;
                        case "Green":
                        Bitmap GreenBM = new(Green);
                        Bitmaps.Add(bitmapString, GreenBM);
                        break;
                        case "Grey":
                        Bitmap GreyBM = new(Grey);
                        Bitmaps.Add(bitmapString, GreyBM);
                        break;
                        case "Red":
                        Bitmap RedBM = new(Red);
                        Bitmaps.Add(bitmapString, RedBM);
                        break;
                        case "Yellow":
                        Bitmap YellowBM = new(Yellow);
                        Bitmaps.Add(bitmapString, YellowBM);
                        break;
                        case "Broken":
                        Bitmap BrokenBM = new(Broken);
                        Bitmaps.Add(bitmapString, BrokenBM);
                        break;
                        case "Pitstop":
                        Bitmap PitstopBM = new(Pitstop);
                        Bitmaps.Add(bitmapString, PitstopBM);
                        break;
                        case "Tree":
                        Bitmap TreeBM = new(Tree);
                        Bitmaps.Add(bitmapString, TreeBM);
                        break;
                    default:
                            Bitmap EmptBitmap = EmptyBitmap(2000, 2500);
                            Bitmaps.Add("Empty", EmptBitmap);
                            break;
                    }
            }

            Bitmaps[bitmapString].Clone();
            return Bitmaps[bitmapString];
        }

        /// <summary>
        /// Leeg de cache van Bitmaps
        /// </summary>
        public static void Clear()
        {
            Bitmaps = new Dictionary<string, Bitmap>();
        }

        /// <summary>
        /// Maakt een Bitmap zonder het circuit zelf. Voegt willekeurig 10 bomen toe
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap EmptyBitmap(int x, int y)
        {
            Bitmap BM = new Bitmap(x, y);
            SolidBrush SB = new SolidBrush(Color.Green);
            Bitmap Tree = GetBitmap("Tree");
            Random random = new Random();

            using (Graphics graph = Graphics.FromImage(BM))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 2000, 2500);
                graph.FillRectangle(SB, ImageSize);

                for(int i = 0; i <= 10; i++)
                {
                    graph.DrawImage(Tree, random.Next(0, x)-100, random.Next(0, y)-300, 500, 500);
                }
            }
            return BM;
        }

        /// <summary>
        /// Methode die de Bitmap image omzet naar een BitmapSource voor de MainImage in MainWindow
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        #region GRAPHICS
        //Sections
        const string Straight    = "..\\..\\..\\Content\\straight.png";
        const string LeftCorner  = "..\\..\\..\\Content\\left.png";
        const string RightCorner = "..\\..\\..\\Content\\right.png";
        const string StartGrid   = "..\\..\\..\\Content\\start.png";
        const string Finish      = "..\\..\\..\\Content\\finish.png";

        const string Tree = "..\\..\\..\\Content\\tree.png";

        //Cars
        const string Blue = "..\\..\\..\\Content\\blue.png";
        const string Green = "..\\..\\..\\Content\\green.png";
        const string Grey = "..\\..\\..\\Content\\grey.png";
        const string Red = "..\\..\\..\\Content\\red.png";
        const string Yellow = "..\\..\\..\\Content\\yellow.png";

        //Broken
        const string Broken = "..\\..\\..\\Content\\broken.png";
        const string Pitstop = "..\\..\\..\\Content\\pitstop.png";
        #endregion
    }
}
