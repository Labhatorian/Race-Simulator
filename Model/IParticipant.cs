using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal interface IParticipant
    {
        public string Naam { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
    }

    enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }
}
