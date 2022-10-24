using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;
using GraphicVisualisation.Properties;

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
                            Bitmap StraightLineBM;
                            try
                            {
                                StraightLineBM = new((Image)Resources.ResourceManager.GetObject("straight"));
                            } catch (Exception e)
                            {
                                StraightLineBM = new(Straight);
                            } 
                            Bitmaps.Add(bitmapString, StraightLineBM);
                            break;
                        case "LeftCorner":
                        Bitmap LeftCornerBM;
                        try
                        {
                            LeftCornerBM = new((Image)Resources.ResourceManager.GetObject("left"));
                        }
                        catch (Exception e)
                        {
                            LeftCornerBM = new(LeftCorner);
                        }

                        Bitmaps.Add(bitmapString, LeftCornerBM);
                            break;
                        case "RightCorner":
                        Bitmap RightCornerBM;
                        try
                        {
                            RightCornerBM = new((Image)Resources.ResourceManager.GetObject("right"));
                        }
                        catch (Exception e)
                        {
                            RightCornerBM = new(RightCorner);
                        }

                        Bitmaps.Add(bitmapString, RightCornerBM);
                            break;
                        case "StartGrid":
                        Bitmap StartGridBM;
                        try
                        {
                            StartGridBM = new((Image)Resources.ResourceManager.GetObject("start"));
                        }
                        catch (Exception e)
                        {
                            StartGridBM = new(StartGrid);
                        }
                        Bitmaps.Add(bitmapString, StartGridBM);
                            break;
                        case "Finish":
                        Bitmap FinishBM;
                        try
                        {
                            FinishBM = new((Image)Resources.ResourceManager.GetObject("finish"));
                        }
                        catch (Exception e)
                        {
                            FinishBM = new(Finish);
                        }
                        Bitmaps.Add(bitmapString, FinishBM);
                            break;
                        case "Blue":
                        Bitmap BlueBM;
                        try
                        {
                            BlueBM = new((Image)Resources.ResourceManager.GetObject("blue"));
                        }
                        catch (Exception e)
                        {
                            BlueBM = new(Blue);
                        }
                        Bitmaps.Add(bitmapString, BlueBM);
                        break;
                        case "Green":
                        Bitmap GreenBM;
                        try
                        {
                            GreenBM = new((Image)Resources.ResourceManager.GetObject("green"));
                        }
                        catch (Exception e)
                        {
                            GreenBM = new(Green);
                        }
                        Bitmaps.Add(bitmapString, GreenBM);
                        break;
                        case "Grey":
                        Bitmap GreyBM;
                        try
                        {
                            GreyBM = new((Image)Resources.ResourceManager.GetObject("grey"));
                        }
                        catch (Exception e)
                        {
                            GreyBM = new(Grey);
                        }
                        Bitmaps.Add(bitmapString, GreyBM);
                        break;
                        case "Red":
                        Bitmap RedBM;
                        try
                        {
                            RedBM = new((Image)Resources.ResourceManager.GetObject("red"));
                        }
                        catch (Exception e)
                        {
                            RedBM = new(Red);
                        }
                        Bitmaps.Add(bitmapString, RedBM);
                        break;
                        case "Yellow":
                        Bitmap YellowBM;
                        try
                        {
                            YellowBM = new((Image)Resources.ResourceManager.GetObject("yellow"));
                        }
                        catch (Exception e)
                        {
                            YellowBM = new(Yellow);
                        }
                        Bitmaps.Add(bitmapString, YellowBM);
                        break;
                        case "Broken":
                        Bitmap BrokenBM;
                        try
                        {
                            BrokenBM = new((Image)Resources.ResourceManager.GetObject("broken"));
                        }
                        catch (Exception e)
                        {
                            BrokenBM = new(Broken);
                        }
                        Bitmaps.Add(bitmapString, BrokenBM);
                        break;
                        case "Pitstop":
                        Bitmap PitstopBM;
                        try
                        {
                            PitstopBM = new((Image)Resources.ResourceManager.GetObject("pitstop"));
                        }
                        catch (Exception e)
                        {
                            PitstopBM = new(Pitstop);
                        }
                        Bitmaps.Add(bitmapString, PitstopBM);
                        break;
                        case "Tree":
                        Bitmap TreeBM;
                        try
                        {
                            TreeBM = new((Image)Resources.ResourceManager.GetObject("tree"));
                        }
                        catch (Exception e)
                        {
                            TreeBM = new(Tree);
                        }
                        Bitmaps.Add(bitmapString, TreeBM);
                        break;
                    default:
                        Bitmap EmptBitmap;
                            EmptBitmap = EmptyBitmap(2000, 2500);
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
