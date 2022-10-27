using Model;

namespace Controller
{
    public static class Data
    {
        //De belangrijkste paremeters. Competitie en huidige race
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        public static Boolean QuickRace { get; set; }

        /// <summary>
        /// Initialiseer data en dus de competitie
        /// </summary>
        public static void Initialise()
        {
            //Maak nieuw competitie en voeg circuit en deelnemers toe
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        /// <summary>
        /// Volgende race
        /// </summary>
        public static void NextRace()
        {
            CurrentRace = null;
            Track track = Competition.NextTrack();
            if (track is not null)
            {
                if (!QuickRace)
                {
                    CurrentRace = new Race(track, Competition.Participants, 700);
                }
                else
                {
                    CurrentRace = new Race(track, Competition.Participants, 50);
                }
            } else
            {
                //Als het afgelopen is, laat de stand zien
                try
                {
                    Console.Clear();
                    Console.WriteLine($"De competitie is afgelopen!!!!");
                    Console.WriteLine($"De WK-stand is uiteindelijk geworden:");

                    foreach (IParticipant driver in Competition.Participants)
                    {
                        Console.WriteLine($"{driver.Naam}: {driver.Points}");
                    }
                } catch (IOException)
                {
                    //We zitten in de WPF project. Dit wordt in GraphicalVisualisation afgehandelt.
                }
            }
        }

        /// <summary>
        /// Voeg deelnemers toe aan de competitie
        /// </summary>
        private static void AddParticipants()
        {
            Competition.Participants = new List<IParticipant>();

            Driver driverOne = new()
            {
                Naam = "Max Verstappen",
                Equipment = new Car(),
                TeamColor = TeamColors.Blue
            };

            Driver driverTwo = new()
            {
                Naam = "Charles Leclerc",
                Equipment = new Car(),
                TeamColor = TeamColors.Red
            };

            Competition.Participants.Add(driverOne);
            Competition.Participants.Add(driverTwo);
        }

        /// <summary>
        /// Voeg de circuits toe
        /// </summary>
        private static void AddTracks()
        {
            Competition.Tracks = new Queue<Track>();

            SectionTypes[] sectionTypesZandvoort = new SectionTypes[14];
            //Naar boven
            sectionTypesZandvoort[0] = SectionTypes.StartGrid;
            sectionTypesZandvoort[1] = SectionTypes.Straight;
            sectionTypesZandvoort[2] = SectionTypes.RightCorner;

            //Naar rechts
            sectionTypesZandvoort[3] = SectionTypes.Straight;
            sectionTypesZandvoort[4] = SectionTypes.Straight;
            sectionTypesZandvoort[5] = SectionTypes.RightCorner;

            //Naar Beneden
            sectionTypesZandvoort[6] = SectionTypes.Straight;
            sectionTypesZandvoort[7] = SectionTypes.Straight;
            sectionTypesZandvoort[8] = SectionTypes.Straight;
            sectionTypesZandvoort[9] = SectionTypes.RightCorner;

            //Naar Links
            sectionTypesZandvoort[10] = SectionTypes.Straight;
            sectionTypesZandvoort[11] = SectionTypes.Straight;
            sectionTypesZandvoort[12] = SectionTypes.RightCorner;

            //Naar boven weer
            sectionTypesZandvoort[13] = SectionTypes.Finish;


            SectionTypes[] sectionTypesSpa = new SectionTypes[18];
            //Naar boven
            sectionTypesSpa[0] = SectionTypes.StartGrid;
            sectionTypesSpa[1] = SectionTypes.Straight;
            sectionTypesSpa[2] = SectionTypes.RightCorner;

            //Naar rechts
            sectionTypesSpa[3] = SectionTypes.Straight;
            sectionTypesSpa[4] = SectionTypes.Straight;
            sectionTypesSpa[5] = SectionTypes.RightCorner;

            //Naar Beneden
            sectionTypesSpa[6] = SectionTypes.RightCorner;

            // Naar Links
            sectionTypesSpa[7] = SectionTypes.Straight;
            sectionTypesSpa[8] = SectionTypes.LeftCorner;

            //Naar Beneden
            sectionTypesSpa[9] = SectionTypes.Straight;
            sectionTypesSpa[10] = SectionTypes.LeftCorner;

            //Naar Rechts
            sectionTypesSpa[11] = SectionTypes.Straight;
            sectionTypesSpa[12] = SectionTypes.RightCorner;

            //Naar Beneden
            sectionTypesSpa[13] = SectionTypes.RightCorner;

            //Naar Links
            sectionTypesSpa[14] = SectionTypes.Straight;
            sectionTypesSpa[15] = SectionTypes.Straight;
            sectionTypesSpa[16] = SectionTypes.RightCorner;

            //Naar boven weer
            sectionTypesSpa[17] = SectionTypes.Finish;

            Track trackOne = new("Zandvoort", sectionTypesZandvoort);
            Track trackTwo = new("Spa", sectionTypesSpa);

            Competition.Tracks.Enqueue(trackOne);
            Competition.Tracks.Enqueue(trackTwo);
            
        }
    }
}