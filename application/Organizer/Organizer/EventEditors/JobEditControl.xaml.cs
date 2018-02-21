using System;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Форма для создания/редактивнования задания
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
                if (MessageBox.Show("Вы точно хотите сохранить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    Window.GetWindow(this).DialogResult = true;
                    Window.GetWindow(this).Close();

                    await job.DeleteRepeat();

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

                        await db.SaveChangesAsync();
                    }

                    if(job.Repeat!="Нет")
                    {
                        Schedule start = job.Start;
                        Schedule end = job.Deadline;
                        await start.CreateRepeat(job);
                        await end.CreateRepeat(job);
                    }

                    MainWindow.MainView.UpdateView();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
