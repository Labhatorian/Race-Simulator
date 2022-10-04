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

        public static void Clear()
        {
            Bitmaps = new Dictionary<string, Bitmap>();
        }

        public static Bitmap EmptyBitmap(int x, int y)
        {
            Bitmap BM = new Bitmap(x, y);
            SolidBrush SB = new SolidBrush(Color.Green);
            using (Graphics graph = Graphics.FromImage(BM))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 2000, 2500);
                graph.FillRectangle(SB, ImageSize);
            }

            return BM;
        }
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
        const string Straight    = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\straight.png";
        const string LeftCorner  = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\left.png";
        const string RightCorner = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\right.png";
        const string StartGrid   = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\start.png";
        const string Finish      = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\finish.png";

        //Cars
        const string Blue = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\blue.png";
        const string Green = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\green.png";
        const string Grey = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\grey.png";
        const string Red = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\red.png";
        const string Yellow = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\yellow.png";


        const string Broken = "C:\\Users\\Harris\\source\\repos\\Race Simulator\\GraphicVisualisation\\Graphics\\broken.png";
        #endregion
    }
}
