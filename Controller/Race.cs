using Model;
using Section = Model.Section;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        //Parameters voor de race
        public Track Track { get; set; }
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Timer timer;
        private int FinishCounter;

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
            _participantsfinished = new Dictionary<IParticipant, Boolean>();
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
        /// <param name="participants"></param>
        private void PlaceParticipants(Track track, List<IParticipant> participants)
        {
            //Telt bij welk deelnemer wij zijn
            int currentAt = 0;

            while (currentAt < participants.Count)
            {
                foreach (Section section in track.Sections)
                {
                    if (section.SectionType == SectionTypes.StartGrid)
                    {
                        //Ga er van uit dat het logisch is ingedeeld dus genoeg starting grids voor de circuit
                        SectionData sectionData = GetSectionData(section);

                        if (sectionData.Left == null)
                        {
                            sectionData.Left = participants[currentAt];
                            currentAt++;

                            //Volgende participant gaat section lijst af
                            break;
                        }
                        else if (sectionData.Right == null)
                        {
                            sectionData.Right = participants[currentAt];
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
            //En voor finished
            foreach (IParticipant participant in participants)
            {
                _participantslaps.Add(participant, 1);
                _participantsfinished.Add(participant, false);
            }
        }

        /// <summary>
        /// Brein van de race. Code bevat beweging voor de drivers en veel checks(einde race) en berekeningen
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            {
                //Zodat de dictionary tijdens de foreach niet wordt aangepast
                var newDictionary = _positions.ToDictionary(entry => entry.Key,
                                                   entry => entry.Value);

                //Zoek section op
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
                        //Gevonden? Ga dan nog 1 keer door voor de volgende
                        if (Found)
                        {
                            NextSection = section;
                            break;
                        }

                        if (section == Section)
                        {
                            //Als de volgende finish is, voeg dan 1 lap toe
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
                    DriverElapsed(SD.Left, false, Section, SD, NextSection, SDnext, AddLap);
                    DriverElapsed(SD.Right, true, Section, SD, NextSection, SDnext, AddLap);

                }
            }
            //Start timer weer op. Null check om exception te vermijden bij finish
            if (timer != null)
            {
                timer.Start();
            }
        }

        /// <summary>
        /// Meerdere checks voor de drivers. Movement, breaking en laps finishen.
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="LeftOrRight"></param>
        /// <param name="SD"></param>
        /// <param name="NextSection"></param>
        /// <param name="SDnext"></param>
        /// <param name="AddLap"></param>
        private void DriverElapsed(IParticipant Driver, Boolean LeftOrRight, Section section, SectionData SD, Section NextSection, SectionData SDnext, Boolean AddLap)
        {
            if (Driver != null)
            {
                //Pak waardes voor snelheid en kapot gaan
                int Speed;
                double PossibleBroken;
                if (!LeftOrRight)
                {
                    Speed = SD.Left.Equipment.Speed * SD.Left.Equipment.Performance;
                    PossibleBroken = ((double)SD.Left.Equipment.Quality / 100.0);
                } else
                {
                    Speed = SD.Right.Equipment.Speed * SD.Right.Equipment.Performance;
                    PossibleBroken = ((double)SD.Right.Equipment.Quality / 100.0);
                }

                Speed *= _random.Next(1, 3);
                PossibleBroken *= (double)_random.Next(1, 5);

                //Heeft de driver een ongeluk? Dan staat hij stil maar kan wel verder als hij weer kapot gaat. - - maakt +
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
                    DriversChanged(this, new DriversChangedEventArgs(Track, NextSection, section));
                }

                //Voeg driver distance toe
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
                //Als distancer over 100 is, dan gaat de driver naar de volgende section
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

                //Probeer te verplaatsen naar LEFT van volgend section en anders RIGHT om inhalen te simuleren.
                //Ook check voor laps toevoegen.
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

                //Als hij verplaatst is, haal de driver bij de vorige section weg om dubbele drivers te verkomen.
                //Ook DriversChanged aanroepen zodat hij niet elke interval update.
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
                        DriversChanged(this, new DriversChangedEventArgs(Track, NextSection, section));
                    }
                }
            }
        }

        /// <summary>
        /// Voeg lap toe aan driver als het moet. Checkt ook voor als hij 3 laps heeft gemaakt om hem te finishen
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="SD"></param>
        /// <param name="SDnext"></param>
        private void AddLapToDriver(IParticipant Driver, SectionData SD, SectionData SDnext)
        {
            _participantslaps[Driver] += 1;
            Console.WriteLine($"{Driver.Naam} Lap: {_participantslaps[Driver]}");
            Thread.Sleep(500);
            if (_participantslaps[Driver] >= 4)
            {
                _participantsfinished[Driver] = true;
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
        /// Verwijder driver bij einde van race en geef punten. Check of er nog iemand op het circuit zit.
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

            //Geef ze punten gebaseerd op de Formule 1 manier
            if(FinishCounter == 1)
            {
                driver.Points += 25;
            } else if (FinishCounter == 2)
            {
                driver.Points += 18;
            } else if (FinishCounter == 3)
            {
                driver.Points += 15;
            } else if (FinishCounter > 3)
            {
                driver.Points += 12 - (2 * (FinishCounter - 3));
            }

            //Check of er ergens een driver is
            Boolean DriverFound = false;

            foreach(IParticipant participant in Participants)
            {
                if (!_participantsfinished[participant])
                {
                    DriverFound = true;
                }
            }

            //Als er geen driver is. Stop de race en verwijder timer en eventhandlers/
            //Roep op driverfinished voor de volgend race
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
