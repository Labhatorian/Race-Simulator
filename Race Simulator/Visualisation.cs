using Controller;
using Model;

namespace Race_Simulator
{
    public static class Visualisation
    {
        //Voor het correct plaatsen van de sections
        private static Directions s_currentDirection = Directions.North;
        private static readonly Dictionary<Section, int[]> s_sectionPositions = new();

        /// <summary>
        /// Initialiseert de console
        /// </summary>
        public static void Initialise()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
        }

        //Variabelen die bij DrawTrack() horen
        private static int s_currentXPos = 0;
        private static int s_currentXCounter = 0;
        private static int s_currentYCounter = 0;
        private static int s_currentYPos = 0;
        private static int s_previousDirection;

        /// <summary>
        /// Code dat uitzoekt welk graphic moet worden geprint. En waar hij moet worden geprint.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="race"></param>
        public static void DrawTrack(Track track, Section? sectionDriver, Section? previousSection)
        {
            //Reset alles    
            s_currentXPos = 0;
            s_currentXCounter = 0;
            s_currentYCounter = 0;
            s_currentYPos = 0;
            LinkedList<Section> sections;
            //Deze zou bij Initialise() willen horen, maar soms luistert de console niet naar deze property.
            //Dus herinneren wij de console elke keer aan met een klap
            Console.CursorVisible = false;

            //Als we alleen een deel moeten updaten, voeg alleen die sections toe
            if (sectionDriver != null)
            {
                sections = new();
                sections.AddFirst(sectionDriver);
                sections.AddLast(previousSection);
            } else
            {
                Console.Clear();
                sections = new(track.Sections);
            }

            //Zoek uit voor elk sectie wat moet worden geprint
            //Bij left of right corner, verander richting
            foreach (Section section in sections)
            {
                //Sla direction op voor als we deze moeten opslaan
                s_previousDirection = (int)s_currentDirection;
                if (sectionDriver != null)
                {
                    s_currentDirection = (Directions)s_sectionPositions[section][2];
                }

                //Hier gaat de code langs om te kijken wat er moet worden gevisualiseerd
                //Verandert ook direction als het moet
                switch (section.SectionType)
                {
                    case SectionTypes.Straight:
                        switch (s_currentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_straighteast, Race.GetSectionData(section), section);
                                break;
                            case Directions.South:
                                PrintTrack(_straightsouth, Race.GetSectionData(section), section);
                                break;
                            case Directions.West:
                                PrintTrack(_straightwest, Race.GetSectionData(section), section);
                                break;
                            default:
                                PrintTrack(_straight, Race.GetSectionData(section), section);
                                break;
                        }
                        break;
                    case SectionTypes.LeftCorner:
                        switch (s_currentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_leftcornereast, Race.GetSectionData(section), section);
                                break;
                            case Directions.South:
                                PrintTrack(_leftcornersouth, Race.GetSectionData(section), section);
                                break;
                            case Directions.West:
                                PrintTrack(_leftcornerwest, Race.GetSectionData(section), section);
                                break;
                            default:
                                PrintTrack(_leftcorner, Race.GetSectionData(section), section);
                                break;
                        }
                        if (sectionDriver == null)
                        {
                            s_currentDirection -= 1;
                        }
                        break;
                    case SectionTypes.RightCorner:
                        switch (s_currentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_rightcornereast, Race.GetSectionData(section), section);
                                break;
                            case Directions.South:
                                PrintTrack(_rightcornersouth, Race.GetSectionData(section), section);
                                break;
                            case Directions.West:
                                PrintTrack(_rightcornerwest, Race.GetSectionData(section), section);
                                break;
                            default:
                                PrintTrack(_rightcorner, Race.GetSectionData(section), section);
                                break;
                        }

                        //Zodat hij reset naar boven bij einde
                        if (s_currentDirection != Directions.West & sectionDriver == null)
                        {
                            s_currentDirection += 1;
                        } else
                        {
                            s_currentDirection = 0;
                        }
                     break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid, Race.GetSectionData(section), section);
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish, Race.GetSectionData(section), section);
                        break;
                }

                //Slaat section x,y en direction op in dictionary voor later gebruik
                if (!s_sectionPositions.ContainsKey(section))
                {
                    s_sectionPositions.Add(section, new int[] { s_currentXPos, s_currentYPos, s_previousDirection});
                    //Update posities voor de rest van de track
                    switch (s_currentDirection)
                    {
                        case Directions.North:
                            //Verplaatst scherm naar onder zodat sections niet worden overwritten
                            if (section.SectionType != SectionTypes.Finish & section.SectionType != SectionTypes.RightCorner)
                            {
                                Console.MoveBufferArea(0, 0, 11, 12, 0, 6);

                                //We moeten alles omlaag plaatsen in de dictionary als we Console.MoveBufferArea hebben gebruikt
                                //Voorkomt dat hij verkeert print
                                foreach(Section movedsection in sections)
                                {
                                    if (s_sectionPositions.ContainsKey(movedsection))
                                    {
                                        s_sectionPositions[movedsection][1] += 6;
                                    }
                                }
                            }

                            //Noord is anders dus kan niet in functie
                            if (section.SectionType == SectionTypes.RightCorner)
                            {
                                s_currentXPos = 0;
                                s_currentYPos = 6 * (s_currentYCounter - 1);
                            }
                            break;
                        case Directions.East:
                            s_currentXCounter += 1;
                            s_currentXPos += 11;
                            SetCurrentXYPos(section, 11, 6);
                            break;
                        case Directions.South:
                            s_currentYPos += 6;
                            s_currentYCounter += 1;
                            SetCurrentXYPos(section, 11, 6);
                            break;
                        case Directions.West:
                            s_currentXPos -= 11;
                            s_currentXCounter -= 1;
                            SetCurrentXYPos(section, 11, 6);
                            break;
                    }
                }
            }
            //Schrijf naam van circuit op
            Console.SetCursorPosition(0, Console.WindowTop);
            Console.WriteLine($"{Data.CurrentRace.Track.Name.ToUpper()}!");
        }

        /// <summary>
        /// Update yPos en xPos voor de volgende section die moet worden gevisualiseerd
        /// </summary>
        /// <param name="section"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private static void SetCurrentXYPos(Section section, int xPos, int yPos)
        {
            if (section.SectionType == SectionTypes.RightCorner)
            {
                s_currentYPos = yPos * (s_currentYCounter);
                s_currentXPos = xPos * s_currentXCounter;
            }
        }

        /// <summary>
        /// Print graphic uit op de goede positie
        /// </summary>
        /// <param name="array"></param>
        /// <param name="data"></param>
        private static void PrintTrack(string[] array, SectionData data, Section section)
        { 
            string[] strings;
            int countX;
            int countY;

            //Haalt positie op als section al is opgeslagen met haar positie. Anders is het al berekent
            if (!s_sectionPositions.ContainsKey(section))
            {
                countX = s_currentXPos;
                countY = s_currentYPos;
            } else
            {
                countX = s_sectionPositions[section][0];
                countY = s_sectionPositions[section][1];

            }
            
            //Print out
            foreach (string toWrite in array)
            {
                    Console.SetCursorPosition(countX, countY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                    countY++;
            }
        }

        /// <summary>
        /// Zet voorletter van driver neer op de goede plek. Zet niks neer als er niemand is in SectionData of % als driver kapot is
        /// </summary>
        /// <param name="stringToReplace"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static string SetParticipants(string stringToReplace, IParticipant left, IParticipant right)
        {

            if (left != null)
            {
                if (!left.Equipment.IsBroken)
                {
                    stringToReplace = stringToReplace.Replace("@", left.Naam[..1]);
                }
                else
                {
                    stringToReplace = stringToReplace.Replace("@", "%");
                }
            } else
            {
                stringToReplace = stringToReplace.Replace("@", " ");
            }

            if (right != null)
            {
                if (!right.Equipment.IsBroken)
                {
                    stringToReplace = stringToReplace.Replace("#", right.Naam[..1]);
                } 
                else
                {
                    stringToReplace = stringToReplace.Replace("#", "%");
                }
            } else
            {
                stringToReplace = stringToReplace.Replace("#", " ");
            }
            return stringToReplace;
        }

        /// <summary>
        /// Eventhandler voor updaten visualisatie als er iets met driver is gebeurt
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {   
            DrawTrack(e.Track, e.Section, e.PreviousSection);
        }

        /// <summary>
        /// Eventhandler voor als de race if gefinisht en de volgende race wordt gestart. Moet hier ivm aanwijzen eventhandler aan events
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void OnDriversFinished(Object source, EventArgs e)
        {
            Data.CurrentRace = null;
            Data.NextRace();
            Visualisation.DrawTrack(Data.CurrentRace.Track, null, null);

            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
                Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;
                
            }

        }

        /// <summary>
        /// Graphics voor de sim
        /// </summary>
        #region Graphics
        private readonly static string[] _straight        = { "|         |", "|         |", "| @   #   |", "|         |", "|         |", "|         |" };
        private readonly static string[] _straighteast    = { "_ _ _ _ _ _", "           ", "    @      ", "           ", "    #      ", "_ _ _ _ _ _" };
        private readonly static string[] _straightsouth   = { "|         |", "|         |", "| #   @   |", "|         |", "|         |", "|         |" };
        private readonly static string[] _straightwest    = { "_ _ _ _ _ _", "           ", "    #      ", "           ", "    @      ", "_ _ _ _ _ _" };


        private readonly static string[] _leftcorner      = { "- - - - -  " , "          \\", "    @     |", "          |", "      #   |" , "\\         |" };
        private readonly static string[] _leftcornereast  = { "_         _" , "          \\", "      @   |", "          |", "  #       |" , "_ _ _ _ /  " };
        private readonly static string[] _leftcornersouth = { "|         \\", "|          " , "|   #      ", "|          ", "|     @   " , "\\ _ _ _ _ _" };
        private readonly static string[] _leftcornerwest  = { "  _ _ _ _ _" , "/          " , "|  #       ", "|          ", "|     @    " , "|          " };

        private readonly static string[] _rightcorner      = { "  _ _ _ _ _" , "/          " , "|     @    ", "|          ", "|   #      ", "|          " };
        private readonly static string[] _rightcornereast  = { "_ _ _ _ _  " , "          \\", "  #       |", "          |", "      @   |", "          |" };
        private readonly static string[] _rightcornersouth = { "/         |" , "          |" , "    #     |", "          |", "      @   |", "_ _ _ _ _ /" };
        private readonly static string[] _rightcornerwest  = { "|         \\", "|          " , "|  @       ", "|          ", "|   #      ", " \\ _ _ _ _ " };


        private readonly static string[] _startgrid        = { "| $ $ $ $ |" , "| ^       |", "| @       |",  "|     ^   |", "|     #   |", "|         |" };
        private readonly static string[] _finish           = { "|         |",  "| ! ! ! ! |", "|     @   |",  "|         |", "|   #     |", "|         |" };
        #endregion
    }
}