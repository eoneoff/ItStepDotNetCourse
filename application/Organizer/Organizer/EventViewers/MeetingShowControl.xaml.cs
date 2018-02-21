using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Отображение подробностей встречи
    /// <summary>
    /// Interaction logic for MeetingShowControl.xaml
    /// </summary>
    public partial class MeetingShowControl : UserControl
    {
        public MeetingShowControl()
        {
            InitializeComponent();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow edit = ((Meeting)DataContext).GetEditWindow();
            if (edit.ShowDialog() == true)
            {
                Window.GetWindow(this).DialogResult = true;
                InitializeComponent();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить запись?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Window.GetWindow(this).DialogResult = true;
                using (organizerEntities db = new organizerEntities())
                {
                    Meeting meeting = new Meeting { Id = ((Meeting)DataContext).Id };
                    db.Event.Attach(meeting);
                    db.Event.Remove(meeting);
                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                }

                MainWindow.MainView.UpdateView();
            }
        }
    }
}
