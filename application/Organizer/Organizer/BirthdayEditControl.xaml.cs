using System;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Редактирование или создание дня рождения
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
                if (MessageBox.Show("Вы точно хотите изменить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    Window.GetWindow(this).DialogResult = true;

                    //Устанавливает напоминание о ближайшем дне рождения
                    DateTime nextBirthday = new DateTime(DateTime.Today.Year, birthday.DateOfBirth.Month, birthday.DateOfBirth.Day);
                    if (DateTime.Today > nextBirthday)
                        nextBirthday = nextBirthday.AddYears(1);

                    Schedule nextBirthdayTimeStamp = new Schedule { TimeStamp = nextBirthday };
                    birthday.NextBirthday = nextBirthdayTimeStamp;
                    birthday.Repeat = "Ежегодно";

                    using (organizerEntities db = new organizerEntities())
                    {
                        db.Entry(birthday).State = birthday.Id == 0 ?
                            System.Data.Entity.EntityState.Added :
                            System.Data.Entity.EntityState.Modified;
                        Window.GetWindow(this).Close();

                        await db.SaveChangesAsync();
                    } 
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
