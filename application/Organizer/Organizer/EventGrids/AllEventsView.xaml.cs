using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void EventList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)EventList.SelectedItem).Event;
            RecordWindow eventView = ev.GetShowWindow();
            if (eventView.ShowDialog() == true)
                getEvents();
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
        private void getEvents()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var allEventsShort = db.Schedule.Include("Event").OrderBy(s => s.TimeStamp).ToList();
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
