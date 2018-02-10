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
    /// Interaction logic for AllEventsView.xaml
    /// </summary>
    public partial class AllEventsView : UserControl
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public AllEventsView()
        {
            InitializeComponent();
            getEvents();
            MainWindow.MainView.CalendarClick += OnCalendarClick;
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
            }
        }

        private void getEvents()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var allEventsShort = db.Schedule.Include("Event").Where(s => s.TimeStamp >= DateTime.Today).OrderBy(s => s.TimeStamp).ToList();
                EventList.ItemsSource = allEventsShort;
                EventList.Items.Refresh();
                OnCalendarClick();
            }
        }
    }
}
