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
    ///Форма для показа будильников определенного события
    /// <summary>
    /// Interaction logic for ShowAlarmControl.xaml
    /// </summary>
    public partial class ShowAlarmControl : UserControl
    {
        public ShowAlarmControl()
        {
            InitializeComponent();
        }

        private void NewAlarm_Click(object sender, RoutedEventArgs e)
        {
            CreateAlarmControl create = new CreateAlarmControl();
            int id = ((Event)DataContext).Id;
            DateTime eventTime = DateTime.Now;
            using (organizerEntities db = new organizerEntities())
            {
                eventTime = db.Schedule.Where(s => s.Event.Id == id).First().TimeStamp;
            }


            Grid.SetRow(create, 0);
            Grid.SetColumnSpan(create, 2);
            create.DataContext = DataContext;
            create.SelectedDateTime = eventTime;

            RecordWindow window = new RecordWindow();
            window.Title = "Новый будильник";
            window.Height = 200;
            window.Win.Children.Add(create);

            if(window.ShowDialog()==true)
            {
                Event ev = (Event)DataContext;
                DateTime alarmTime = (DateTime)create.SelectedDateTime;
                Alarm alarm = null;
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Attach(ev);
                    alarm = db.Alarm.Where(a => a.AlarmTriggerTime == alarmTime).FirstOrDefault();
                    if(alarm==null)
                    {
                        alarm = new Alarm() { AlarmTriggerTime = alarmTime };
                    }

                    db.Alarm.Attach(alarm);

                    ev.Alarm.Add(alarm);

                    db.Entry(ev).State = System.Data.Entity.EntityState.Modified;

                    db.Entry(alarm).State = alarm.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }

                DataContext = ev;

                Alarms.Items.Refresh();
            }
        }

        private void DeleteAlarm_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить напоминание?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Alarm alarm = (Alarm)Alarms.SelectedItem;
                Event ev = (Event)DataContext;

                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Attach(ev);
                    db.Alarm.Attach(alarm);

                    ev.Alarm.Remove(alarm);

                    db.Entry(ev).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(alarm).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }

                DataContext = ev;

                Alarms.Items.Refresh(); 
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
