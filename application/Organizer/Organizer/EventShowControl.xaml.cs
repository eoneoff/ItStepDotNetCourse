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
    /// Interaction logic for EventShowControl.xaml
    /// </summary>
    public partial class EventShowControl : UserControl
    {
        public EventShowControl()
        {
            InitializeComponent();
        }

        private void DoneCheckBox_Click(object sender, RoutedEventArgs e)
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
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Attach(ev);
                    db.Entry(ev).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    InitializeComponent();
                }
            }
            else
            {
                DoneCheckBox.IsChecked = !DoneCheckBox.IsChecked;
            }
        }

        private void Alarms_Click(object sender, RoutedEventArgs e)
        {
            Event ev = null;
            using (organizerEntities db = new organizerEntities())
            {
                ev = (Event)DataContext;
                db.Event.Attach(ev);
                db.Entry(ev).Collection("Alarm").Load();
            }

            ShowAlarmControl showAlarms = new ShowAlarmControl();
            showAlarms.DataContext = ev;
            Grid.SetRow(showAlarms, 1);
            Grid.SetColumnSpan(showAlarms, 3);
            RecordWindow window = new RecordWindow();
            window.Win.Children.Add(showAlarms);
            window.ShowDialog();
        }
    }
}
