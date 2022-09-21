using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition competition;
        public static Race CurrentRace;

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
            Track track = competition.NextTrack();
            if (track != null)
            {
                CurrentRace = new Race(track, competition.Participants);
            }
        }

        //Voeg deelnemers toe
        private static void AddParticipants()
        {
            competition.Participants = new List<IParticipant>();

            Driver DriverOne = new Driver();
            DriverOne.Naam = "Max Verstappen";

            Driver DriverTwo = new Driver();
            DriverTwo.Naam = "Lewis Hamilton";

            Driver DriverThree = new Driver();
            DriverThree.Naam = "Charles Leclerc";

            competition.Participants.Add(DriverOne);
            competition.Participants.Add(DriverTwo);
            competition.Participants.Add(DriverThree);
        }

        //Voeg circuits toe met hun bochten en straights
        private static void AddTracks()
        {
            competition.Tracks = new Queue<Track>();

            SectionTypes[] sectionTypesZandvoort = new SectionTypes[3];
            sectionTypesZandvoort[0] = (SectionTypes)3;
            sectionTypesZandvoort[1] = (SectionTypes)3;
            sectionTypesZandvoort[2] = (SectionTypes)2;
            //sectionTypesZandvoort[3] = (SectionTypes)1;
            //sectionTypesZandvoort[4] = (SectionTypes)4;

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
