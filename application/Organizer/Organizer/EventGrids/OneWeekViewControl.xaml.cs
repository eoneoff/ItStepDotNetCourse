using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Organizer
{
    ///Показ событий одной недели
    /// <summary>
    /// Interaction logic for OneWeekViewControl.xaml
    /// </summary>
    public partial class OneWeekViewControl : UserControl
    {
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime?), typeof(OneWeekViewControl),
                new PropertyMetadata(default(DateTime?), new PropertyChangedCallback(CurrentDateChanged)));

        public DateTime? CurrentDate
        {
            get { return (DateTime?)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        public Event SelectedEvent
        {
            get
            {
                return ((Schedule)EventList.SelectedItem).Event;
            }
        }

        public OneWeekViewControl()
        {
            InitializeComponent();
            getEvents();
            OnCalendarClick();
            MainWindow.MainView.CalendarClick += OnCalendarClick;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = ((DateTime)CurrentDate).AddDays(-7);
            getEvents();

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = ((DateTime)CurrentDate).AddDays(7);
            getEvents();
        }

        private void getEvents()
        {
            DateTime date;
            if (CurrentDate == null)
                date = (DateTime)MainWindow.MainView.CurrentDate.SelectedDate;
            else
                date = (DateTime)CurrentDate;

            DateTime uppepBound = date.Date;

            while (uppepBound.DayOfWeek != DayOfWeek.Monday)
                uppepBound = uppepBound.AddDays(-1);

            uppepBound = uppepBound.Date;
            DateTime lowerBound = uppepBound.AddDays(7);

            using (organizerEntities db = new organizerEntities())
            {
                var events = db.Schedule.
                    Include("Event").
                    Where(t => t.TimeStamp >= uppepBound && t.TimeStamp < lowerBound).
                    OrderBy(t => t.TimeStamp).ToList();

                switch (MainWindow.MainView.DoneMode.SelectedIndex)
                {
                    case 1:
                        events = events.Where(s => s.Event.Done == false).ToList();
                        break;
                    case 2:
                        events = events.Where(s => s.Event.Done == true).ToList();
                        break;
                }

                EventList.ItemsSource = events;
            }
            EventList.Items.Refresh();
        }

        private static void CurrentDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            ((OneWeekViewControl)sender).getEvents();
        }

        private void EventList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)EventList.SelectedItem).Event;
            RecordWindow eventView = ev.GetShowWindow();
            if (eventView.ShowDialog() == true)
                getEvents();
        }

        private void OnCalendarClick()
        {
            List<Schedule> events = (List<Schedule>)EventList.ItemsSource;
            Schedule selected = events.Where(s => s.TimeStamp >= ((DateTime)MainWindow.MainView.CurrentDate.SelectedDate).Date).FirstOrDefault();
            if (selected != null)
            {
                EventList.SelectedItem = selected;
                EventList.Focus();
            }
        }
    }
}
