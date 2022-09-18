using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        //Haal volgende circuit op
        public Track? NextTrack()
        {
            if (Tracks != null)
            {
                if (Tracks.Count > 0)
                {
                    Track Track = Tracks.Peek();
                    Tracks.Dequeue();
                    return Track;
                }
            }
            return null;
        }
}
}
