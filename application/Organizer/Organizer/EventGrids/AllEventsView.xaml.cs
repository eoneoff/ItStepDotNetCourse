using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;

namespace Organizer
{
    /// Вывод всех событий из календаря
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

        private async void EventList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)EventList.SelectedItem).Event;
            RecordWindow eventView = ev.GetShowWindow();
            if (eventView.ShowDialog() == true)
                await getEvents();
        }

        //Выделение события в зависимости от даты, выделенной на календате в боковой панели
        private void OnCalendarClick()
        {
            List<Schedule> events = (List<Schedule>)EventList.ItemsSource;
            Schedule selected = events.Where(s => s.TimeStamp >= ((DateTime)MainWindow.MainView.CurrentDate.SelectedDate).Date).FirstOrDefault();
            if (selected != null)
            {
                EventList.SelectedItem = selected;
            }
        }

        //Получение списка событий в зависимости от выбранных в главном окне пунктов
        private async Task getEvents()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var allEventsShort = await db.Schedule.Include("Event").OrderBy(s => s.TimeStamp).ToListAsync();
                if (MainWindow.MainView.DoneMode!=null)
                {
                    switch (MainWindow.MainView.DoneMode.SelectedIndex)
                    {
                        case 1:
                            allEventsShort = allEventsShort.Where(s => s.Event.Done == false).ToList();
                            break;
                        case 2:
                            allEventsShort = allEventsShort.Where(s => s.Event.Done == true).ToList();
                            break;
                    } 
                }
                EventList.ItemsSource = allEventsShort;
                EventList.Items.Refresh();
                OnCalendarClick();
            }
        }
    }
}
