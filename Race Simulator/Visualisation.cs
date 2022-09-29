using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race_Simulator
{
    public static class Visualisation
    {
        static Directions CurrentDirection = Directions.North;
        public static void Initialise()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetBufferSize(200, 50);

        }

        static int CurrentXPos = 0;
        static int CurrentYPos = 0;
        //Zoek uit welk section er moet worden geprint
        public static void DrawTrack(Track track, Race race, Section? sectionedDriver = null)
        {

            Console.Clear();
            foreach (Section section in track.Sections)
            {

                switch (section.SectionType)
                {
                    case SectionTypes.Straight:
                        PrintTrack(_straight, race.GetSectionData(section));
                        //CurrentYPos += 6;
                        break;
                    case SectionTypes.LeftCorner:
                        PrintTrack(_leftcorner, race.GetSectionData(section));
                        break;
                    case SectionTypes.RightCorner:
                        PrintTrack(_rightcorner, race.GetSectionData(section));
                        CurrentDirection += 1;
                     break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid, race.GetSectionData(section));
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish, race.GetSectionData(section));
                        break;
                }

                if (CurrentDirection == Directions.North)
                {
                    Console.MoveBufferArea(0, 0, 11, 11, 0, 6);
                }
            }
        }

        //Voor elk string in de graphic, print het uit met goede gegevens
        private static void PrintTrack(string[] array, SectionData data)
        { 
            string[] strings;

            int CounterX = CurrentXPos;
            int CounterY = 0 + CurrentYPos;
            foreach (string toWrite in array)
            {
                if(CurrentDirection == Directions.North)
                {
                    Console.SetCursorPosition(CounterX, CounterY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                    CounterY++;
                }
                if (CurrentDirection == Directions.East)
                {
                    Console.SetCursorPosition(CounterX, CounterY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                }
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
                    String = String.Replace("@", "|" + left.Naam.Substring(0, 1));
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
                    String = String.Replace("#", right.Naam.Substring(0, 1) + "|");
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
            DrawTrack(e.Track, Data.CurrentRace, e.Section);
        }

        public static void OnDriversFinished(Object source, EventArgs e)
        {
            Data.CurrentRace = null;
            Data.NextRace();
            Data.CurrentRace.DriversChanged += Visualisation.OnDriverChanged;
        }

        #region Graphics

        //TODO Graphics verbeteren
        private static string[] _straight    = { "|         |",  "|         |", "| @   #   |",  "|         |", "|         |", "|         |" };
        private static string[] _leftcorner  = { "- - - - -  ", "          \\", "    @     |", "\\         |", "|     #   |", "|         |" };
        private static string[] _rightcorner = { "  - - - - -",  "/          ", "|     @    ",  "|         /", "|   #     |", "|         |" };
        private static string[] _startgrid   = { "|         |" , "| $ $ $ $ |", "| @       |",  "|         |", "|     #   |", "|         |" };
        private static string[] _finish      = { "|         |",  "| ! ! ! ! |", "|     @   |",  "|         |", "|   #     |", "|         |" };

        #endregion
    }
}
