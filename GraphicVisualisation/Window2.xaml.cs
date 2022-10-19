using System.Windows;
using System.Windows.Controls;

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private RaceSimDataContext dataContexter;
        public Window2(RaceSimDataContext dataContext)
        {
            this.dataContexter = dataContext;
            this.DataContext = dataContext;
            InitializeComponent();
        }

        /// <summary>
        /// Eventhandler die bij listview selecteren, de selectie doorgeeft aan de datacontext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DriverRow selectedItem = (DriverRow)DriverList.SelectedItem;
            if (selectedItem != null)
            {
                dataContexter.SelectedDriver = selectedItem.Naam;
            }
        }
    }
}
