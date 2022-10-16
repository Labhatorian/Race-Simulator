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
    public class DataContexter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Tabellen en variable voor gegevens competitie en race
        /// </summary>
        /// TODO namen
        public ObservableCollection<CompetitionRow> CompetitionStats { get; set; } = new();
        public ObservableCollection<DriverRow> tableRaceDrivers { get; set; } = new();
        public ObservableCollection<DriverInfo> tableRaceDriverInfo { get; set; } = new();
        public string SelectedDriver;

        /// <summary>
        /// Voor de label bovenaan de scherm. Laat circuitnaam zien
        /// TrackNames wordt niet geupdatet tijdens de competitie
        /// </summary>
        public string trackname { get; set; }
        private List<Track> TrackNames = Data.competition.Tracks.ToList();

        /// <summary>
        /// Begin met als eerst gegevens ophalen zodat er geen leeg label en tabellen zijn bij opstarten
        /// </summary>
        public DataContexter()
        {
            trackname = GetTrackName();
            DataContexterRefresh();
            OnPropertyChanged();
        }

        /// <summary>
        /// Om na elk race opnieuw op te halen
        /// </summary>
        public void DataContexterRefresh()
        {
            Data.CurrentRace.DriversFinished += OnDriverFinished;
            Data.CurrentRace.DriversChanged += OnDriverChanged;  
            UpdateCompetitionInfo();
            UpdateRaceInfoDrivers();
        }

        /// <summary>
        /// Elke keer als iets verandert. Update alle properties, zodat de updates te zien is op het scherm
        /// </summary>
        /// TODO nameof
        private void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("trackname"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CompetitionStats"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tableRaceDriverInfo"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tableRaceDrivers"));
        }

        /// <summary>
        /// Eventhandler die trackname ophaalt. Wordt hier gedaan omdat bij OnDriverFinished niet mogelijk is, de volgende race is nog niet bekend.
        /// Kijkt of bij tabel van race driver is geselecteerd en update de tabel als dat zo is
        /// Update competitie tabel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDriverChanged(object sender, EventArgs e)
        {
            trackname = GetTrackName();
            if (SelectedDriver != null)
            {
                IParticipant driver = (IParticipant)Data.competition.Participants.Where(s => s.Naam.Equals(SelectedDriver)).Single();
                UpdateRaceDriverInfo(driver);
            }
            UpdateCompetitionInfo();
            OnPropertyChanged();
        }

        /// <summary>
        /// Bij einde van race, update competitie info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDriverFinished(object sender, EventArgs e)
        {
            UpdateCompetitionInfo();
            OnPropertyChanged();
        }

        /// <summary>
        /// Haalt trackname op met lambda uit variable
        /// </summary>
        /// <returns></returns>
        private string GetTrackName()
        {
            return TrackNames.Select(x => Data.CurrentRace.Track.Name).First();
        }

        /// <summary>
        /// Haalt naam en punten op van elk coureur in de competitie
        /// </summary>
        private void UpdateCompetitionInfo()
        {
            CompetitionStats = new();
            Data.competition.Participants.Where(s => Data.competition.Participants.Contains(s))
                .ToList()
                .ForEach(i => CompetitionStats.Add(new CompetitionRow(i.Naam, i.Points)));
        }


        /// <summary>
        /// Haalt naam en kleur op van elk coureur in de competitie
        /// </summary>
        public void UpdateRaceInfoDrivers()
    {
        tableRaceDrivers = new();
        Data.competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
            .ToList()
            .ForEach(i => tableRaceDrivers.Add(new DriverRow(i.Naam, i.TeamColor.ToString())));
    }

        /// <summary>
        /// Als een driver is geselecteerd, haal de informatie op van de geselecteerde driver
        /// </summary>
        /// <param name="driver"></param>
        private void UpdateRaceDriverInfo(IParticipant driver)
        {
            tableRaceDriverInfo = new();
            int Lapcount = Race._participantslaps.Where(p => p.Key == driver).Select(p => p.Value).Single();
            Data.competition.Participants.Where(s =>
            {
                return s == driver;
            }).ToList().ForEach(i =>
            {
                tableRaceDriverInfo.Add(new DriverInfo(Lapcount, i.Equipment.Quality, i.Equipment.Performance, i.Equipment.Speed, i.Equipment.IsBroken));
            });
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
