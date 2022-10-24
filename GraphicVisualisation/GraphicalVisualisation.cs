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
        private static Directions s_currentDirection = Directions.North;
        private static int s_currentXCounter = 0;
        private static int s_currentYCounter = 0;
        private static readonly Dictionary<Section, int[]> s_sectionPositions = new();

        /// <summary>
        /// Maakt één afbeelding van het circuit met alles er op en er aan en geeft dat door aan MainImage in MainWindow
        /// </summary>
        /// <param name="race"></param>
        /// <param name="track"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap DrawTrack(Track track)
        {
            s_currentXCounter = 0;
            s_currentYCounter = 0;
            int offsetFirstTimeNorth = CountNorth(track);

            Bitmap bitMap = LoadResources.GetBitmap("Empty");

            using (Graphics g = Graphics.FromImage(bitMap))
            {
                foreach (Section section in track.Sections)
                {
                    Bitmap imageSection = LoadResources.GetBitmap(section.SectionType.ToString());                 
                    switch (s_currentDirection)
                    {
                        case Directions.North:
                            //Deze offset komt voort uit de console visualisatie. We beginnen met de StartGrid en die moet niet linksboven gedrawd worden.
                            int yOffset = 500 * offsetFirstTimeNorth;
                            g.DrawImage(imageSection, (500 * s_currentXCounter), (500 * s_currentYCounter + yOffset), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!s_sectionPositions.ContainsKey(section))
                            {
                                s_sectionPositions.Add(section, new int[] { (500 * s_currentXCounter), (500 * s_currentYCounter + yOffset) });
                            }
                            
                            if (offsetFirstTimeNorth > 0){
                                offsetFirstTimeNorth--;
                            } else
                            {
                                s_currentYCounter--;
                                MoveDirection(section);
                            }
                            break;
                        case Directions.East:
                            imageSection = RotateImage(imageSection, 90);
                            g.DrawImage(imageSection, (500 * s_currentXCounter), (500 * s_currentYCounter), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!s_sectionPositions.ContainsKey(section))
                            {
                                s_sectionPositions.Add(section, new int[] { (500 * s_currentXCounter), (500 * s_currentYCounter) });
                            }
                            s_currentXCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.South:
                            imageSection = RotateImage(imageSection, 180);
                            g.DrawImage(imageSection, (500 * s_currentXCounter), (500 * s_currentYCounter), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!s_sectionPositions.ContainsKey(section))
                            {
                                s_sectionPositions.Add(section, new int[] { (500 * s_currentXCounter), (500 * s_currentYCounter) });
                            }

                            s_currentYCounter++;
                            MoveDirection(section);
                            break;
                        case Directions.West:
                            imageSection = RotateImage(imageSection, 270);
                            g.DrawImage(imageSection, (500 * s_currentXCounter), (500 * s_currentYCounter), 500, 500);
                            PositionDrivers(Race.GetSectionData(section), g, section);
                            if (!s_sectionPositions.ContainsKey(section))
                            {
                                s_sectionPositions.Add(section, new int[] { (500 * s_currentXCounter), (500 * s_currentYCounter) });
                            }
                            s_currentXCounter--;
                            MoveDirection(section);
                            break;
                    }
                }
            }     
            return bitMap;
        }

        /// <summary>
        /// Telt op hoe veel sections er zijn tot de eerste corner bij begin van race
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        private static int CountNorth(Track track)
        {
            int counter = 0;
            foreach(Section section in track.Sections)
            {
                if(section.SectionType == SectionTypes.RightCorner || section.SectionType == SectionTypes.LeftCorner)
                {
                    break;
                    
                } else
                {
                    counter++;
                }
            }
            return counter;
        }

        /// <summary>
        /// Verandert Direction en dus ook xCounter en yCounter zodat elk section op de goede plek terecht komt
        /// </summary>
        /// <param name="section"></param>
        private static void MoveDirection(Section section)
        {
            Boolean changedDirection = false;
            if (section.SectionType == SectionTypes.RightCorner)
            {
                if (s_currentDirection == Directions.West)
                {
                    s_currentDirection = 0;
                }
                else
                {
                    s_currentDirection += 1;
                }
                changedDirection = true;
            }

            if (section.SectionType == SectionTypes.LeftCorner)
            {
                s_currentDirection -= 1;
                changedDirection = true;

                if(s_currentDirection == Directions.South)
                {
                    s_currentXCounter += 2;
                }
                if (s_currentDirection == Directions.East)
                {
                    s_currentYCounter -= 2;

                }
            }

            if (changedDirection)
            {
                switch (s_currentDirection)
                {
                    case Directions.North:
                       s_currentYCounter--;
                       s_currentXCounter++;
                        break;
                    case Directions.East:
                        s_currentXCounter++;
                        s_currentYCounter++;
                        break;
                    case Directions.South:
                        s_currentYCounter++;
                        s_currentXCounter--;
                        break;
                    case Directions.West:
                        s_currentXCounter--;
                        s_currentYCounter--;
                        break;
                }
            }

            if(s_currentXCounter <= 0)
            {
                s_currentXCounter = 0;
            }

            if(s_currentYCounter <= 0)
            {
                s_currentYCounter = 0;
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
            Bitmap imageSection = null;

            //Haal kleur en direction op
            foreach (IParticipant participant in participants)
            {
                if (participant != null)
                {
                    switch (participant.TeamColor)
                    {
                        case TeamColors.Red:
                            imageSection = LoadResources.GetBitmap(TeamColors.Red.ToString());
                            break;
                        case TeamColors.Blue:
                            imageSection = LoadResources.GetBitmap(TeamColors.Blue.ToString());
                            break;
                        case TeamColors.Yellow:
                            imageSection = LoadResources.GetBitmap(TeamColors.Yellow.ToString());
                            break;
                        case TeamColors.Green:
                            imageSection = LoadResources.GetBitmap(TeamColors.Green.ToString());
                            break;
                        case TeamColors.Grey:
                            imageSection = LoadResources.GetBitmap(TeamColors.Grey.ToString());
                            break;
                    }


                    if (imageSection != null)
                    {
                        switch (s_currentDirection)
                        {
                            case Directions.East:
                                imageSection = RotateImage(imageSection, 90);
                                break;
                            case Directions.South:
                                imageSection = RotateImage(imageSection, 180);
                                break;
                            case Directions.West:
                                imageSection = RotateImage(imageSection, 270);
                                break;
                            default:
                                break;
                        }
                    }
                    DrawDriver(sectionData, g, section, imageSection, participant);
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
        /// <param name="imageSection"></param>
        /// <param name="participant"></param>
        private static void DrawDriver(SectionData sectionData, Graphics g, Section section, Bitmap imageSection, IParticipant participant)
        {
            int xPos = 0;
            int yPos = 0;
            Bitmap imageBroken = LoadResources.GetBitmap("Broken");
            Bitmap imagePitstop = LoadResources.GetBitmap("Pitstop");
            if (imageSection != null & (s_sectionPositions.ContainsKey(section)))
            {
                switch (s_currentDirection)
                {
                    case Directions.East:
                        if (sectionData.Left == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 170;
                            yPos = s_sectionPositions[section][1] + 120;
                        }
                        if (sectionData.Right == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 20;
                            yPos = s_sectionPositions[section][1] + 160;
                        }
                        imageBroken = RotateImage(imageBroken, 90);
                        break;
                    case Directions.South:
                        if (sectionData.Left == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 200;
                            yPos = s_sectionPositions[section][1] + 200;
                        }
                        if (sectionData.Right == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 100;
                            yPos = s_sectionPositions[section][1] + 80;
                        }
                        imageBroken = RotateImage(imageBroken, 180);
                        break;
                    case Directions.West:
                        if (sectionData.Left == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 100;
                            yPos = s_sectionPositions[section][1] + 160;
                        }
                        if (sectionData.Right == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 200;
                            yPos = s_sectionPositions[section][1] + 120;
                        }
                        imageBroken = RotateImage(imageBroken, 270);
                        break;
                    default:
                        if (sectionData.Left == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 100;
                            yPos = s_sectionPositions[section][1] + 120;
                        }
                        if (sectionData.Right == participant)
                        {
                            xPos = s_sectionPositions[section][0] + 250;
                            yPos = s_sectionPositions[section][1] + 220;
                        }
                        break;
                }
                g.DrawImage(imageSection, xPos, yPos, 150, 150);
                if (participant.Equipment.IsBroken)
                { 
                    g.DrawImage(imageBroken, xPos, yPos, 150, 150);
                }
                if (participant.TakingPitstop)
                {
                    g.DrawImage(imagePitstop, xPos, yPos, 150, 150);
                }
            }
        }

        /// <summary>
        /// Draait bitmap om met een gekozen graad
        /// </summary>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <returns>Bitmap</returns>
        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new(b.Width, b.Height);
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