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
    /// Interaction logic for ReminderEditControl.xaml
    /// </summary>
    public partial class ReminderEditControl : UserControl
    {
        public ReminderEditControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = (Reminder)DataContext;
            if (reminder.Name==String.Empty||DateTimePicker.SelectedDateTime==null)
            {
                MessageBox.Show("Заполните обязательне поля(название и время)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (DateTime.Now<DateTimePicker.SelectedDateTime||
                MessageBox.Show("Вы точно хотите создать напоминание в прошедшем времени?","Вы уверены",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Add(reminder);
                    db.SaveChanges();
                    Window.GetWindow(this).Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
