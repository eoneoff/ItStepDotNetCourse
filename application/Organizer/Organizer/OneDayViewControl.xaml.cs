using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Показ событий одного дня
    /// <summary>
    /// Interaction logic for OneDayViewControl.xaml
    /// </summary>
    public partial class OneDayViewControl : UserControl
    {
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime?), typeof(OneDayViewControl),
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

        public OneDayViewControl()
        {
            InitializeComponent();
            getEvents();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate=((DateTime)CurrentDate).AddDays(-1);
            getEvents();
            
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = ((DateTime)CurrentDate).AddDays(1);
            getEvents();
        }

        private void getEvents()
        {
            DateTime date;
            if (CurrentDate == null)
                date = (DateTime)MainWindow.MainView.CurrentDate.SelectedDate;
            else
                date = (DateTime)CurrentDate;

            using (organizerEntities db = new organizerEntities())
            {
                DateTime uppepBound = date.Date;
                DateTime lowerBound = uppepBound.AddDays(1);
                var events = db.Schedule.
                    Include("Event").
                    Where(t => t.TimeStamp>=uppepBound && t.TimeStamp<lowerBound).
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
            
            ((OneDayViewControl)sender).getEvents();
        }

        private void EventList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)EventList.SelectedItem).Event;
            RecordWindow eventView = ev.GetShowWindow();
            if (eventView.ShowDialog() == true)
                getEvents();
        }
    }
}
