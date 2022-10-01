using Model;
using Section = Model.Section;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        //TODO is elk parameter voor elk functie wel goede naam?

        //Parameters voor de race
        public Track Track { get; set; }
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Timer timer;

        //Houdt bij belangrijke dingen voor de race
        public static Dictionary<Section, SectionData> _positions;
        private static Dictionary<IParticipant, int> _participantslaps;
       private static Dictionary<IParticipant, Boolean> _participantsfinished;

        //Eventhandlers voor verplaatste driver en gefinishte driver
        public event EventHandler<DriversChangedEventArgs>? DriversChanged;
        public event EventHandler<EventArgs> DriversFinished;

        /// <summary>
        /// Vind sectiondata van section. Als die niet bestaat, maak een nieuwe aan voor die section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Maak race aan. Haal gegevens op uit data van competitie en zet timer en event klaar. Begin dan de race
        /// </summary>
        /// <param name="track"></param>
        /// <param name="participants"></param>
        public Race(Track track, List<IParticipant> participants)
        {
            //Maak de race
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            _participantslaps = new Dictionary<IParticipant, int>();
            PlaceParticipants(track, participants);

            //Timer en eventhandler klaar en we starten
            timer = new Timer(500);
            timer.Elapsed += OnTimedEvent;
            Start();
        }

        /// <summary>
        /// Geef de deelnemers een willekeurig waarde voor kwaliteit performance en speed
        /// </summary>
        private void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(1, 100);
                participant.Equipment.Performance = _random.Next(3, 8);
                participant.Equipment.Speed = _random.Next(3, 8);
            }
        }


        /// <summary>
        /// Zet participants op de starting grid. De eerste is LEFT en de tweede is RIGHT enzo.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="Participants"></param>
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

            //Voor elk participant maak entry aan in _participantslaps
            foreach (IParticipant participant in Participants)
            {
                _participantslaps.Add(participant, 1);
            }
        }

        /// <summary>
        /// Brein van de race. Code bevat beweging voor de drivers en veel checks(einde race) en berekeningen
        /// TODO Verander summary
        /// TODO Verbeter zodat left en right een functie is en niet zo lange code met veel hetzelfde
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            {
                //Zodat de dictionary tijdens de foreach niet wordt aangepast
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

                    //Stuur elk driver door naar de functie DriverElapsed. De code gaat daar verder.
                    //TODO Maak parameters na true en false voor hele klasse?
                    DriverElapsed(SD.Left, false, SD, NextSection, SDnext, AddLap);
                    DriverElapsed(SD.Right, true, SD, NextSection, SDnext, AddLap);

                }
            }
            if (timer != null)
            {
                timer.Start();
            }
        }

        /// <summary>
        /// WORK IN PROGRESS
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="LeftOrRight"></param>
        /// <param name="SD"></param>
        /// <param name="NextSection"></param>
        /// <param name="SDnext"></param>
        /// <param name="AddLap"></param>
        private void DriverElapsed(IParticipant Driver, Boolean LeftOrRight, SectionData SD, Section NextSection, SectionData SDnext, Boolean AddLap)
        {
            if (Driver != null)
            {
                int Speed;
                double PossibleBroken;
                if (!LeftOrRight)
                {
                    Speed = SD.Left.Equipment.Speed * SD.Left.Equipment.Performance;
                    PossibleBroken = ((double)SD.Left.Equipment.Quality / 100.0);
                } else
                {
                    Speed = SD.Right.Equipment.Speed * SD.Right.Equipment.Performance
                    PossibleBroken = ((double)SD.Right.Equipment.Quality / 100.0);
                }

                Speed *= _random.Next(1, 3);
                PossibleBroken *= (double)_random.Next(1, 5);

                if (Math.Ceiling(PossibleBroken) >= 7)
                {
                    if (!Driver.Equipment.IsBroken)
                    {
                        Driver.Equipment.IsBroken = true;
                    }
                    else
                    {
                        Driver.Equipment.IsBroken = false;
                        Driver.Equipment.Quality -= 1;
                    }
                    DriversChanged(this, new DriversChangedEventArgs(Track, NextSection));
                }

                if (!Driver.Equipment.IsBroken)
                {
                    if (!LeftOrRight)
                    {
                        SD.DistanceLeft += Speed;
                    }
                    else
                    {
                        SD.DistanceRight += Speed;
                    }
                }

                Boolean Moved = false;
                if (!LeftOrRight)
                {
                    if (SD.DistanceLeft >= 100)
                    {
                        if (!Driver.Equipment.IsBroken)
                        {
                            SD.DistanceLeft = 0;
                            Moved = true;
                        }
                    }
                }
                else
                {
                    if (SD.DistanceRight >= 100)
                    {
                        if (!Driver.Equipment.IsBroken)
                        {
                            SD.DistanceRight = 0;
                            Moved = true;
                        }
                    }
                }


                if (SDnext.Left == null & Moved)
                {
                    SDnext.Left = Driver;
                    if (AddLap)
                    {
                        AddLapToDriver(Driver, SD, SDnext);
                    }
                }
                else if (SDnext.Right == null & Moved)
                {
                    SDnext.Right = Driver;
                    if (AddLap)
                    {
                        AddLapToDriver(Driver, SD, SDnext);
                    }
                }

                if (Moved)
                {
                    if (!LeftOrRight)
                    {
                        SD.Left = null;
                    }
                    else
                    {
                        SD.Right = null;
                    }
                    if (timer != null)
                    {
                        DriversChanged(this, new DriversChangedEventArgs(Track, NextSection));
                    }
                }
            }
        }

        private void AddLapToDriver(IParticipant Driver, SectionData SD, SectionData SDnext)
        {
            _participantslaps[Driver] += 1;
            Console.WriteLine($"{Driver.Naam} Lap: {_participantslaps[Driver]}");
            Thread.Sleep(500);
            if (_participantslaps[Driver] >= 3)
            {
                RemoveDriverAndCheck(Driver, SDnext, SD);
            }
        }

        /// <summary>
        /// Start de race. Willekeurig equipment Timer autorestart uit zodat hij niet de thread volspamt met queries van eventhandler
        /// </summary>
        private void Start()
        {
            RandomizeEquipment();
            timer.AutoReset = false;
            timer.Start();
        }

        /// <summary>
        /// Verwijder driver bij einde van race. Check of er nog iemand op het circuit zit.
        /// TODO verbeter code
        /// TODO Kan het checken van driver sneller? Via parameter
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="SD"></param>
        /// <param name="SDprev"></param>
        private void RemoveDriverAndCheck(IParticipant driver, SectionData SD, SectionData SDprev)
        {
            //Verwijder driver. Zorgt ervoor dat driver niet naar volgende section kan glippen
            if (SD.Left == driver)
            {
                SD.Left = null;
            }
            else if (SD.Right == driver)
            {
                SD.Right = null;
            }

            if (SDprev.Left == driver)
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
                timer = null;
                foreach (Delegate d in DriversChanged.GetInvocationList())
                {
                    DriversChanged -= (EventHandler<DriversChangedEventArgs>)d;
                }
                DriversFinished(this, new EventArgs());
                foreach (Delegate d in DriversFinished.GetInvocationList())
                {
                    DriversFinished -= (EventHandler<EventArgs>)d;
                }
            }
        }
    }
}
