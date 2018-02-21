using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Форма для создания/редактинования праздников
    /// <summary>
    /// Interaction logic for HolidayEditControl.xaml
    /// </summary>
    public partial class HolidayEditControl : UserControl
    {
        public HolidayEditControl()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите сохранить запись?","Вы уверены?",MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Window.GetWindow(this).DialogResult = true;
                Holiday holiday = DataContext as Holiday;

                Window.GetWindow(this).Close();

                await holiday.DeleteRepeat();

                using (organizerEntities db = new organizerEntities())
                {
                    db.Entry(holiday).State = holiday.Id == 0 ?
                            System.Data.Entity.EntityState.Added :
                            System.Data.Entity.EntityState.Modified;

                    await db.SaveChangesAsync();
                }

                if (holiday.Repeat != "Нет")
                {
                    Schedule prime = holiday.Date;
                    await prime.CreateRepeat(holiday);
                }

                MainWindow.MainView.UpdateView();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
