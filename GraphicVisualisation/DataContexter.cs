using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphicVisualisation
{
    public class DataContexter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string trackname { get; set; }
        private List<Track> TrackNames = new List<Track>(Data.competition.Tracks.ToList());
        public DataContexter()
        {
            //trackname = GetTrackName();
            PropertyChanged += OnPropertyChanged;
            Data.CurrentRace.DriversChanged += OnDriverChanged;
            Data.CurrentRace.DriversFinished += OnDriverFinished;
        }
        
        
        private void OnPropertyChanged(object sender, EventArgs e)
        {
            string test = GetTrackName();
            trackname = test;
        }

        public void OnDriverChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(trackname));
        }

        public void OnDriverFinished(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(trackname));
        }

        private string GetTrackName()
        {
            //Liever niet zo, maar moet van opdracht
            return TrackNames.Select(x => Data.CurrentRace.Track.Name).First();
        }
        
    }
}
