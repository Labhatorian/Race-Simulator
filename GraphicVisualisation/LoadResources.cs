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

namespace GraphicVisualisation
{
    public static class LoadResources
    {
        private static Dictionary<string, Bitmap> Bitmaps = new Dictionary<string, Bitmap>();
        
        public static Bitmap GetBitmap(string String)
        {
            if (Bitmaps[String] == null)
            {
                Bitmaps.Add(String, new Bitmap(String));
            }
            else if (String.Equals("Empty"))
            {
                Bitmap EmptBitmap = EmptyBitmap(500, 500);
                Bitmaps.Add(String, EmptBitmap);
            }

            Bitmaps[String].Clone();
            return Bitmaps[String];
        }

        public static void Clear()
        {
            Bitmaps = new Dictionary<string, Bitmap>();
        }

        public static Bitmap EmptyBitmap(int x, int y)
        {
            Bitmap BM = new Bitmap(x, y);
            Graphics image = Graphics.FromImage(BM);
            image.Clear(Color.Green);
            return new Bitmap(x , y, image); ;
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
    }
}
