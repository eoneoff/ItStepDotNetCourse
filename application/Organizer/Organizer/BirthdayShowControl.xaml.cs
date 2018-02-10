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
    /// Interaction logic for BirthdayShowControl.xaml
    /// </summary>
    public partial class BirthdayShowControl : UserControl
    {
        public BirthdayShowControl()
        {
            InitializeComponent();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow edit = ((Birthday)DataContext).GetEditWindow();
            if (edit.ShowDialog() == true)
            {
                InitializeComponent();
                Window.GetWindow(this).DialogResult = true;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Window.GetWindow(this).DialogResult = true;
                using (organizerEntities db = new organizerEntities())
                {
                    Birthday birthday = new Birthday { Id = ((Birthday)DataContext).Id };
                    db.Event.Attach(birthday);
                    db.Event.Remove(birthday);
                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                } 
            }
        }
    }
}
