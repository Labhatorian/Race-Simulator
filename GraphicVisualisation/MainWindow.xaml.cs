using Controller;
using Model;
using Race_Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using System.Windows.Threading;
using System.Drawing;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
using Brushes = System.Drawing.Brushes;
using System.Xml;

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window1 Window1;
        private Window2 Window2;
        public DataContexter DataContexter { get; set; }

        public MainWindow()
        {

            Data.Initialise();
            DataContexter = new();
            this.DataContext = DataContexter;

            Data.NextRace();
            InitializeComponent();
            
            GraphicalVisualisation.DrawTrack(Data.CurrentRace, Data.CurrentRace.Track, null);
            Data.CurrentRace.DriversChanged += OnDriverChanged;
            Data.CurrentRace.DriversFinished += OnDriversFinished;
        }

        public void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {
            this.MainImage.Dispatcher.BeginInvoke(

                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.MainImage.Source = null;
                    this.MainImage.Source = LoadResources.CreateBitmapSourceFromGdiBitmap(GraphicalVisualisation.DrawTrack(Data.CurrentRace, e.Track, e.Section.SectionType.ToString())); ;
                }));
        }

        public void OnDriversFinished(Object source, EventArgs e)
        {
            Data.CurrentRace = null;
            Data.NextRace();

            //
            LoadResources.Clear();
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriverChanged;
                Data.CurrentRace.DriversFinished += OnDriversFinished;
                GraphicalVisualisation.DrawTrack(Data.CurrentRace, Data.CurrentRace.Track, null);
                Data.CurrentRace.DriversChanged += DataContexter.OnDriverChanged;
                Data.CurrentRace.DriversFinished += DataContexter.OnDriverFinished;
            } else
            {
                //niks
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Window2_Click(object sender, RoutedEventArgs e)
        {
            Window2 = new Window2();
            Window2.Show();
        }

        private void MenuItem_Window1_Click(object sender, RoutedEventArgs e)
        {
            Window1 = new Window1();
            Window1.Show();
        }
    }
}
