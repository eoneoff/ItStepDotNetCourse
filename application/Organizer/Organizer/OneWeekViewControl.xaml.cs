using System;
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
    /// Interaction logic for OneWeekViewControl.xaml
    /// </summary>
    public partial class OneWeekViewControl : UserControl
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

        public OneWeekViewControl()
        {
            InitializeComponent();
            getEvents();
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
                EventList.ItemsSource = events;
            }
            EventList.Items.Refresh();
        }

        private static void CurrentDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            ((OneWeekViewControl)sender).getEvents();
        }
    }
}
