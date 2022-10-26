using Model;
using System.Media;
using Section = Model.Section;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        //Parameters voor de race
        public Track Track { get; set; }
        public List<IParticipant> Participants;
        private readonly Random _random;
        private Timer _timer;
        private int _finishCounter = 0;
        private SoundPlayer _soundPlayer;

        //Houdt bij belangrijke dingen voor de race
        public static Dictionary<Section, SectionData> positions;
        public static Dictionary<IParticipant, int> participantsLaps;
        private static Dictionary<IParticipant, Boolean> s_participantsFinished;

        //Eventhandlers voor verplaatste driver en gefinishte driver
        public event EventHandler<DriversChangedEventArgs>? DriversChanged;
        public event EventHandler<EventArgs> DriversFinished;

        /// <summary>
        /// Vind sectiondata van section. Als die niet bestaat, maak een nieuwe aan voor die section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static SectionData GetSectionData(Section section)
        {
            positions.TryGetValue(section, out var value);

            if (value != null)
            {
                return value;
            }
            else
            {
                SectionData sectionData = new();
                positions.Add(section, sectionData);
                return sectionData;
            }
        }

        /// <summary>
        /// Maak race aan. Haal gegevens op uit data van competitie en zet de timer, event en muziek klaar. Begin dan de race
        /// </summary>
        /// <param name="track"></param>
        /// <param name="participants"></param>
        public Race(Track track, List<IParticipant> participants, int timer)
        {
            //Maak de race
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            positions = new Dictionary<Section, SectionData>();
            participantsLaps = new Dictionary<IParticipant, int>();
            s_participantsFinished = new Dictionary<IParticipant, Boolean>();
            PlaceParticipants(track, participants);

            //Titel: Dramatic Music
            //Creator: PureDesign Girl - https://freesound.org/people/PureDesignGirl/
            //Source: https://freesound.org/people/PureDesignGirl/sounds/538828/
            //License: 'CC BY 4.0' - https://creativecommons.org/licenses/by/4.0/

            try
            {
                _soundPlayer = new SoundPlayer("..\\..\\..\\Content\\racemusic.wav");
            } catch (Exception e)
            {
                _soundPlayer = null;
            }
            

            //timer en eventhandler klaar en we starten
            _timer = new Timer(timer);
            _timer.Elapsed += OnTimedEvent;
            Start();
        }

        /// <summary>
        /// Geef de deelnemers een willekeurig waarde voor quality, performance en speed
        /// </summary>
        private void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(40, 100);
                participant.Equipment.Performance = _random.Next(4, 8);
                participant.Equipment.Speed = _random.Next(4, 8);
            }
        }


        /// <summary>
        /// Zet participants op de starting grid. De eerste is LEFT en de tweede is RIGHT enzo.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="participants"></param>
        private static void PlaceParticipants(Track track, List<IParticipant> participants)
        {
            //Telt bij welk deelnemer wij zijn
            int currentAt = 0;
            Boolean participantPlaced = false;

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
                            participantPlaced = true;

                            //Volgende participant gaat section lijst af
                            break;
                        }
                        else if (sectionData.Right == null)
                        {
                            sectionData.Right = participants[currentAt];
                            currentAt++;
                            participantPlaced = true;

                            //Volgende participant gaat section lijst af
                            break;
                        }
                        //Als section vol is, gaat hij naar de volgende
                    }
                }
                //Participant is niet geplaatst??? Gooi error
                if (!participantPlaced)
                {
                    throw new Exception("Not enough place for all participants on track");
                } else
                {
                    participantPlaced = false;
                }        
            }

            //Voor elk participant maak entry aan in participantsLaps
            //En voor finished
            foreach (IParticipant participant in participants)
            {
                participantsLaps.Add(participant, 1);
                s_participantsFinished.Add(participant, false);
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
                var newDictionary = positions.ToDictionary(entry => entry.Key,
                                                   entry => entry.Value);

                //Zoek section op
                foreach (KeyValuePair<Section, SectionData> entry in newDictionary)
                {
                    SectionData sectionData = entry.Value;
                    Section sectionTrack = entry.Key;

                    Section nextSection = sectionTrack;

                    //Vind volgende section
                    Boolean found = false;
                    Boolean addLap = false;
                    foreach (Section section in Track.Sections)
                    {
                        //Gevonden? Ga dan nog 1 keer door voor de volgende
                        if (found)
                        {
                            nextSection = section;
                            break;
                        }

                        if (section == sectionTrack)
                        {
                            //Als de volgende finish is, voeg dan 1 lap toe
                            if (sectionTrack.SectionType == SectionTypes.Finish)
                            {
                                nextSection = Track.Sections.First.Value;
                                addLap = true;
                                break;
                            }
                            else
                            {
                                found = true;
                            }
                        }
                        else
                        {
                            //Blijf dan zitten
                        }
                    }
                    SectionData sectionDataNext = GetSectionData(nextSection);

                    //Stuur elk driver door naar de functie DriverElapsed. De code gaat daar verder.
                    DriverElapsed(sectionData.Left, false, sectionTrack, sectionData, nextSection, sectionDataNext, addLap);
                    DriverElapsed(sectionData.Right, true, sectionTrack, sectionData, nextSection, sectionDataNext, addLap);

                }
            }
            //Start _timer weer op. Null check om exception te vermijden bij finish
            if (_timer != null)
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// Meerdere checks voor de drivers. Movement, breaking en laps finishen.
        /// Brein van de sim.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="leftOrRight"></param>
        /// <param name="sectionData"></param>
        /// <param name="nextSection"></param>
        /// <param name="sectionDataNext"></param>
        /// <param name="addLap"></param>
        private void DriverElapsed(IParticipant driver, Boolean leftOrRight, Section section, SectionData sectionData, Section nextSection, SectionData sectionDataNext, Boolean addLap)
        {
            if (driver != null)
            {
                //Pak waardes voor snelheid en kapot gaan
                int speed;
                int possibleBroken;

                if (!leftOrRight)
                {
                    speed = sectionData.Left.Equipment.Speed * sectionData.Left.Equipment.Performance;
                    possibleBroken = (sectionData.Left.Equipment.Quality);
                } else
                {
                    speed = sectionData.Right.Equipment.Speed * sectionData.Right.Equipment.Performance;
                    possibleBroken = (sectionData.Right.Equipment.Quality);
                }

                //Maakt de race minder voorspelbaar
                speed *= _random.Next(1, 3);
                // Zorg ervoor dat als driver kapot is, hij wel door gaan
                if (!driver.Equipment.IsBroken)
                {
                    possibleBroken += _random.Next(-25, 30);
                } else
                {
                    possibleBroken += _random.Next(-25, 0);
                }

                //Heeft de driver een ongeluk? Dan staat hij stil maar kan wel verder als hij weer kapot gaat. - - maakt +
                //Daarnaast moet hij natuurlijk een pitstop nemen om schade te herstellen
                if (possibleBroken <= 30)
                {
                    if (!driver.Equipment.IsBroken)
                    {
                        driver.Equipment.IsBroken = true;
                        driver.ToTakePitstop = true;
                    }
                    else
                    {
                        driver.Equipment.IsBroken = false;
                        driver.Equipment.Quality -= 1;
                    }
                    DriversChanged(this, new DriversChangedEventArgs(Track, nextSection, section));
                }

                //Voeg driver distance toe
                //Als speler op MoveDriver button heeft gedrukt, voeg extra distance toe
                //Dit gebeurt maar 1 keer per event zodat het nog wel een soort spel krijgt en het spel niet breekt.
                //Maar de gebruiker heeft de illusie dat er wel meer gebeurd als er op de knop wordt gehammerd
                if (!driver.Equipment.IsBroken && !driver.TakingPitstop)
                {
                    if (!leftOrRight)
                    {
                        sectionData.DistanceLeft += speed;
                        if (driver.Equipment.UserAddedDistance)
                        {
                            sectionData.DistanceLeft += 20;
                        }
                    }
                    else
                    {
                        sectionData.DistanceRight += speed;
                        if (driver.Equipment.UserAddedDistance)
                        {
                            sectionData.DistanceLeft += 20;
                        }
                    }
                } else if (driver.TakingPitstop)
                {
                    driver.TakingPitstop = false;
                }


                Boolean moved = false;
                //Als distancer over 100 is, dan gaat de driver naar de volgende section
                if (!leftOrRight)
                {
                    if (sectionData.DistanceLeft >= 100)
                    {
                        if (!driver.Equipment.IsBroken)
                        {
                            sectionData.DistanceLeft = 0;
                            moved = true;
                        }
                    }
                }
                else
                {
                    if (sectionData.DistanceRight >= 100)
                    {
                        if (!driver.Equipment.IsBroken)
                        {
                            sectionData.DistanceRight = 0;
                            moved = true;
                        }
                    }
                }

                //Probeer te verplaatsen naar LEFT van volgend section en anders RIGHT om inhalen te simuleren.
                //Ook check voor laps toevoegen.
                if (sectionDataNext.Left == null & moved)
                {
                    sectionDataNext.Left = driver;
                }
                else if (sectionDataNext.Right == null & moved)
                {
                    sectionDataNext.Right = driver; 
                }

                if (addLap & moved)
                {
                    AddLapToDriver(driver, sectionData, sectionDataNext);
                    if (driver.ToTakePitstop)
                    {
                        driver.ToTakePitstop = false;
                        driver.Equipment.Quality += 25;
                        driver.TakingPitstop = true;
                    }
                }

                //Als hij verplaatst is, haal de driver bij de vorige section weg om dubbele drivers te verkomen.
                //Ook DriversChanged aanroepen zodat hij niet elke interval update.
                if (moved)
                {
                    if (!leftOrRight)
                    {
                        sectionData.Left = null;
                    }
                    else
                    {
                        sectionData.Right = null;
                    }
                    if (_timer != null)
                    {
                        DriversChanged(this, new DriversChangedEventArgs(Track, nextSection, section));
                    }
                }
            }
        }

        /// <summary>
        /// Voeg lap toe aan driver als het moet. Checkt ook voor als hij 3 laps heeft gemaakt om hem te finishen
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="sectionData"></param>
        /// <param name="sectionDataNext"></param>
        private void AddLapToDriver(IParticipant driver, SectionData sectionData, SectionData sectionDataNext)
        {
            participantsLaps[driver] += 1;
            try
            {
                Console.SetCursorPosition(50, 0);
                Console.WriteLine($"{driver.Naam} Lap: {participantsLaps[driver]}");
                Thread.Sleep(500);
                Console.SetCursorPosition(50, 0);
                Console.WriteLine($"                     ");
            } catch (IOException)
            {
                //Doe niks, we zitten in de WPF project.
            }
                if (participantsLaps[driver] >= 4)
            {
                s_participantsFinished[driver] = true;
                RemoveLappedDriverAndCheck(driver, sectionDataNext, sectionData);
            }
        }

        /// <summary>
        /// Start de race. Willekeurig equipment timer autorestart uit zodat hij niet de thread volspamt met queries van eventhandler.
        /// Start ook muziek op
        /// </summary>
        private void Start()
        {
            RandomizeEquipment();
            _timer.AutoReset = false;
            _timer.Start();

            try
            {
                _soundPlayer.PlayLooping();
            }
            catch
            {
                _soundPlayer = null;
            }                    
        }

        /// <summary>
        /// Verwijder driver bij einde van race en geef punten op de Formule 1 manier. Check of er nog iemand op het circuit zit.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="sectionData"></param>
        /// <param name="previousSectionData"></param>
        private void RemoveLappedDriverAndCheck(IParticipant driver, SectionData sectionData, SectionData previousSectionData)
        {
            //Verwijder driver. Zorgt ervoor dat driver niet naar volgende section kan glippen
            if (sectionData.Left == driver)
            {
                sectionData.Left = null;
            }
            else if (sectionData.Right == driver)
            {
                sectionData.Right = null;
            }

            if (previousSectionData.Left == driver)
            {
                previousSectionData.Left = null;
            }
            else if (previousSectionData.Right == driver)
            {
                previousSectionData.Right = null;
            }

            //Geef ze punten gebaseerd op de Formule 1 manier
            _finishCounter += 1;
            switch (_finishCounter)
            {
                case 1:
                    driver.Points += 25;
                    break;
                case 2:
                    driver.Points += 18;
                    break;
                case 3:
                    driver.Points += 15;
                    break;
                default:
                    driver.Points += 12 - (2 * (_finishCounter - 3));
                    break;
            }

            //Check of er ergens een driver is
            Boolean driverFound = false;

            foreach(IParticipant participant in Participants)
            {
                if (!s_participantsFinished[participant])
                {
                    driverFound = true;
                }
            }

            //Als er geen driver is. Stop de race en verwijder _timer en eventhandlers en muziek
            //Roep op driverfinished voor de volgend race
            if (!driverFound)
            {
                if (_soundPlayer is not null)
                {
                    _soundPlayer.Stop();
                    _soundPlayer = null;
                }
                _timer.Stop();
                _timer.Enabled = false;
                _timer.Dispose();
                _timer = null;
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