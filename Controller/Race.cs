using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        public static Dictionary<Section, SectionData> _positions;
        private Timer timer;
        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        
        //Haal sectiondata op als het bestaan anders maak nieuw
        //Sectiondata bevat gegevens over de deelnemers die nu in de section zitten
        public SectionData GetSectionData(Section section)
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

        //Maak race aan

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
            
            PlaceParticipants(track, participants);
            Start();
        }

        //Geef de deelnemers een willekeurig aantal kwaliteit en performance
        private void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(1, 10);
                participant.Equipment.Performance = _random.Next(1, 5);
                participant.Equipment.Speed = _random.Next(1, 5);
            }
        }

        //Plaats deelnemers in de startopstelling
        private void PlaceParticipants(Track track, List<IParticipant> Participants)
        {
            //Telt bij welk deelnemer wij zijn
            int currentAt = 0;

            while (currentAt < Participants.Count)
            {
                foreach (Section section in track.Sections)
                {
                    if (section.SectionType == SectionTypes.StartGrid)
                    {
                        //Ga er van uit dat het logisch is ingedeeld dus genoeg starting grids voor de circuit
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

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

            foreach (KeyValuePair<Section, SectionData> entry in _positions)
            {
                SectionData SD = entry.Value;
                Section Section = entry.Key;

                Section NextSection = Section;

                //Vind volgende section
                Boolean Found = false;
                foreach (Section section in Track.Sections)
                {
                    if (Found)
                    {
                        NextSection = section;
                        break;
                    }
                    if (section == Section)
                    {
                        Found = true;
                    }
                    else
                    {
                        //Blijf dan zitten?
                    }
                }
                SectionData SDnext = GetSectionData(NextSection);

                //Check of er een driver op de section zit
                if (SD.Left != null)
                {
                    int Speed = SD.Left.Equipment.Speed * SD.Left.Equipment.Performance * _random.Next(1, 2); ;
                    SD.DistanceLeft += Speed;

                    if (SD.DistanceLeft >= 100)
                    {
                        SD.DistanceLeft = 0;

                        if (SDnext.Left == null)
                        {
                            SDnext.Left = SD.Left;
                            SD.Left = null;
                        }
                        else if (SDnext.Right == null)
                        {
                            SDnext.Right = SD.Left;
                            SD.Left = null;
                        }

                        //TODO Alleen driver zelf printen
                        DriversChanged(this, new DriversChangedEventArgs(Track, NextSection));
                    }
                }

                if (SD.Right != null)
                {
                    int Speed = SD.Right.Equipment.Speed * SD.Right.Equipment.Performance * _random.Next(1, 2); ;
                    SD.DistanceRight += Speed;

                    if (SD.DistanceRight >= 100)
                    {
                        SD.DistanceRight = 0;

                        if (SDnext.Left == null)
                        {
                            SDnext.Left = SD.Right;
                            SD.Right = null;
                        }
                        else if (SDnext.Right == null)
                        {
                            SDnext.Right = SD.Right;
                            SD.Right = null;
                        }

                        //TODO Alleen driver zelf printen
                        DriversChanged(this, new DriversChangedEventArgs(Track, NextSection));
                    }
                }
            }
        }

        private void Start()
        {
            RandomizeEquipment();
            timer.Enabled = true;
        }
    }
}
