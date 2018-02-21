using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Форма для показа подробностей праздника
    /// <summary>
    /// Interaction logic for HolidayShowControl.xaml
    /// </summary>
    public partial class HolidayShowControl : UserControl
    {
        public HolidayShowControl()
        {
            InitializeComponent();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow edit = ((Holiday)DataContext).GetEditWindow();
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
                Window.GetWindow(this).Close();
                using (organizerEntities db = new organizerEntities())
                {
                    Holiday holiday = new Holiday { Id = ((Holiday)DataContext).Id };
                    db.Event.Attach(holiday);
                    db.Event.Remove(holiday);
                    await db.SaveChangesAsync();
                }

                MainWindow.MainView.UpdateView();
            }
        }
    }
}
