﻿using System;
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

namespace Organizer
{
    /// <summary>
    /// Interaction logic for DetailedDayView.xaml
    /// </summary>
    public partial class DetailedDayView : Window
    {
        public DetailedDayView()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            CreateNewEvent create = new CreateNewEvent();
            create.Date.SelectedDate = CurrentDate.SelectedDate;
            create.Show();
        }
    }
}
