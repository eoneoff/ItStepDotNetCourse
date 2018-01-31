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
using MoreLinq;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainView;

        private Control view;

        public MainWindow()
        {
            MainView = this;

            InitializeComponent();
            ViewModePicker.SelectedIndex = 0;
            using (organizerEntities db = new organizerEntities())
            {
                var top5 = db.Schedule.Include("Event").Where(s => s.TimeStamp > DateTime.Now).OrderBy(s => s.TimeStamp).DistinctBy(s => s.Event).Take(5).ToList();
                foreach (var s in top5)
                {
                    if (s.Event.EventType == "Job")
                    {
                        if (!db.Entry((Job)s.Event).Reference(j => j.Start).IsLoaded)
                            db.Entry((Job)s.Event).Reference(j => j.Start).Load();
                    }

                    else
                    {
                        if (s.Event.EventType == "Meeting")
                        {
                            if (!db.Entry((Meeting)s.Event).Reference(m => m.Start).IsLoaded)
                                db.Entry((Meeting)s.Event).Reference(m => m.Start).Load();
                        }
                    }
                }
                Top5Events.ItemsSource = top5;
            }
        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetailedDayView day = new DetailedDayView();
            day.CurrentDate.SelectedDate = PreviewCalendar.SelectedDate;
            day.Show();
        }

        private void Top5Events_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)Top5Events.SelectedItem).Event;
            RecordWindow window = ev.GetShowWindow();      
            window.Show();
        }

        private void ViewModePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainPanel.Children.Remove(view);

            switch (ViewModePicker.SelectedIndex)
            {
                case 0:
                    view = new AllEventsView();
                    break;
                case 1:
                    view = new OneDayViewControl();
                    Binding b = new Binding("SelectedDate");
                    b.Source = CurrentDate;
                    b.Mode = BindingMode.TwoWay;
                    view.SetBinding(OneDayViewControl.CurrentDateProperty, b);
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            Grid.SetRow(view, 1);
            MainPanel.Children.Add(view);
        }
    }
}
