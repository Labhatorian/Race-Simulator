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

    public enum SectionTypes
    {
        Straight,
        HorizStraight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }

    public enum Directions
    {
        North,
        East,
        South,
        West,
    }
}
