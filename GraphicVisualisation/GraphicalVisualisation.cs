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
        public static Bitmap DrawTrack(Track Track, String String)
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
                            CurrentXCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.South:
                             g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            CurrentYCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.West:
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
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
    }
}
