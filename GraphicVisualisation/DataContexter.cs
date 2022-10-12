using Controller;
using Model;
using System;
using System.Collections.Generic;
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
        public delegate void TableChanged(ListView listView);

        public System.Data.DataTable table = new() ;

        public System.Data.DataTable tableRaceDrivers = new();
        public System.Data.DataTable tableRaceDriverInfo = new();
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
            //Window2.FinishAuto += OnTableChanged;   
            UpdateCompetitionInfo();
            UpdateRaceInfoDrivers();
        }

        private void OnTableChanged(ListView listView)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("trackname"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Laps"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quality"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Performance"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Speed"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Broken"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tableRaceDriverInfo"));
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
            table = new DataTable("Competitie");
            table.Columns.Add("Name");
            table.Columns.Add("Points");
            Data.competition.Participants.Where(s => Data.competition.Participants.Contains(s))
                .ToList()
                .ForEach(i => table.Rows.Add(i.Naam, i.Points));
        
    }

    public void UpdateRaceInfoDrivers()
    {
        tableRaceDrivers = new DataTable("Drivers");
        tableRaceDrivers.Columns.Add("Naam");
        tableRaceDrivers.Columns.Add("TeamColour");
        Data.competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
            .ToList()
            .ForEach(i => tableRaceDrivers.Rows.Add(i.Naam, i.TeamColor.ToString()));
    }


        private void UpdateRaceDriverInfo(IParticipant driver)
        {
            tableRaceDriverInfo = new DataTable("Competitie");
            tableRaceDriverInfo.Columns.Add("Laps");
            tableRaceDriverInfo.Columns.Add("Quality");
            tableRaceDriverInfo.Columns.Add("Performance");
            tableRaceDriverInfo.Columns.Add("Speed");
            tableRaceDriverInfo.Columns.Add("Broken");
            int Lapcount = Race._participantslaps.Where(p => p.Key == driver).Select(p => p.Value).Single();
            Data.competition.Participants.Where(s =>
            {
                return s == driver;
            }).ToList().ForEach(i =>
            {
                tableRaceDriverInfo.Rows.Add(Lapcount, i.Equipment.Quality, i.Equipment.Performance, i.Equipment.Speed, i.Equipment.IsBroken);
            });
        }
    }
}
