using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race_Simulator
{
    public static class Visualisation
    {
        //Voor het correct plaatsen van de sections
        static Directions CurrentDirection = Directions.North;

        /// <summary>
        /// Initialiseert de console
        /// </summary>
        public static void Initialise()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.CursorVisible = false;
        }

        //Variabelen die bij DrawTrack() horen
        static int CurrentXPos = 0;
        static int CurrentXCounter = 0;
        static int CurrentYCounter = 0;
        static int CurrentYPos = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="track"></param>
        /// <param name="race"></param>
        public static void DrawTrack(Track track, Race race)
        {
            Console.Clear();
            CurrentXPos = 0;
            CurrentXCounter = 0;
            CurrentYCounter = 0;
            CurrentYPos = 0;

            foreach (Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.Straight:
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_straighteast, race.GetSectionData(section));
                                break;
                            case Directions.South:
                                PrintTrack(_straightsouth, race.GetSectionData(section));
                                break;
                            case Directions.West:
                                PrintTrack(_straightwest, race.GetSectionData(section));
                                break;
                            default:
                                PrintTrack(_straight, race.GetSectionData(section));
                                break;
                        }
                        break;
                    case SectionTypes.LeftCorner:
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_leftcornereast, race.GetSectionData(section));
                                break;
                            case Directions.South:
                                PrintTrack(_leftcornersouth, race.GetSectionData(section));
                                break;
                            case Directions.West:
                                PrintTrack(_leftcornerwest, race.GetSectionData(section));
                                break;
                            default:
                                PrintTrack(_leftcorner, race.GetSectionData(section));
                                break;
                        }
                        CurrentDirection -= 1;
                        break;
                    case SectionTypes.RightCorner:
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                PrintTrack(_rightcornereast, race.GetSectionData(section));
                                break;
                            case Directions.South:
                                PrintTrack(_rightcornersouth, race.GetSectionData(section));
                                break;
                            case Directions.West:
                                PrintTrack(_rightcornerwest, race.GetSectionData(section));
                                break;
                            default:
                                PrintTrack(_rightcorner, race.GetSectionData(section));
                                break;
                        }
                        if (CurrentDirection != Directions.West)
                        {
                            CurrentDirection += 1;
                        } else
                        {
                            CurrentDirection = 0;
                        }
                     break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid, race.GetSectionData(section));
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish, race.GetSectionData(section));
                        break;
                }

                switch (CurrentDirection)
                {
                    case Directions.North:
                        if (section.SectionType != SectionTypes.Finish & section.SectionType != SectionTypes.RightCorner)
                        {
                            Console.MoveBufferArea(0, 0, 11, 12, 0, 6);
                        }

                        if (section.SectionType == SectionTypes.RightCorner)
                        {
                            CurrentXPos = 0;
                            CurrentYPos = 6 * (CurrentYCounter - 1);
                        }
                        break;
                    case Directions.East:
                        CurrentXCounter += 1;
                        CurrentXPos += 11;
                        if (section.SectionType == SectionTypes.RightCorner)
                        {
                            CurrentYPos = 6 * (CurrentYCounter);
                        }
                        break;
                    case Directions.South:
                        CurrentYPos += 6;
                        CurrentYCounter += 1;
                        if (section.SectionType == SectionTypes.RightCorner)
                        {
                            CurrentYPos = 6 * (CurrentYCounter);
                            CurrentXPos = 11 * CurrentXCounter;
                        }
                        break;
                    case Directions.West:
                        CurrentXPos -= 11;
                        CurrentXCounter -= 1;
                        if (section.SectionType == SectionTypes.RightCorner)
                        {
                            CurrentYPos = 6 * (CurrentYCounter);
                            CurrentXPos = 11 * (CurrentXCounter);
                        }
                        break;
                }
            }
            Console.SetCursorPosition(0, Console.WindowTop);
            Console.WriteLine($"{Data.CurrentRace.Track.Name.ToUpper()}!");
        }

        //Voor elk string in de graphic, print het uit met goede gegevens
        private static void PrintTrack(string[] array, SectionData data)
        { 
            string[] strings;

            int CounterX = CurrentXPos;
            int CounterY = CurrentYPos;
            foreach (string toWrite in array)
            {
                    Console.SetCursorPosition(CounterX, CounterY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                    CounterY++;
            }
        }

        //Zet de drivers op de goede plek in de section. Zet niks neer als er niemand is.
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
                    String.Replace("#", "%");
                }
            } else
            {
                String = String.Replace("#", " ");
            }
            return String;
        }

        //Handler voor bewegen drivers
        public static void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {   
            DrawTrack(e.Track, Data.CurrentRace);
        }

        public static void OnDriversFinished(Object source, EventArgs e)
        {
            Data.CurrentRace = null;
            Data.NextRace();

            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
                Data.CurrentRace.DriversFinished += Visualisation.OnDriversFinished;
            }
        }

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


        private static string[] _startgrid        = { "|         |" , "| $ $ $ $ |", "| @       |",  "|         |", "|     #   |", "|         |" };
        private static string[] _finish           = { "|         |",  "| ! ! ! ! |", "|     @   |",  "|         |", "|   #     |", "|         |" };
        #endregion
    }
}
