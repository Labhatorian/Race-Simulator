using Controller;
using Model;
using System;
using System.Collections;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public IParticipant SelectedDriver { get; set; }
        public Window1(DataContexter dataContexter)
        {
            this.DataContext = dataContexter;
            InitializeComponent();
        }

        public void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CompetitionRow selectedItem = (CompetitionRow)CompetitionList.SelectedItem;
            if (selectedItem != null)
            {
                SelectedDriver = (IParticipant)Data.competition.Participants.Where(s => s.Naam.Equals(selectedItem.Name)).Single();
            } 
        }
    }
}
