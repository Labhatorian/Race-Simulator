using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Func<string, string> CircuitName = (CircuitName => Data.CurrentRace.Track.Name);

        public DataContext()
        {
            PropertyChanged += OnPropertyChanged;
            Data.CurrentRace.DriversChanged += OnDriverChanged;
        }

        private void OnPropertyChanged(object sender, EventArgs e)
        {

        }

        private void OnDriverChanged(object sender, EventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        
    }
}
