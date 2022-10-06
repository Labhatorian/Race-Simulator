using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GraphicVisualisation
{
    public class DataContexter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //Competitie info
        System.Data.DataTable table = new DataTable("Competitie");
        private System.Data.DataSet dataSet;

        public string trackname { get; set; }
        private List<Track> TrackNames = new List<Track>(Data.competition.Tracks.ToList());
        public DataContexter()
        {
            //trackname = GetTrackName();
            PropertyChanged += OnPropertyChanged;
            Data.CurrentRace.DriversChanged += OnDriverChanged;
            Data.CurrentRace.DriversFinished += OnDriverFinished;
        }
        
        
        private void OnPropertyChanged(object sender, EventArgs e)
        {
            string test = GetTrackName();
            trackname = test;
        }

        public void OnDriverChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(trackname));
        }

        public void OnDriverFinished(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(trackname));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Window1.));
        }

        private string GetTrackName()
        {
            //Liever niet zo, maar moet van opdracht
            return TrackNames.Select(x => Data.CurrentRace.Track.Name).First();
        }

        private void MakeCompetitionInfo()
        {
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType,
            // ColumnName and add to DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Drivers";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Create second column.
            //column = new DataColumn();
            //column.DataType = System.Type.GetType("System.String");
            //column.ColumnName = "Points";
            //column.AutoIncrement = false;
            //column.Caption = "Punten";
            //column.ReadOnly = false;
            //column.Unique = false;
            //// Add the column to the table.
            //table.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["Drivers"];
            table.PrimaryKey = PrimaryKeyColumns;

            // Instantiate the DataSet variable.
            dataSet = new DataSet();
            // Add the new DataTable to the DataSet.
            dataSet.Tables.Add(table);

            // Create three new DataRow objects and add
            // them to the DataTable
            foreach(IParticipant participant in Data.competition.Participants)
            {
                row = table.NewRow();
                row["Drivers"] = participant.Naam;
                row["Points"] = participant.Points;
                table.Rows.Add(row);
            }
        }

        private void UpdateCompetitionInfo()
        {
            foreach (DataRow dr in table.Rows) // search whole table
            {
                foreach (IParticipant participant in Data.competition.Participants)
                {
                    if (dr["Driver"].Equals(participant.Naam)) // if id==2
                    {
                        dr["Points"] = participant.Points; //change the name
                                                    //break; break or not depending on you
                    }
                }
            }
        }

    }
}
