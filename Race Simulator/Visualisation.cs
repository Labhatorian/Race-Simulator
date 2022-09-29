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
            Console.SetBufferSize(300, 100);

        }

        static int CurrentXPos = 0;
        static int CurrentXCounter = 0;
        static int CurrentYCounter = 0;
        static int CurrentYPos = 0;
        //Zoek uit welk section er moet worden geprint
        public static void DrawTrack(Track track, Race race, Section? sectionedDriver = null)
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
                        if (CurrentDirection == Directions.East)
                        {
                            PrintTrack(_straighteast, race.GetSectionData(section));
                        }
                        else if (CurrentDirection == Directions.South)
                        {
                            PrintTrack(_straightsouth, race.GetSectionData(section));
                        }
                        else if (CurrentDirection == Directions.West)
                        {
                            PrintTrack(_straightwest, race.GetSectionData(section));
                        } else
                        {
                            PrintTrack(_straight, race.GetSectionData(section));
                        }
                        break;
                    case SectionTypes.LeftCorner:
                        if (CurrentDirection == Directions.East)
                        {
                            PrintTrack(_leftcornereast, race.GetSectionData(section));
                        }
                        else if (CurrentDirection == Directions.South)
                        {
                            PrintTrack(_leftcornersouth, race.GetSectionData(section));
                        }
                        else if (CurrentDirection == Directions.West)
                        {
                            PrintTrack(_leftcornerwest, race.GetSectionData(section));
                        }
                        else
                        {
                            PrintTrack(_leftcorner, race.GetSectionData(section));
                        }

                        CurrentDirection -= 1;
                        break;
                    case SectionTypes.RightCorner:
                        if (CurrentDirection == Directions.East)
                        {
                            PrintTrack(_rightcornereast, race.GetSectionData(section));
                        }
                        else if (CurrentDirection == Directions.South)
                        {
                            PrintTrack(_rightcornersouth, race.GetSectionData(section));
                        }
                        else if (CurrentDirection == Directions.West)
                        {
                            PrintTrack(_rightcornerwest, race.GetSectionData(section));
                        }
                        else
                        {
                            PrintTrack(_rightcorner, race.GetSectionData(section));
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

                if (CurrentDirection == Directions.North)
                {
                    if (section.SectionType != SectionTypes.Finish & section.SectionType != SectionTypes.RightCorner)
                    {
                        Console.MoveBufferArea(0, 0, 11, 12, 0, 6);
                    }

                    if (section.SectionType == SectionTypes.RightCorner)
                    {
                        CurrentXPos = 0;
                        CurrentYPos = 6 * (CurrentYCounter-1);
                    }
                }

                if (CurrentDirection == Directions.East)
                {
                    CurrentXCounter += 1;
                    CurrentXPos += 11;
                    if (section.SectionType == SectionTypes.RightCorner)
                    {
                        CurrentYPos = 0;
                    }
                }

                if (CurrentDirection == Directions.South)
                {
                    CurrentYPos += 6;
                    CurrentYCounter += 1;
                    if (section.SectionType == SectionTypes.RightCorner)
                    {
                        CurrentYPos = 6;
                        CurrentXPos = 11 * CurrentXCounter;
                    }
                }

                if (CurrentDirection == Directions.West)
                {
                    CurrentXPos -= 11;
                    //CurrentXCounter -= 1;
                    if (section.SectionType == SectionTypes.RightCorner)
                    {
                        CurrentYPos = 6 * (CurrentYCounter );
                        CurrentXPos = 11 * (CurrentXCounter -1);
                    }
                }
            }
        }

        //Voor elk string in de graphic, print het uit met goede gegevens
        private static void PrintTrack(string[] array, SectionData data)
        { 
            string[] strings;

            int CounterX = CurrentXPos;
            int CounterY = CurrentYPos;
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
                    CounterY++;
                }
                if (CurrentDirection == Directions.South)
                {
                    Console.SetCursorPosition(CounterX, CounterY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                    CounterY++;
                }
                if (CurrentDirection == Directions.West)
                {
                    Console.SetCursorPosition(CounterX, CounterY);
                    Console.Write(SetParticipants(toWrite, data.Left, data.Right));
                    CounterY++;
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
                    String = String.Replace("@", ":");
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
                    String.Replace("#", ":");
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
        private static string[] _straight    = { "|         |",  "|         |", "| @   #   |",  "|         |", "|         |", "|         |" };
        private static string[] _straighteast = { "|         |", "|         |", "| @   #   |", "|         |", "|         |", "|         |" };
        private static string[] _straightsouth = { "|         |", "|         |", "| @   #   |", "|         |", "|         |", "|         |" };
        private static string[] _straightwest = { "|         |", "|         |", "| @   #   |", "|         |", "|         |", "|         |" };

        private static string[] _leftcorner  = { "- - - - -  ", "          \\", "    @     |", "\\         |", "|     #   |", "|         |" };
        private static string[] _leftcornereast = { "- - - - -  ", "          \\", "    @     |", "\\         |", "|     #   |", "|         |" };
        private static string[] _leftcornersouth = { "- - - - -  ", "          \\", "    @     |", "\\         |", "|     #   |", "|         |" };
        private static string[] _leftcornerwest = { "- - - - -  ", "          \\", "    @     |", "\\         |", "|     #   |", "|         |" };

        private static string[] _rightcorner = { "  - - - - -",  "/          ", "|     @    ",  "|         /", "|   #     |", "|         |" };
        private static string[] _rightcornereast = { "  - - - - -", "/          ", "|     @    ", "|         /", "|   #     |", "|         |" };
        private static string[] _rightcornersouth = { "  - - - - -", "/          ", "|     @    ", "|         /", "|   #     |", "|         |" };
        private static string[] _rightcornerwest = { "  - - - - -", "/          ", "|     @    ", "|         /", "|   #     |", "|         |" };


        private static string[] _startgrid   = { "|         |" , "| $ $ $ $ |", "| @       |",  "|         |", "|     #   |", "|         |" };
        private static string[] _finish      = { "|         |",  "| ! ! ! ! |", "|     @   |",  "|         |", "|   #     |", "|         |" };
        #endregion
    }
}
