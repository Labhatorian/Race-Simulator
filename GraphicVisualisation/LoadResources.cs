using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Controller;
using Model;
using System.Diagnostics;

namespace GraphicVisualisation
{
    public static class LoadResources
    {
        private static Dictionary<string, Bitmap> Bitmaps = new Dictionary<string, Bitmap>();
        
        /// <summary>
        /// Haalt Bitmap op uit de cache. Als die niet bestaat, maak een nieuwe en voeg toe aan de cache
        /// </summary>
        /// <param name="Stringo"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetBitmap(string Stringo)
        {
                if (!Bitmaps.ContainsKey(Stringo))
                {
                    switch (Stringo)
                    {
                        case "Straight":
                            Bitmap StraightLineBM = new(Straight);
                            Bitmaps.Add(Stringo, StraightLineBM);
                            break;
                        case "LeftCorner":
                            Bitmap LeftCornerBM = new(LeftCorner);
                        Bitmaps.Add(Stringo, LeftCornerBM);
                            break;
                        case "RightCorner":
                            Bitmap RightCornerBM = new(RightCorner);
                        Bitmaps.Add(Stringo, RightCornerBM);
                            break;
                        case "StartGrid":
                            Bitmap StartGridBM = new(StartGrid);
                        Bitmaps.Add(Stringo, StartGridBM);
                            break;
                        case "Finish":
                            Bitmap FinishBM = new(Finish);
                        Bitmaps.Add(Stringo, FinishBM);
                            break;
                        case "Blue":
                        Bitmap BlueBM = new(Blue);
                        Bitmaps.Add(Stringo, BlueBM);
                        break;
                        case "Green":
                        Bitmap GreenBM = new(Green);
                        Bitmaps.Add(Stringo, GreenBM);
                        break;
                        case "Grey":
                        Bitmap GreyBM = new(Grey);
                        Bitmaps.Add(Stringo, GreyBM);
                        break;
                        case "Red":
                        Bitmap RedBM = new(Red);
                        Bitmaps.Add(Stringo, RedBM);
                        break;
                        case "Yellow":
                        Bitmap YellowBM = new(Yellow);
                        Bitmaps.Add(Stringo, YellowBM);
                        break;
                        case "Broken":
                        Bitmap BrokenBM = new(Broken);
                        Bitmaps.Add(Stringo, BrokenBM);
                        break;
                    default:
                            Bitmap EmptBitmap = EmptyBitmap(2000, 2500);
                            Bitmaps.Add("Empty", EmptBitmap);
                            break;
                    }
                }

            Bitmaps[Stringo].Clone();
            return Bitmaps[Stringo];
        }

        /// <summary>
        /// Leeg de cache van Bitmaps
        /// </summary>
        public static void Clear()
        {
            Bitmaps = new Dictionary<string, Bitmap>();
        }

        /// <summary>
        /// Maakt een Bitmap zonder het circuit zelf
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap EmptyBitmap(int x, int y)
        {
            //TODO voeg bomen toe
            Bitmap BM = new Bitmap(x, y);
            SolidBrush SB = new SolidBrush(Color.Green);
            using (Graphics graph = Graphics.FromImage(BM))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 2000, 2500);
                graph.FillRectangle(SB, ImageSize);
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
        //TODO Gebruik relative paths
        //Sections
        const string Straight    = "..\\..\\..\\Graphics\\straight.png";
        const string LeftCorner  = "..\\..\\..\\Graphics\\left.png";
        const string RightCorner = "..\\..\\..\\Graphics\\right.png";
        const string StartGrid   = "..\\..\\..\\Graphics\\start.png";
        const string Finish      = "..\\..\\..\\Graphics\\finish.png";

        //Cars
        const string Blue = "..\\..\\..\\Graphics\\blue.png";
        const string Green = "..\\..\\..\\Graphics\\green.png";
        const string Grey = "..\\..\\..\\Graphics\\grey.png";
        const string Red = "..\\..\\..\\Graphics\\red.png";
        const string Yellow = "..\\..\\..\\Graphics\\yellow.png";

        //Broken
        const string Broken = "..\\..\\..\\Graphics\\broken.png";
        #endregion
    }
}
