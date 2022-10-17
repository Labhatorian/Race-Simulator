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
        /// <summary>
        /// De nieuwe Main(). Maak competitie en start volgende race.
        /// </summary>
        public MainWindow()
        { 
            Data.Initialise();
            Data.NextRace();
            InitializeComponent();
            DataContexter = (DataContexter)this.DataContext;

            Data.CurrentRace.DriversChanged += OnDriverChanged;
            Data.CurrentRace.DriversFinished += OnDriversFinished;  
        }

        /// <summary>
        /// Elke keer dat driver changed. Haal nieuwe Bitmap op en laat dat zien op MainImage
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Als race klaar is. Ga naar volgend race en laat anders geen circuit zien maar wel window met competitiegegevens
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public async void OnDriversFinished(Object source, EventArgs e)
        {
            Data.CurrentRace = null;
            //Window1.Close();
            //Window2.Close();
            Data.NextRace();

            LoadResources.Clear();
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriverChanged;
                Data.CurrentRace.DriversFinished += OnDriversFinished;
                GraphicalVisualisation.DrawTrack(Data.CurrentRace, Data.CurrentRace.Track, null);
                DataContexter.DataContexterRefresh();
            } else
            {
                SectionTypes[] sectionTypesEmptyTrack = new SectionTypes[0];
                Track EmptyTrack = new Track("", sectionTypesEmptyTrack);
                
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    this.MainImage.Source = null;
                    this.MainImage.Source = LoadResources.CreateBitmapSourceFromGdiBitmap(GraphicalVisualisation.DrawTrack(null, EmptyTrack, null));
                    Window1 = new Window1(DataContexter);
                    Window1.Owner = this;
                    Window1.Show();
                });
            }
        }

        /// <summary>
        /// Eventhandler voor menubutton sluiten. Sluit het programma af
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Eventhandler voor menubutton Competitieinfo. Laat window ziet met competitie info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Window2_Click(object sender, RoutedEventArgs e)
        {
            Window2 = new Window2(DataContexter);
            Window2.Owner = this;
            Window2.Show();
        }

        /// <summary>
        /// Eventhandler voor menubutton Raceinfo. Laat window ziet met race info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Window1_Click(object sender, RoutedEventArgs e)
        {
            Window1 = new Window1(DataContexter);
            Window1.Owner = this;
            Window1.Show();
        }

        private void AddDistanceToDriver(object sender, RoutedEventArgs e)
        {
            if (Window1 is not null)
            {
                if (Window1.SelectedDriver is not null)
                {
                    Window1.SelectedDriver.Equipment.UserAddedDistance = true;
                }
            }
        }
    }
}
