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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for AllEventsView.xaml
    /// </summary>
    public partial class AllEventsView : UserControl
    {
        public AllEventsView()
        {
            InitializeComponent();
            using (organizerEntities db = new organizerEntities())
            {
                var allEventsShort = db.Schedule.Include("Event").OrderBy(s => s.TimeStamp).ToList();

                EventList.ItemsSource = allEventsShort;
            }
        }

        public Event SelectedEvent
        {
            get
            {
                return ((Schedule)EventList.SelectedItem).Event;
            }
        }

        private void EventList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)EventList.SelectedItem).Event;
            RecordWindow eventView = ev.GetShowWindow();
            eventView.Show();
        }
    }
}