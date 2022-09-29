using Model;

namespace Controller
{
    public static class Data
    {
        private static Competition competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void SetCompetition(Competition comp)
        {
            competition = comp;
        }

        public static void Initialise()
        {
            //Maak nieuw competitie en voeg circuit en deelnemers toe
            competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        //Volgende race dus circuit
        public static void NextRace()
        {
            CurrentRace = null;
            Track track = competition.NextTrack();
            if (track != null)
            {
                CurrentRace = new Race(track, competition.Participants);
                Console.WriteLine($"Op naar: {Data.CurrentRace.Track.Name}!");
            } else
            {
                Console.WriteLine($"De competitie is afgelopen!!!!");
            }
        }

        //Voeg deelnemers toe
        private static void AddParticipants()
        {
            competition.Participants = new List<IParticipant>();

            Driver DriverOne = new Driver();
            DriverOne.Naam = "Max Verstappen";
            DriverOne.Equipment = new Car();

            Driver DriverTwo = new Driver();
            DriverTwo.Naam = "Lewis Hamilton";
            DriverTwo.Equipment = new Car();

            competition.Participants.Add(DriverOne);
            competition.Participants.Add(DriverTwo);
        }

        //Voeg circuits toe met hun bochten en straights
        private static void AddTracks()
        {
            competition.Tracks = new Queue<Track>();

            SectionTypes[] sectionTypesZandvoort = new SectionTypes[3];
            //Naar boven
            sectionTypesZandvoort[0] = SectionTypes.StartGrid;
            sectionTypesZandvoort[1] = SectionTypes.Straight;
            sectionTypesZandvoort[2] = SectionTypes.RightCorner;
            //sectionTypesZandvoort[0] = SectionTypes.RightCorner;

            //Naar rechts
            //sectionTypesZandvoort[3] = SectionTypes.Straight;

            SectionTypes[] sectionTypesSpa = new SectionTypes[3];
            sectionTypesSpa[0] = (SectionTypes)3;
            sectionTypesSpa[1] = (SectionTypes)3;
            sectionTypesSpa[2] = (SectionTypes)1;

            SectionTypes[] sectionTypesMonza = new SectionTypes[3];
            sectionTypesMonza[0] = (SectionTypes)3;
            sectionTypesMonza[1] = (SectionTypes)3;
            sectionTypesMonza[2] = (SectionTypes)2;

            Track TrackOne = new Track("Zandvoort", sectionTypesZandvoort);
            Track TrackTwo = new Track("Spa", sectionTypesSpa);
            Track TrackThree = new Track("Monza", sectionTypesMonza);

            competition.Tracks.Enqueue(TrackOne);
            competition.Tracks.Enqueue(TrackTwo);
            competition.Tracks.Enqueue(TrackThree);
        }
    }
}
