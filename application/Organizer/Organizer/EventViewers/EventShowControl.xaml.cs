using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Organizer
{
    ///Контрол для показа свойств, общих для всех событий
    /// <summary>
    /// Interaction logic for EventShowControl.xaml
    /// </summary>
    public partial class EventShowControl : UserControl
    {
        public EventShowControl()
        {
            InitializeComponent();
        }

        //Отметка события выполненным/не выполненным с проверкой и сохранением
        private async void DoneCheckBox_Click(object sender, RoutedEventArgs e)
        {
            string question = String.Empty;
            if (DoneCheckBox.IsChecked == true)
                question = "Хотите отметить это событие как выполненное?";
            else
                question = "Хотите отметить это событие как не выполненное?";

            if (MessageBox.Show(question,"Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Event ev = (Event) DataContext;
                ev.Done = !ev.Done;
                Window.GetWindow(this).DialogResult = true;
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Attach(ev);
                    db.Entry(ev).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    InitializeComponent();
                }
            }
            else
            {
                DoneCheckBox.IsChecked = !DoneCheckBox.IsChecked;
            }
        }

        private async void Alarms_Click(object sender, RoutedEventArgs e)
        {
            Event ev = null;
            using (organizerEntities db = new organizerEntities())
            {
                ev = (Event)DataContext;
                db.Event.Attach(ev);
                await db.Entry(ev).Collection("Alarm").LoadAsync();
            }

            ShowAlarmControl showAlarms = new ShowAlarmControl();
            showAlarms.DataContext = ev;
            Grid.SetRow(showAlarms, 1);
            Grid.SetColumnSpan(showAlarms, 3);
            RecordWindow window = new RecordWindow();
            window.Win.Children.Add(showAlarms);
            window.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Images/alarmicon.ico"));
            window.ShowDialog();
        }
    }
}
