using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GraphicVisualisation
{
    public class DataContexter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CompetitionRow> table { get; set; } = new();

        public ObservableCollection<DriverRow> tableRaceDrivers { get; set; } = new();
        public ObservableCollection<DriverInfo> tableRaceDriverInfo { get; set; } = new();
        public string SelectedDriver;

        public static Window2 win2;

        public string trackname { get; set; }
        private List<Track> TrackNames = Data.competition.Tracks.ToList();

        public DataContexter()
        {
            trackname = GetTrackName();
            DataContexterRefresh();
            OnPropertyChanged();
        }

        public void DataContexterRefresh()
        {
            Data.CurrentRace.DriversFinished += OnDriverFinished;
            Data.CurrentRace.DriversChanged += OnDriverChanged;  
            UpdateCompetitionInfo();
            UpdateRaceInfoDrivers();
        }

        private void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("trackname"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("table"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tableRaceDriverInfo"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tableRaceDrivers"));
        }

        public void OnDriverChanged(object sender, EventArgs e)
        {
            //TODO Probeer dit naar ander plek te zetten
            trackname = GetTrackName();
            if (SelectedDriver != null)
            {
                IParticipant driver = (IParticipant)Data.competition.Participants.Where(s => s.Naam.Equals(SelectedDriver)).Single();
                UpdateRaceDriverInfo(driver);
            }
            UpdateCompetitionInfo();
            OnPropertyChanged();
        }

        public void OnDriverFinished(object sender, EventArgs e)
        {
            UpdateCompetitionInfo();
            OnPropertyChanged();
        }


        private string GetTrackName()
        {
            return TrackNames.Select(x => Data.CurrentRace.Track.Name).First();
        }


        private void UpdateCompetitionInfo()
        {
            table = new();
            Data.competition.Participants.Where(s => Data.competition.Participants.Contains(s))
                .ToList()
                .ForEach(i => table.Add(new CompetitionRow(i.Naam, i.Points)));
        }



    public void UpdateRaceInfoDrivers()
    {
        tableRaceDrivers = new();
        Data.competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
            .ToList()
            .ForEach(i => tableRaceDrivers.Add(new DriverRow(i.Naam, i.TeamColor.ToString())));
    }


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

    public class DriverInfo
    {
        public int LapCount { get; set; }
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public Boolean Broken { get; set; }

        public DriverInfo(int lapCount, int quality, int performance, int speed, bool broken)
        {
            LapCount = lapCount;
            Quality = quality;
            Performance = performance;
            Speed = speed;
            Broken = broken;
        }
    }
}
