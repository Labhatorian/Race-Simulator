using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class Driver : IParticipant
    {
        public string Naam { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }

        //public Driver(string naam, int points, IEquipment equipment, TeamColors teamColor)
        //{
        //    Naam = naam;
        //    Points = points;
        //    Equipment = equipment;
        //    TeamColor = teamColor;
        //}
    }
}
