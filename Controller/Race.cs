using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        private SectionData GetSectionData(Section section)
        {
            _positions.TryGetValue(section, out var value);

            if(value != null)
            {
                return value;
            } else
            {
                SectionData sectionData = new SectionData();
                _positions.Add(section, sectionData);
                return sectionData;
            }
        }

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
        }

        private void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(99);
                participant.Equipment.Performance = _random.Next(99);
            }
        }
    }
}
