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
        public static void DrawTrack(Track track, Race race)
        {
            foreach(Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.Straight:
                        PrintTrack(_straight, race.GetSectionData(section));
                        break;
                    case SectionTypes.LeftCorner:
                        PrintTrack(_leftcorner, race.GetSectionData(section));
                        break;
                    case SectionTypes.RightCorner:
                        PrintTrack(_rightcorner, race.GetSectionData(section));
                        break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid, race.GetSectionData(section));
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish, race.GetSectionData(section));
                        break;
                }
                
            }
        }

        //Voor elk string in de graphic, print het uit met goede gegevens
        private static void PrintTrack(string[] array, SectionData data)
        {
            foreach (string toWrite in array)
            {
                Console.WriteLine(SetParticipants(toWrite, data.Left, data.Right));
            }
        }

        //Zet de drivers op de goede plek in de section. Zet niks neer als er niemand is.
        private static string SetParticipants(string String, IParticipant left, IParticipant right)
        {

            if (left != null)
            {
                String = String.Replace("@", left.Naam.Substring(0, 1));
            } else
            {
                String = String.Replace("@", " ");
            }

            if (right != null)
            {
               String = String.Replace("#", right.Naam.Substring(0, 1));
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

        #region Graphics

        private static string[] _straight    = { "|         |",  "|         |", "| @   #   |",  "|         |", "|         |", "|         |" };
        private static string[] _leftcorner  = { "- - - - -  ", "          \\", "    @     |", "\\         |", "|     #   |", "|         |" };
        private static string[] _rightcorner = { "  - - - - -",  "/          ", "|     @    ",  "|         /", "|   #     |", "|         |" };
        private static string[] _startgrid   = { "|         |" , "| $ $ $ $ |", "| @       |",  "|         |", "|     #   |", "|         |" };
        private static string[] _finish      = { "|         |",  "| ! ! ! ! |", "|     @   |",  "|         |", "|   #     |", "|         |" };

        #endregion
    }
}
