using Model;
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
        private static Dictionary<IParticipant, int> _ParticipantsLaps;
        private Timer timer;

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        //Haal sectiondata op als het bestaan anders maak nieuw
        //Sectiondata bevat gegevens over de deelnemers die nu in de section zitten
        public SectionData GetSectionData(Section section)
        {
            _positions.TryGetValue(section, out var value);

            if (value != null)
            {
                return value;
            }
            else
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
            _ParticipantsLaps = new Dictionary<IParticipant, int>();
            timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;

           //Data.CurrentRace.DriversChanged += OnDriverChanged;

            PlaceParticipants(track, participants);
            Start();
        }

        //Geef de deelnemers een willekeurig aantal kwaliteit en performance
        private void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(1, 10);
                participant.Equipment.Performance = _random.Next(3, 8);
                participant.Equipment.Speed = _random.Next(3, 8);
            }
        }

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

                        if (sectionData.Left == null)
                        {
                            sectionData.Left = Participants[currentAt];
                            currentAt++;

                            //Volgende participant gaat section lijst af
                            break;
                        }
                        else if (sectionData.Right == null)
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

            //Voor elk participant maak entry aan in _ParticipantsLaps
            foreach (IParticipant participant in Participants)
            {
                _ParticipantsLaps.Add(participant, 1);
            }
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {   
           {
                Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

                //Zodat de dictionary tijdens de foreach wordt aangepast
                var newDictionary = _positions.ToDictionary(entry => entry.Key,
                                                   entry => entry.Value);

                foreach (KeyValuePair<Section, SectionData> entry in newDictionary)
                {
                    SectionData SD = entry.Value;
                    Section Section = entry.Key;

                    Section NextSection = Section;

                    //Vind volgende section
                    Boolean Found = false;
                    Boolean AddLap = false;
                    foreach (Section section in Track.Sections)
                    {
                        if (Found)
                        {
                            NextSection = section;
                            break;
                        }
                        if (section == Section)
                        {
                            if (Section.SectionType == SectionTypes.Finish)
                            {
                                NextSection = Track.Sections.First.Value;
                                AddLap = true;
                                break;
                            }
                            else
                            {
                                Found = true;
                            }
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
                        int Speed = SD.Left.Equipment.Speed * SD.Left.Equipment.Performance * _random.Next(1, 3);
                        SD.DistanceLeft += Speed;

                        if (SD.DistanceLeft >= 100)
                        {
                            SD.DistanceLeft = 0;

                            if (SDnext.Left == null)
                            {
                                SDnext.Left = SD.Left;
                                if (AddLap)
                                {
                                    _ParticipantsLaps[SD.Left] += 1;
                                    Console.WriteLine($"{SD.Left.Naam} Lap: {_ParticipantsLaps[SD.Left]}");
                                    if (_ParticipantsLaps[SD.Left] >= 3)
                                    {
                                        RemoveDriverAndCheck(SD.Left, SDnext, SD);
                                    }
                                }
                                SD.Left = null;
                            }
                            else if (SDnext.Right == null)
                            {
                                SDnext.Right = SD.Left;
                                if (AddLap)
                                {
                                    _ParticipantsLaps[SD.Left] += 1;
                                    Console.WriteLine($"{SD.Left.Naam} Lap: {_ParticipantsLaps[SD.Left]}");
                                    if (_ParticipantsLaps[SD.Left] == 3)
                                    {
                                        SD.Left = null;
                                        RemoveDriverAndCheck(SD.Left, SDnext, SD);
                                    }
                                }
                                SD.Left = null;
                            }

                            DriversChanged(this, new DriversChangedEventArgs(Track, NextSection));
                        }
                    }

                    if (SD.Right != null)
                    {
                        int Speed = SD.Right.Equipment.Speed * SD.Right.Equipment.Performance * _random.Next(1, 3); ;
                        SD.DistanceRight += Speed;

                        if (SD.DistanceRight >= 100)
                        {
                            SD.DistanceRight = 0;

                            if (SDnext.Left == null)
                            {
                                SDnext.Left = SD.Right;
                                if (AddLap)
                                {
                                    _ParticipantsLaps[SD.Right] += 1;
                                    Console.WriteLine($"{SD.Right.Naam} Lap: {_ParticipantsLaps[SD.Right]}");
                                    if (_ParticipantsLaps[SD.Right] == 3)
                                    {
                                        RemoveDriverAndCheck(SD.Right, SDnext, SD);
                                    }
                                }
                                SD.Right = null;
                            }
                            else if (SDnext.Right == null)
                            {
                                SDnext.Right = SD.Right;
                                if (AddLap)
                                {
                                    _ParticipantsLaps[SD.Right] += 1;
                                    Console.WriteLine($"{SD.Right.Naam} Lap: {_ParticipantsLaps[SD.Right]}");
                                    if (_ParticipantsLaps[SD.Right] == 3)
                                    {
                                        RemoveDriverAndCheck(SD.Right, SDnext, SD);
                                    }
                                }
                                SD.Right = null;
                            }

                            DriversChanged(this, new DriversChangedEventArgs(Track, NextSection));
                        }
                    }
                }
            }
            timer.Start();
        }

        private void Start()
        {
            RandomizeEquipment();
            timer.AutoReset = false;
            timer.Start();
        }

        private void RemoveDriverAndCheck(IParticipant driver, SectionData SD, SectionData SDprev)
        {
            if (SD.Left == driver)
            {
                SD.Left = null;
            }
            else if (SD.Right == driver)
            {
                SD.Right = null;
            }
            if(SDprev.Left == driver)
            {
                SDprev.Left = null;
            }
            else if (SDprev.Right == driver)
            {
                SDprev.Right = null;
            }


            //Check of er ergens een driver is
            Boolean DriverFound = false;

            //Zodat de dictionary tijdens de foreach wordt aangepast
            var newDictionary = _positions.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

            foreach (KeyValuePair<Section, SectionData> entry in newDictionary)
            {
                SectionData SeD = entry.Value;
                if (SeD.Left != null | SeD.Right != null)
                {
                    DriverFound = true;
                    break;
                }
            }

            if (!DriverFound)
            {
                timer.Stop();
                timer.Enabled = false;
                timer.Dispose();
                foreach (Delegate d in DriversChanged.GetInvocationList())
                {
                    DriversChanged -= (EventHandler<DriversChangedEventArgs>)d;
                }
                Data.NextRace();
            } 
        }
    }
}
