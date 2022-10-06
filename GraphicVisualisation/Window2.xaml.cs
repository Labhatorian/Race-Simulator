using Model;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private DataContexter dataContexter;
        public Window2(DataContexter dataContext)
        {
            this.dataContexter = dataContext;
            InitializeComponent();
            DriverList.ItemsSource = dataContexter.tableRaceDrivers.DefaultView;
            DriverInfo.ItemsSource = dataContexter.tableRaceDriverInfo.DefaultView;
        }

        private void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataContexter.SelectedDriver = (string)DriverList.SelectedItems[0];
        }
    }
}
