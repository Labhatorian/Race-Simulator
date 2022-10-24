using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace GraphicVisualisation
{
    public class RaceSimDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Tabellen en variable voor gegevens competitie en race
        /// </summary>
  
        public ObservableCollection<CompetitionRow> CompetitionStats { get; set; } = new();
        public ObservableCollection<DriverRow> RaceDrivers { get; set; } = new();
        public ObservableCollection<DriverInfo> RaceDriversDriverInfo { get; set; } = new();
        public string SelectedDriver;

        /// <summary>
        /// Voor de label bovenaan de scherm. Laat circuitnaam zien
        /// _tracknames wordt niet geupdatet tijdens de competitie
        /// </summary>
        public string trackname { get; set; }
        private List<Track> _tracknames = Data.Competition.Tracks.ToList();

        /// <summary>
        /// Begin met als eerst gegevens ophalen zodat er geen leeg label en tabellen zijn bij opstarten
        /// </summary>
        public RaceSimDataContext()
        {
            trackname = GetTrackName();
            DataContextRefresh();
            OnPropertyChanged("trackname");
        }

        /// <summary>
        /// Om na elk race opnieuw op te halen
        /// </summary>
        public void DataContextRefresh()
        {
            Data.CurrentRace.DriversFinished += OnDriverFinished;
            Data.CurrentRace.DriversChanged += OnDriverChanged;  
            UpdateCompetitionInfo();
            UpdateRaceDrivers();
        }

        /// <summary>
        /// Elke keer als iets verandert. Update alle properties, zodat de updates te zien is op het scherm
        /// </summary>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Eventhandler die trackname ophaalt. Wordt hier gedaan omdat bij OnDriverFinished niet mogelijk is, de volgende race is nog niet bekend.
        /// Kijkt of bij tabel van race driver is geselecteerd en update de tabel als dat zo is
        /// Updatet competitie tabel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDriverChanged(object sender, EventArgs e)
        {
            trackname = GetTrackName();
            if (SelectedDriver != null)
            {
                IParticipant driver = (IParticipant)Data.Competition.Participants.Where(s => s.Naam.Equals(SelectedDriver)).Single();
                UpdateRaceDriverInfo(driver);
                OnPropertyChanged("RaceDriversDriverInfo");
            }
            UpdateCompetitionInfo();
            OnPropertyChanged(trackname);
        }

        /// <summary>
        /// Bij einde van race, update competitie info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDriverFinished(object sender, EventArgs e)
        {
            UpdateCompetitionInfo();         
            OnPropertyChanged(trackname);
        }

        /// <summary>
        /// Haalt trackname op met lambda uit variable
        /// </summary>
        /// <returns></returns>
        private string GetTrackName()
        {
            return _tracknames.Select(x => Data.CurrentRace.Track.Name).First();
        }

        /// <summary>
        /// Haalt naam en punten op van elk coureur in de competitie
        /// </summary>
        private void UpdateCompetitionInfo()
        {
            CompetitionStats = new();
            Data.Competition.Participants.Where(s => Data.Competition.Participants.Contains(s))
                .ToList()
                .ForEach(i => CompetitionStats.Add(new CompetitionRow(i.Naam, i.Points)));
            OnPropertyChanged("CompetitionStats");
        }


        /// <summary>
        /// Haalt naam en kleur op van elk coureur in de competitie
        /// </summary>
        private void UpdateRaceDrivers()
    {
        RaceDrivers = new();
        Data.Competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
            .ToList()
            .ForEach(i => RaceDrivers.Add(new DriverRow(i.Naam, i.TeamColor.ToString())));
            OnPropertyChanged("RaceDrivers");
        }

        /// <summary>
        /// Als een driver is geselecteerd, haal de informatie op van de geselecteerde driver
        /// </summary>
        /// <param name="driver"></param>
        private void UpdateRaceDriverInfo(IParticipant driver)
        {
            RaceDriversDriverInfo = new();
            int Lapcount = Race.participantsLaps.Where(p => p.Key == driver).Select(p => p.Value).Single();
            Data.Competition.Participants.Where(s =>
            {
                return s == driver;
            }).ToList().ForEach(i =>
            {
                RaceDriversDriverInfo.Add(new DriverInfo(Lapcount, i.Equipment.Quality, i.Equipment.Performance, i.Equipment.Speed, i.Equipment.IsBroken));
            });
            OnPropertyChanged("RaceDriversDriverInfo");
        }
    }

    /// <summary>
    /// Class voor de tabel van competitie
    /// </summary>
    public class CompetitionRow
    {
        public string Name { get; set; }
        public int Points { get; set; }

        public CompetitionRow(string name, int points)
        {
            Name = name;
            Points = points;
        }
    }

    /// <summary>
    /// Class voor de tabel van drivers in race
    /// </summary>
    public class DriverRow
    {
        public string Naam { get; set; }
        public string TeamColour { get; set; }

        public DriverRow(string name, string teamColour)
        {
            Naam = name;
            TeamColour = teamColour;
        }
    }

    /// <summary>
    /// Class voor de tabel van specifieke driver
    /// </summary>
    public class DriverInfo
    {
        public int Laps { get; set; }
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public Boolean Broken { get; set; }

        public DriverInfo(int lapCount, int quality, int performance, int speed, bool broken)
        {
            Laps = lapCount;
            Quality = quality;
            Performance = performance;
            Speed = speed;
            Broken = broken;
        }
    }
}
