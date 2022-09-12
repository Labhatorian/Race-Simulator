using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    internal static class Data
    {
        public static Competition competition;
        public static Race CurrentRace;

        public static void initialise()
        {
            competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        public static void NextRace()
        {
            Track track = competition.NextTrack();
            if (track != null)
            {
                CurrentRace = new Race(track, competition.Participants);
            }
        }

        private static void AddParticipants()
        {
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

        private static void AddTracks()
        {
            SectionTypes[] sectionTypesZandvoort = new SectionTypes[2];
            sectionTypesZandvoort[0] = (SectionTypes)1;
            sectionTypesZandvoort[1] = (SectionTypes)3;
            sectionTypesZandvoort[2] = (SectionTypes)2;

            SectionTypes[] sectionTypesSpa = new SectionTypes[2];
            sectionTypesSpa[0] = (SectionTypes)1;
            sectionTypesSpa[1] = (SectionTypes)2;
            sectionTypesSpa[2] = (SectionTypes)3;

            SectionTypes[] sectionTypesMonza = new SectionTypes[2];
            sectionTypesMonza[0] = (SectionTypes)3;
            sectionTypesMonza[1] = (SectionTypes)2;
            sectionTypesMonza[2] = (SectionTypes)1;

            Track TrackOne = new Track("Zandvoort", sectionTypesZandvoort);
            Track TrackTwo = new Track("Spa", sectionTypesSpa);
            Track TrackThree = new Track("Monza", sectionTypesMonza);

            competition.Tracks.Enqueue(TrackOne);
            competition.Tracks.Enqueue(TrackTwo);
            competition.Tracks.Enqueue(TrackThree);
        }
    }
}
