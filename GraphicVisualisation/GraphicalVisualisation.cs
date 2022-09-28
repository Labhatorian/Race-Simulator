using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Controller;
using Model;
using Track = Model.Track;

namespace GraphicVisualisation
{
    public class GraphicalVisualisation
    {

        public Bitmap DrawTrack(Track Track, Bitmap Bitmap)
        {
            Bitmap BM = LoadResources.GetBitmap("Empty");
            return BM;
        }
        
    }
}
