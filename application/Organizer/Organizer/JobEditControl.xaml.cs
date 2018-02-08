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
    /// Interaction logic for JobEditControl.xaml
    /// </summary>
    public partial class JobEditControl : UserControl
    {
        public JobEditControl()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Job job = DataContext as Job;
            if (String.IsNullOrEmpty(job.Name) || StartPicker.SelectedDateTime == null || DeadlinePicker.SelectedDateTime == null)
            {
                MessageBox.Show("Заполните обязательне поля(название, начало и конец встречи)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (StartPicker.SelectedDateTime >= DeadlinePicker.SelectedDateTime)
            {
                MessageBox.Show("Дата начала встречи должна предшестовать окончанию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if ((DateTime.Now < DeadlinePicker.SelectedDateTime && DateTime.Now < StartPicker.SelectedDateTime) ||
                MessageBox.Show("Вы точно хотите создать встречу в прошедшем времени?", "Вы уверены", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (organizerEntities db = new organizerEntities())
                {
                    db.Entry(job).State = job.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                    db.Entry(job.Start).State = job.Start.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                    db.Entry(job.Deadline).State = job.Start.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

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
