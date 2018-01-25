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

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Birthday birthday = DataContext as Birthday;
            if(String.IsNullOrEmpty(birthday.Name)|| BirthDate.SelectedDate==null)
            {
                MessageBox.Show("Введите обязательные параметры (имя и дата рождения)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                DateTime nextBirthday = new DateTime(DateTime.Today.Year, birthday.DateOfBirth.Month, birthday.DateOfBirth.Day);
                if (DateTime.Today > nextBirthday)
                    nextBirthday = nextBirthday.AddYears(1);

                Schedule nextBirthdayTimeStamp = new Schedule{ TimeStamp=nextBirthday};
                birthday.NextBirthday = nextBirthdayTimeStamp;
                birthday.Repeat = "Ежегодно";

                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Add(birthday);
                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
