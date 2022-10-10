using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static GraphicVisualisation.DataContexter;

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private DataContexter dataContexter;
        public static event TableChanged FinishAuto;
        public Window2(DataContexter dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
           //DriverList.ItemsSource = dataContext.tableRaceDrivers;
            //DriverInfo.ItemsSource = dataContext.tableRaceDriverInfo.DefaultView;
        }

        public void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedItem = DriverList.SelectedItem;
            dataContexter.SelectedDriver = selectedItem[0];
        }
    }
}
