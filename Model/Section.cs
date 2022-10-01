using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Section
    {
        public SectionTypes SectionType { get; set; }
    }

    /// <summary>
    /// Elk mogelijk blok van circuit
    /// </summary>
    public enum SectionTypes
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }

    /// <summary>
    /// Elk mogelijk richting van hierboven (blok van circuit)
    /// </summary>
    public enum Directions
    {
        North,
        East,
        South,
        West,
    }
}
