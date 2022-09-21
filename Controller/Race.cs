using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            timer = new Timer(500);
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
                participant.Equipment.Performance = _random.Next(1, 10);
                participant.Equipment.Speed = _random.Next(1, 10);
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
            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

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
                    if(section == Section)
                    {
                        Found = true;
                    } else
                    {
                        //Blijf dan zitten?
                    }
                }


                if (SD.Left != null)
                {
                    int Speed = SD.Left.Equipment.Speed * SD.Left.Equipment.Performance;
                    SD.DistanceLeft += Speed;

                    if(SD.DistanceLeft >= 100)
                    {
                        SD.DistanceLeft = 0;
                        ToNextSection(NextSection, SD.Left);
                    }
                }

                if (SD.Right != null)
                {
                    int Speed = SD.Right.Equipment.Speed * SD.Right.Equipment.Performance;
                    SD.DistanceRight += Speed;

                    if (SD.DistanceRight >= 100)
                    {
                        SD.DistanceRight = 0;
                        ToNextSection(NextSection, SD.Right);
                    }
                }
            }
        }
        
        //Dit moet een event invoken
        private void ToNextSection(Section section, IParticipant driver)
        {
            SectionData SD = GetSectionData(section);
            if(SD.Right == null)
            {
                SD.Right = driver;
            } else if(SD.Left == null)
            {
                SD.Left = SD.Right;
                SD.Right = driver;
            }
            DriversChanged(this, new DriversChangedEventArgs(Track));
        }

            private void Start()
        {
            RandomizeEquipment();
            timer.Enabled = true;
        }
    }
}
