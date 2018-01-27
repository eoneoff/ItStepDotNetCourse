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
        public MainWindow()
        {
            InitializeComponent();
            using (organizerEntities db = new organizerEntities())
            {
                var top5 = db.Schedule.Include("Event").Where(s => s.TimeStamp > DateTime.Now).OrderBy(s => s.TimeStamp).DistinctBy(s => s.Event).Take(5).ToList();
                foreach (var s in top5)
                {
                    if (Accessories.GetEventType(s.Event) == "Job")
                    {
                        if (!db.Entry((Job)s.Event).Reference(j => j.Start).IsLoaded)
                            db.Entry((Job)s.Event).Reference(j => j.Start).Load();
                    }

                    else
                    {
                        if (Accessories.GetEventType(s.Event) == "Meeting")
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

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            using (organizerEntities db = new organizerEntities())
            {
                var jobs = db.Event.OfType<Job>().Select(j=>j);
                foreach(Job j in jobs)
                {
                    RecordWindow window = new RecordWindow();
                    JobShowControl c = new JobShowControl();
                    Grid.SetRow(c, 0);
                    window.Win.Children.Add(c);
                    window.DataContext = j;
                    window.Show();
                }
            }
        }

        private void Top5Events_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)Top5Events.SelectedItem).Event;
            RecordWindow window = new RecordWindow();
            Control showControl = Accessories.EventShowControlFactoryMethod(ev);
            Grid.SetRow(showControl, 0);
            window.Win.Children.Add(showControl);
            window.DataContext = ev;

            switch(Accessories.GetEventType(ev))
            {
                case "Birthday":
                    window.Title = $"День рождения {ev.Name}";
                    window.Height = 325;
                    break;
                case "Holiday":
                    window.Title = $"Праздник {ev.Name}";
                    window.Height = 325;
                    break;
                case "Job":
                    window.Title = $"Задание {ev.Name}";
                    window.Height = 325;
                    break;
                case "Meeting":
                    window.Title = $"Встреча {ev.Name}";
                    window.Height = 385;
                    break;
                case "Reminder":
                    window.Title = $"Напоминание {ev.Name}";
                    window.Height = 350;
                    break;
            }

            window.Show();
        }
    }
}
