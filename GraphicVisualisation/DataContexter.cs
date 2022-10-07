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
using System.Windows.Documents;

namespace GraphicVisualisation
{
    public class DataContexter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public System.Data.DataTable table = new() ;

        public System.Data.DataTable tableRaceDrivers = new();
        public System.Data.DataTable tableRaceDriverInfo = new();
        public string SelectedDriver;
        static int debug = 0;

        public string trackname { get; set; }
        private List<Track> TrackNames = Data.competition.Tracks.ToList();

        public DataContexter()
        {
            Data.CurrentRace.DriversFinished += OnDriverFinished;
            PropertyChanged += OnPropertyChanged;
            UpdateCompetitionInfo();
            UpdateRaceInfoDrivers();
        }
        
        
        private void OnPropertyChanged(object sender, EventArgs e)
        {
            trackname = GetTrackName();
            UpdateCompetitionInfo();
            if (SelectedDriver != null)
            {
                IParticipant driver = (IParticipant)Data.competition.Participants.Where(s => s.Naam.Equals(SelectedDriver)).Single();
                UpdateRaceDriverInfo(driver);
            }
        }

        //TODO Dit niet gebruiken voor performance. Vervangen met click event van de form 
        public void OnDriverChanged(object sender, EventArgs e)
        {
           // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tableRaceDriverInfo"));
        }

        public void OnDriverFinished(object sender, EventArgs e)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("trackname"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("table"));
        }

        
        private string GetTrackName()
        { 
            if (debug == 0)
            {
                debug += 1;
                return TrackNames.Select(x => Data.CurrentRace.Track.Name).First();
            } else
            {
                return "Test";
            }
            
        }

       
        private void UpdateCompetitionInfo()
        {
            table = new DataTable("Competitie");
            table.Columns.Add("Name");
            table.Columns.Add("Points");
            Data.competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
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
            tableRaceDriverInfo.Columns.Add("LapCount");
            tableRaceDriverInfo.Columns.Add("Quality");
            tableRaceDriverInfo.Columns.Add("Performance");
            tableRaceDriverInfo.Columns.Add("Speed");
            tableRaceDriverInfo.Columns.Add("Broken");
            int Lapcount = Race._participantslaps.Where(p => p.Key == driver).Select(p => p.Value).Single();
            Data.competition.Participants.Where(s =>
            {
                return Data.CurrentRace.Participants.Contains(driver);
            }).ToList().ForEach(i =>
            {
                tableRaceDriverInfo.Rows.Add(Lapcount, i.Equipment.Quality, i.Equipment.Performance, i.Equipment.Speed, i.Equipment.IsBroken);
            });
        }
    }
}
