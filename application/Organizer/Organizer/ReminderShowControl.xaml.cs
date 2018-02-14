using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Форма для показа подробностей напоминания
    /// <summary>
    /// Interaction logic for ReminderShowControl.xaml
    /// </summary>
    public partial class ReminderShowControl : UserControl
    {
        public ReminderShowControl()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow edit = ((Reminder)DataContext).GetEditWindow();
            if (edit.ShowDialog() == true)
            {
                Window.GetWindow(this).DialogResult = true;
                InitializeComponent();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить запись?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Window.GetWindow(this).DialogResult = true;
                using (organizerEntities db = new organizerEntities())
                {
                    Reminder reminder = new Reminder { Id = ((Reminder)DataContext).Id };
                    db.Event.Attach(reminder);
                    db.Event.Remove(reminder);
                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
