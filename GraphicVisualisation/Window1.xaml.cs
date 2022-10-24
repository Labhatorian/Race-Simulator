using Controller;
using Model;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public IParticipant SelectedDriver { get; set; }
        public Window1(RaceSimDataContext dataContexter)
        {
            this.DataContext = dataContexter;
            InitializeComponent();
        }

        /// <summary>
        /// Selecteert driver in competitielijst voor het spelgedeelte van de sim
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CompetitionRow selectedItem = (CompetitionRow)CompetitionList.SelectedItem;
            if (selectedItem != null)
            {
                SelectedDriver = (IParticipant)Data.Competition.Participants.Where(s => s.Naam.Equals(selectedItem.Name)).Single();
            } 
        }
    }
}
