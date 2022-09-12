﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal interface IParticipant
    {
        string Naam { get; set; }
        int Points { get; set; }
        IEquipment Equipment { get; set; }
        TeamColors TeamColor { get; set; }
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
