using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicVisualisation
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string trackname { get; set; }

        public DataContext()
        {
            trackname = Data.CurrentRace.Track.Name;
            PropertyChanged += OnPropertyChanged;
            Data.CurrentRace.DriversChanged += OnDriverChanged;
        }

        private void OnPropertyChanged(object sender, EventArgs e)
        {

        }

        private void OnDriverChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        
    }
}
