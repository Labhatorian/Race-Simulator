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
        public static void Initialise()
        {
            //Console.BackgroundColor = ConsoleColor.Red;

        }

        //Zoek uit welk section er moet worden geprint
        public static void DrawTrack(Track track, Race race, Section sectionedDriver = null)
        {
            Boolean StopEarly = false;
            if(sectionedDriver != null)
            {
                StopEarly = true;
            }

            foreach(Section section in track.Sections)
            {
                //TODO Verbeteren
                Section usedSection = section;
                if (StopEarly)
                {
                    usedSection = sectionedDriver;
                }

                switch (usedSection.SectionType)
                {
                    case SectionTypes.Straight:
                        PrintTrack(_straight, race.GetSectionData(usedSection));
                        break;
                    case SectionTypes.LeftCorner:
                        PrintTrack(_leftcorner, race.GetSectionData(usedSection));
                        break;
                    case SectionTypes.RightCorner:
                        PrintTrack(_rightcorner, race.GetSectionData(usedSection));
                        break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid, race.GetSectionData(usedSection));
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish, race.GetSectionData(usedSection));
                        break;
                }

                if(usedSection != null)
                {
                    break;
                }
                
            }
        }

        //Voor elk string in de graphic, print het uit met goede gegevens
        private static void PrintTrack(string[] array, SectionData data)
        {
            foreach (string toWrite in array)
            {
                if (data.Left != null | data.Right != null)
                {
                    Console.WriteLine(SetParticipants(toWrite, data.Left, data.Right));
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

        public static void NextRace()
        {
            Data.CurrentRace.DriversChanged += OnDriverChanged;
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
