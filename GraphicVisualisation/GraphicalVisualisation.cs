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
        static Directions CurrentDirection = Directions.North;
        static int CurrentXCounter = 0;
        static int CurrentYCounter = 0;
        static Dictionary<Section, int[]> SectionPositions = new();

        /// <summary>
        /// Maakt één afbeelding van het circuit met alles er op en er aan en geeft dat door aan MainImage in MainWindow
        /// </summary>
        /// <param name="Race"></param>
        /// <param name="Track"></param>
        /// <param name="String"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap DrawTrack(Race Race, Track Track, String String)
        {
            CurrentXCounter = 0;
            CurrentYCounter = 0;
            int OffsetFirstNorth = CountNorth(Track);

            Bitmap BM = LoadResources.GetBitmap("Empty");

            using (Graphics g = Graphics.FromImage(BM))
            {
                foreach (Section section in Track.Sections)
                {
                    Bitmap ImageSection = LoadResources.GetBitmap(section.SectionType.ToString());                 
                    switch (CurrentDirection)
                    {
                        case Directions.North:
                            //Deze offset komt voort uit de console visualisatie. We beginnen met de StartGrid en die moet niet linksboven gedrawd worden.
                            int YOffset = 500 * OffsetFirstNorth;
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter + YOffset), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!SectionPositions.ContainsKey(section))
                            {
                                SectionPositions.Add(section, new int[] { (500 * CurrentXCounter), (500 * CurrentYCounter + YOffset) });
                            }
                            
                            if (OffsetFirstNorth > 0){
                                OffsetFirstNorth--;
                            } else
                            {
                                CurrentYCounter--;
                                MoveDirection(section);
                            }
                            break;
                        case Directions.East:
                            ImageSection = RotateImage(ImageSection, 90);
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!SectionPositions.ContainsKey(section))
                            {
                                SectionPositions.Add(section, new int[] { (500 * CurrentXCounter), (500 * CurrentYCounter) });
                            }
                            CurrentXCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.South:
                            ImageSection = RotateImage(ImageSection, 180);
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!SectionPositions.ContainsKey(section))
                            {
                                SectionPositions.Add(section, new int[] { (500 * CurrentXCounter), (500 * CurrentYCounter) });
                            }

                            CurrentYCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.West:
                            ImageSection = RotateImage(ImageSection, 270);
                            g.DrawImage(ImageSection, (500 * CurrentXCounter), (500 * CurrentYCounter), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!SectionPositions.ContainsKey(section))
                            {
                                SectionPositions.Add(section, new int[] { (500 * CurrentXCounter), (500 * CurrentYCounter) });
                            }
                            CurrentXCounter--;
                            MoveDirection(section);
                            break;
                    }
                }
            }
            
            return BM;
        }

        /// <summary>
        /// Telt op hoe veel sections er zijn tot de eerste corner bij begin van race
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Verandert Direction en dus ook X en Y
        /// </summary>
        /// <param name="section"></param>
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

        /// <summary>
        /// Methode dat de drivers op het scherm positioneert en goede kleur geeft
        /// </summary>
        /// <param name="sectionData"></param>
        /// <param name="g"></param>
        /// <param name="section"></param>
        private static void PositionDrivers(SectionData sectionData, Graphics g, Section section)
        {
            IParticipant[] participants = new IParticipant[2];
            participants[0] = sectionData.Left;
            participants[1] = sectionData.Right;
            Bitmap ImageSection = null;

            //Haal kleur en direction op
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


                    if (ImageSection != null)
                    {
                        switch (CurrentDirection)
                        {
                            case Directions.East:
                                ImageSection = RotateImage(ImageSection, 90);
                                break;
                            case Directions.South:
                                ImageSection = RotateImage(ImageSection, 180);
                                break;
                            case Directions.West:
                                ImageSection = RotateImage(ImageSection, 270);
                                break;
                            default:
                                break;
                        }
                    }

                    DrawDriver(sectionData, g, section, ImageSection, participant);
                }

            }
        }

        /// <summary>
        /// Methode dat driver op het bitmap plaatst per direction
        /// De X en Y wordt geoffset en komt uit de opgeslagen posities van toen het circuit werd gedrawd
        /// </summary>
        /// <param name="sectionData"></param>
        /// <param name="g"></param>
        /// <param name="section"></param>
        /// <param name="ImageSection"></param>
        /// <param name="participant"></param>
        private static void DrawDriver(SectionData sectionData, Graphics g, Section section, Bitmap ImageSection, IParticipant participant)
        {
            int Xpos = 0;
            int Ypos = 0;
            Bitmap ImageBroken = LoadResources.GetBitmap("Broken");
            Bitmap ImagePitstop = LoadResources.GetBitmap("Pitstop");
            if (ImageSection != null & (SectionPositions.ContainsKey(section)))
            {
                switch (CurrentDirection)
                {
                    case Directions.East:
                        if (sectionData.Left == participant)
                        {
                            Xpos = SectionPositions[section][0] + 170;
                            Ypos = SectionPositions[section][1] + 120;
                        }
                        if (sectionData.Right == participant)
                        {
                            Xpos = SectionPositions[section][0] + 20;
                            Ypos = SectionPositions[section][1] + 160;
                        }
                        ImageBroken = RotateImage(ImageBroken, 90);
                        break;
                    case Directions.South:
                        if (sectionData.Left == participant)
                        {
                            Xpos = SectionPositions[section][0] + 200;
                            Ypos = SectionPositions[section][1] + 200;
                        }
                        if (sectionData.Right == participant)
                        {
                            Xpos = SectionPositions[section][0] + 100;
                            Ypos = SectionPositions[section][1] + 80;
                        }
                        ImageBroken = RotateImage(ImageBroken, 180);
                        break;
                    case Directions.West:
                        if (sectionData.Left == participant)
                        {
                            Xpos = SectionPositions[section][0] + 100;
                            Ypos = SectionPositions[section][1] + 160;
                        }
                        if (sectionData.Right == participant)
                        {
                            Xpos = SectionPositions[section][0] + 200;
                            Ypos = SectionPositions[section][1] + 120;
                        }
                        ImageBroken = RotateImage(ImageBroken, 270);
                        break;
                    default:
                        if (sectionData.Left == participant)
                        {
                            Xpos = SectionPositions[section][0] + 100;
                            Ypos = SectionPositions[section][1] + 120;
                        }
                        if (sectionData.Right == participant)
                        {
                            Xpos = SectionPositions[section][0] + 250;
                            Ypos = SectionPositions[section][1] + 220;
                        }
                        break;
                }
                g.DrawImage(ImageSection, Xpos, Ypos, 150, 150);
                if (participant.Equipment.IsBroken)
                { 
                    g.DrawImage(ImageBroken, Xpos, Ypos, 150, 150);
                }
                if (participant.TakingPitstop)
                {
                    g.DrawImage(ImagePitstop, Xpos, Ypos, 150, 150);
                }
            }
        }

        /// <summary>
        /// Draait bitmap om met een gekozen graad
        /// https://stackoverflow.com/questions/36871291/how-do-you-rotate-a-bitmap-an-arbitrary-number-of-degrees
        /// </summary>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }
    }
}