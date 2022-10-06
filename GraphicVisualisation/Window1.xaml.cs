﻿using Controller;
using Model;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace GraphicVisualisation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            List<IParticipant> results = Data.competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s)).ToList();

            // 3. Query execution.
            foreach (IParticipant participant in results)
            {
                var row = new { Naam = participant.Naam, Points = participant.Points };
                CompetitionList.Items.Add(row);
            }
            
            //TODO Sorteer op punten
        }
    }
}
