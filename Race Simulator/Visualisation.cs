using Controller;
using Model;

namespace Race_Simulator
{
    public static class Visualisation
    {
        //Voor het correct plaatsen van de sections
        static Directions CurrentDirection = Directions.North;
        private static Dictionary<Section, int[]> SectionPositions = new();

        /// <summary>
        /// Initialiseert de console
        /// </summary>
        public static void Initialise()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
          
        }

        //Variabelen die bij DrawTrack() horen
        static int CurrentXPos = 0;
        static int CurrentXCounter = 0;
        static int CurrentYCounter = 0;
        static int CurrentYPos = 0;
        static int PreviousDirection;

        /// <summary>
        /// Code dat uitzoekt welk graphic moet worden geprint. En waar hij moet worden geprint.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="race"></param>
        public static void DrawTrack(Track track, Race race, Section? sectiondriver, Section? previoussection)
        {
            //Reset alles    
            CurrentXPos = 0;
            CurrentXCounter = 0;
            CurrentYCounter = 0;
            CurrentYPos = 0;
            LinkedList<Section> Sections;
            //Deze zou bij Initialise() willen horen, maar soms luistert de console niet naar deze property.
            //Dus herinneren wij de console elke keer aan met een klap
            Console.CursorVisible = false;

            
            //Als we alleen een deel moeten updaten, voeg alleen die sections toe
            if (sectiondriver != null)
            {
                Sections = new();
                Sections.AddFirst(sectiondriver);
                Sections.AddLast(previoussection);
            } else
            {
                Console.Clear();
                Sections = new(track.Sections);
            }

            //Zoek uit voor elk sectie wat moet worden geprint
            //Bij left of right corner, verander richting
            foreach (Section section in Sections)
            {
                //Sla direction op voor als we deze moeten opslaan
                PreviousDirection = (int)CurrentDirection;
                if (sectiondriver != null)
                {
                    CurrentDirection = (Directions)SectionPositions[section][2];
                }

                //Hier gaat de code langs om te kijken wat er moet worden gevisualiseerd
                //Verandert ook direction als het moet
                switch (section.SectionType)
                {
                    case SectionTypes.Straight:
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_straighteast, race.GetSectionData(section), section);
                                break;
                            case Directions.South:
                                PrintTrack(_straightsouth, race.GetSectionData(section), section);
                                break;
                            case Directions.West:
                                PrintTrack(_straightwest, race.GetSectionData(section), section);
                                break;
                            default:
                                PrintTrack(_straight, race.GetSectionData(section), section);
                                break;
                        }
                        break;
                    case SectionTypes.LeftCorner:
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_leftcornereast, race.GetSectionData(section), section);
                                break;
                            case Directions.South:
                                PrintTrack(_leftcornersouth, race.GetSectionData(section), section);
                                break;
                            case Directions.West:
                                PrintTrack(_leftcornerwest, race.GetSectionData(section), section);
                                break;
                            default:
                                PrintTrack(_leftcorner, race.GetSectionData(section), section);
                                break;
                        }
                        if (sectiondriver == null)
                        {
                            CurrentDirection -= 1;
                        }
                        break;
                    case SectionTypes.RightCorner:
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_rightcornereast, race.GetSectionData(section), section);
                                break;
                            case Directions.South:
                                PrintTrack(_rightcornersouth, race.GetSectionData(section), section);
                                break;
                            case Directions.West:
                                PrintTrack(_rightcornerwest, race.GetSectionData(section), section);
                                break;
                            default:
                                PrintTrack(_rightcorner, race.GetSectionData(section), section);
                                break;
                        }

                        //Zodat hij reset naar boven bij einde
                        if (CurrentDirection != Directions.West & sectiondriver == null)
                        {
                            CurrentDirection += 1;
                        } else
                        {
                            CurrentDirection = 0;
                        }
                     break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid, race.GetSectionData(section), section);
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish, race.GetSectionData(section), section);
                        break;
                }

                //Slaat section x,y en direction op in dictionary voor later gebruik
                if (!SectionPositions.ContainsKey(section))
                {
                    SectionPositions.Add(section, new int[] { CurrentXPos, CurrentYPos, PreviousDirection});
                    //Update posities voor de rest van de track
                    switch (CurrentDirection)
                    {
                        case Directions.North:
                            //Verplaatst scherm naar onder zodat sections niet worden overwritten
                            if (section.SectionType != SectionTypes.Finish & section.SectionType != SectionTypes.RightCorner)
                            {
                                Console.MoveBufferArea(0, 0, 11, 12, 0, 6);

                                //We moeten alles omlaag plaatsen in de dictionary als we Console.MoveBufferArea hebben gebruikt
                                //Voorkomt dat hij verkeert print
                                foreach(Section movedsection in Sections)
                                {
                                    if (SectionPositions.ContainsKey(movedsection))
                                    {
                                        SectionPositions[movedsection][1] += 6;
                                    }
                                }
                            }

                            //Noord is anders dus kan niet in functie
                            if (section.SectionType == SectionTypes.RightCorner)
                            {
                                CurrentXPos = 0;
                                CurrentYPos = 6 * (CurrentYCounter - 1);
                            }
                            break;
                        case Directions.East:
                            CurrentXCounter += 1;
                            CurrentXPos += 11;
                            SetCurrentXYPos(section, 11, 6);
                            break;
                        case Directions.South:
                            CurrentYPos += 6;
                            CurrentYCounter += 1;
                            SetCurrentXYPos(section, 11, 6);
                            break;
                        case Directions.West:
                            CurrentXPos -= 11;
                            CurrentXCounter -= 1;
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
        /// Update Ypos en Xpos voor de volgende section die moet worden gevisualiseerd
        /// </summary>
        /// <param name="section"></param>
        /// <param name="Xpos"></param>
        /// <param name="Ypos"></param>
        private static void SetCurrentXYPos(Section section, int Xpos, int Ypos)
        {
            if (section.SectionType == SectionTypes.RightCorner)
            {
                CurrentYPos = Ypos * (CurrentYCounter);
                CurrentXPos = Xpos * CurrentXCounter;
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
            int CounterX;
            int CounterY;

            //Haalt positie op als section al is opgeslagen met haar positie. Anders is het al berekent
            if (!SectionPositions.ContainsKey(section))
            {
                CounterX = CurrentXPos;
                CounterY = CurrentYPos;
            } else
            {
                CounterX = SectionPositions[section][0];
                CounterY = SectionPositions[section][1];

            }
            
            //Print out
            foreach (string toWrite in array)
            {
                    Console.SetCursorPosition(CounterX, CounterY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                    CounterY++;
            }
        }

        /// <summary>
        /// Zet voorletter van driver neer op de goede plek. Zet niks neer als er niemand is in SectionData of % als driver kapot is
        /// </summary>
        /// <param name="String"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static string SetParticipants(string String, IParticipant left, IParticipant right)
        {

            if (left != null)
            {
                if (!left.Equipment.IsBroken)
                {
                    String = String.Replace("@", left.Naam.Substring(0, 1));
                }
                else
                {
                    String = String.Replace("@", "%");
                }
            } else
            {
                String = String.Replace("@", " ");
            }

            if (right != null)
            {
                if (!right.Equipment.IsBroken)
                {
                    String = String.Replace("#", right.Naam.Substring(0, 1));
                } 
                else
                {
                    String = String.Replace("#", "%");
                }
            } else
            {
                String = String.Replace("#", " ");
            }
            return String;
        }

        /// <summary>
        /// Eventhandler voor updaten visualisatie als er iets met driver is gebeurt
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {   
            DrawTrack(e.Track, Data.CurrentRace, e.Section, e.PreviousSection);
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
            Visualisation.DrawTrack(Data.CurrentRace.Track, Data.CurrentRace, null, null);

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
        private static string[] _straight        = { "|         |", "|         |", "| @   #   |", "|         |", "|         |", "|         |" };
        private static string[] _straighteast    = { "_ _ _ _ _ _", "           ", "    @      ", "           ", "    #      ", "_ _ _ _ _ _" };
        private static string[] _straightsouth   = { "|         |", "|         |", "| #   @   |", "|         |", "|         |", "|         |" };
        private static string[] _straightwest    = { "_ _ _ _ _ _", "           ", "    #      ", "           ", "    @      ", "_ _ _ _ _ _" };


        private static string[] _leftcorner      = { "- - - - -  " , "          \\", "    @     |", "          |", "      #   |" , "\\         |" };
        private static string[] _leftcornereast  = { "_         _" , "          \\", "      @   |", "          |", "  #       |" , "_ _ _ _ /  " };
        private static string[] _leftcornersouth = { "|         \\", "|          " , "|   #      ", "|          ", "|     @   " , "\\ _ _ _ _ _" };
        private static string[] _leftcornerwest  = { "  _ _ _ _ _" , "/          " , "|  #       ", "|          ", "|     @    " , "|          " };

        private static string[] _rightcorner      = { "  _ _ _ _ _" , "/          " , "|     @    ", "|          ", "|   #      ", "|          " };
        private static string[] _rightcornereast  = { "_ _ _ _ _  " , "          \\", "  #       |", "          |", "      @   |", "          |" };
        private static string[] _rightcornersouth = { "/         |" , "          |" , "    #     |", "          |", "      @   |", "_ _ _ _ _ /" };
        private static string[] _rightcornerwest  = { "|         \\", "|          " , "|  @       ", "|          ", "|   #      ", " \\ _ _ _ _ " };


        private static string[] _startgrid        = { "| $ $ $ $ |" , "| ^       |", "| @       |",  "|     ^   |", "|     #   |", "|         |" };
        private static string[] _finish           = { "|         |",  "| ! ! ! ! |", "|     @   |",  "|         |", "|   #     |", "|         |" };
        #endregion
    }
}