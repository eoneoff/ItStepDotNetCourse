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
                using (organizerEntities db = new organizerEntities())
                {
                    Holiday holiday = new Holiday { Id = ((Holiday)DataContext).Id };
                    db.Event.Attach(holiday);
                    db.Event.Remove(holiday);
                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
