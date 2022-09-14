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
            Console.BackgroundColor = ConsoleColor.Red;
        }

        public static void DrawTrack(Track track)
        {
            foreach(Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.Straight:
                        PrintTrack(_straight);
                        break;
                    case SectionTypes.LeftCorner:
                        PrintTrack(_leftcorner);
                        break;
                    case SectionTypes.RightCorner:
                        PrintTrack(_rightcorner);
                        break;
                    case SectionTypes.StartGrid:
                        PrintTrack(_startgrid);
                        break;
                    case SectionTypes.Finish:
                        PrintTrack(_finish);
                        break;
                }
                
            }
        }

        private static void PrintTrack(string[] array)
        {
            foreach (string toWrite in array)
            {
                Console.WriteLine(toWrite);
            }
        }

        #region Graphics

        private static string[] _straight    = { "|         |",  "|         |", "| #   #   |",  "|         |", "|         |", "|         |" };
        private static string[] _leftcorner  = { "- - - - -  ", "          \\", "    #     |", "\\         |", "|     #   |", "|         |" };
        private static string[] _rightcorner = { "  - - - - -",  "/          ", "|     #    ",  "|         /", "|   #     |", "|         |" };
        private static string[] _startgrid   = { "|         |" , "| $ $ $ $ |", "| #       |",  "|         |", "|     #   |", "|         |" };
        private static string[] _finish      = { "|         |",  "| ! ! ! ! |", "|     #   |",  "|         |", "|   #     |", "|         |" };

        #endregion
    }
}
