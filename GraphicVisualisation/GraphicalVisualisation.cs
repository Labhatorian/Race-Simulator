using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Controller;
using Model;
using Track = Model.Track;

namespace GraphicVisualisation
{
    public static class GraphicalVisualisation
    {
        /// <summary>
        /// Bouw het circuit image. Gebruik een groot leeg afbeelding met de naam van het circuit en zet daar alle bitmaps in.
        /// </summary>
        /// <param name="Track"></param>
        /// <param name="String"></param>
        /// <returns></returns>
        /// 
        static Directions CurrentDirection = Directions.North;
        static int CurrentXCounter = 0;
        static int CurrentYCounter = 0;
        public static Bitmap DrawTrack(Race Race, Track Track, String String)
        {
            CurrentXCounter = 0;
            CurrentYCounter = 0;
            int OffsetFirstNorth = CountNorth(Track);

            
            Dictionary<Section, int[]> SectionPositions = new();

            Bitmap BM = LoadResources.GetBitmap("Empty");

            using (Graphics g = Graphics.FromImage(BM))
            {

                foreach (Section section in Track.Sections)
                {
                    Bitmap ImageSection = LoadResources.GetBitmap(section.SectionType.ToString());                 
                    SectionPositions.Add(section, new int[] { 500 * CurrentXCounter, 400 * CurrentYCounter });

                    switch (CurrentDirection)
                    {
                        case Directions.North:
                            int YOffset = 500 * OffsetFirstNorth;

                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter + YOffset), 500, 500);
                            DrawDrivers(Race.GetSectionData(section), g);
                            if (OffsetFirstNorth > 0){
                                OffsetFirstNorth--;
                            } else
                            {
                                CurrentYCounter--;
                                MoveDirection(section);
                            }
                            break;
                        case Directions.East:
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            DrawDrivers(Race.GetSectionData(section), g);
                            CurrentXCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.South:
                             g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            DrawDrivers(Race.GetSectionData(section), g);
                            CurrentYCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.West:
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            DrawDrivers(Race.GetSectionData(section), g);
                            CurrentXCounter--;
                            MoveDirection(section);
                            break;
                    }
                }
            }
            
            return BM;
        }

        private static int CountNorth(Track track)
        {
            int Counter = 0;
            foreach(Section section in track.Sections)
            {
                if(section.SectionType == SectionTypes.RightCorner || section.SectionType == SectionTypes.LeftCorner)
                {
                    break;
                    
                } else
                {
                    Counter++;
                }
            }
            return Counter;
        }

        private static void MoveDirection(Section section)
        {
            Boolean ChangedDirection = false;
            if (section.SectionType == SectionTypes.RightCorner)
            {
                if (CurrentDirection == Directions.West)
                {
                    CurrentDirection = 0;
                }
                else
                {
                    CurrentDirection += 1;
                }
                ChangedDirection = true;
            }

            if (section.SectionType == SectionTypes.LeftCorner)
            {
                CurrentDirection -= 1;
                ChangedDirection = true;

                if(CurrentDirection == Directions.South)
                {
                    CurrentXCounter += 2;
                }
                if (CurrentDirection == Directions.East)
                {
                    CurrentYCounter -= 2;

                }
            }

            if (ChangedDirection)
            {
                switch (CurrentDirection)
                {
                    //Unused?
                    case Directions.North:
                       CurrentYCounter--;
                       CurrentXCounter++;
                        break;
                    case Directions.East:
                        CurrentXCounter++;
                        CurrentYCounter++;
                        break;
                    case Directions.South:
                        CurrentYCounter++;
                        CurrentXCounter--;
                        break;
                    case Directions.West:
                        //CurrentDirection = 0;
                        CurrentXCounter--;
                        CurrentYCounter--;
                        break;
                }
            }

            if(CurrentXCounter <= 0)
            {
                CurrentXCounter = 0;
            }

            if(CurrentYCounter <= 0)
            {
                CurrentYCounter = 0;
            }
        }

        private static void DrawDrivers(SectionData sectionData, Graphics g)
        {
            IParticipant[] participants = new IParticipant[2];
            participants[0] = sectionData.Left;
            participants[1] = sectionData.Right;
            Bitmap ImageSection = null;

            foreach (IParticipant participant in participants)
            {
                if (participant != null)
                {
                    switch (participant.TeamColor)
                    {
                        case TeamColors.Red:
                            ImageSection = LoadResources.GetBitmap(TeamColors.Red.ToString());
                            break;
                        case TeamColors.Blue:
                            ImageSection = LoadResources.GetBitmap(TeamColors.Blue.ToString());
                            break;
                        case TeamColors.Yellow:
                            ImageSection = LoadResources.GetBitmap(TeamColors.Yellow.ToString());
                            break;
                        case TeamColors.Green:
                            ImageSection = LoadResources.GetBitmap(TeamColors.Green.ToString());
                            break;
                        case TeamColors.Grey:
                            ImageSection = LoadResources.GetBitmap(TeamColors.Grey.ToString());
                            break;
                    }
                }

                if (participants[0] == participant)
                {
                    if (ImageSection != null)
                    {
                        g.DrawImage(ImageSection, (500 * CurrentXCounter + 80), (500 * CurrentYCounter + 80), 150, 150);
                    }
                }

                if (participants[1] == participant)
                {
                    if (ImageSection != null)
                    {
                        g.DrawImage(ImageSection, (500 * CurrentXCounter + 100), (500 * CurrentYCounter + 120), 150, 150);
                    }
                }

            }
        }
    }
}
