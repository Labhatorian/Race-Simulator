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
            
            PlaceParticipants(track, participants);
        }

        private void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(99);
                participant.Equipment.Performance = _random.Next(99);
            }
        }

        private void PlaceParticipants(Track track, List<IParticipant> Participants)
        {
            int currentAt = 0;

            while (currentAt < Participants.Count)
            {
                foreach (Section section in track.Sections)
                {
                    if (section.SectionType == SectionTypes.StartGrid)
                    {
                        //Ga er van uit dat het logisch is ingedeeld dus achter elkaar starting grids voor de circuit
                        SectionData sectionData = GetSectionData(section);
                        
                        if(sectionData.Left == null)
                        {
                            sectionData.Left = Participants[currentAt];
                            currentAt++;

                            //Volgende participant gaat section lijst af
                            break;
                        } else if(sectionData.Right == null)
                        {
                            sectionData.Right = Participants[currentAt];
                            currentAt++;

                            //Volgende participant gaat section lijst af
                            break;
                        }
                        //Als section vol is, gaat hij naar de volgende
                    }
                }
                //Ga er van uit dat er genoeg plek is
            }
         }
    }
}
