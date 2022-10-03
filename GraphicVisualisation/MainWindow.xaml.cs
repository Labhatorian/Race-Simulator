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

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.Initialise();
            Data.NextRace();

            Data.CurrentRace.DriversChanged += OnDriverChanged;
        }

        public void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {
            this.MainImage.Dispatcher.BeginInvoke(

                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.MainImage.Source = null;
                    this.MainImage.Source = LoadResources.CreateBitmapSourceFromGdiBitmap(GraphicalVisualisation.DrawTrack(e.Track, e.Section.SectionType.ToString())); ;
                }));
        }
    }
}
