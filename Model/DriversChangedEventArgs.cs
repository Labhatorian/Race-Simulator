using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DriversChangedEventArgs : EventArgs
    {

        /// <summary>
        /// Arugmenten voor de custom events en eventhandlers
        /// </summary>
        /// <param name="track"></param>
        /// <param name="section"></param>
        public DriversChangedEventArgs(Track track, Section section)
        {
            Track = track;
            Section = section;  
        }

        public Track Track { get; set; }
        public Section Section { get; set; }
    }
}
