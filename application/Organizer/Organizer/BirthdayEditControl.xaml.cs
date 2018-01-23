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
    /// Interaction logic for BirthdayEditControl.xaml
    /// </summary>
    public partial class BirthdayEditControl : UserControl
    {
        public BirthdayEditControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Birthday birthday = DataContext as Birthday;
            if(birthday.Name==String.Empty||BirthDate.SelectedDate==null)
            {
                MessageBox.Show("Введите обязательные параметры (имя и дата рождения)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Add(birthday);
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
