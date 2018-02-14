using System;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Редактирование/создание новой встречи
    /// <summary>
    /// Interaction logic for MeetingEditControl.xaml
    /// </summary>
    public partial class MeetingEditControl : UserControl
    {
        public MeetingEditControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Meeting meeting = DataContext as Meeting;
            if(String.IsNullOrEmpty(meeting.Name) || StartPicker.SelectedDateTime == null || EndPicker.SelectedDateTime == null)
            {
                MessageBox.Show("Заполните обязательне поля(название, начало и конец встречи)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if(StartPicker.SelectedDateTime >= EndPicker.SelectedDateTime)
            {
                MessageBox.Show("Дата начала встречи должна предшестовать окончанию","Внимание",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
            else if ((DateTime.Now < EndPicker.SelectedDateTime && DateTime.Now < StartPicker.SelectedDateTime) ||
                MessageBox.Show("Вы точно хотите создать встречу в прошедшем времени?", "Вы уверены", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (MessageBox.Show("Вы точно хотите сохранить запись,","Вы уверены,",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    Window.GetWindow(this).DialogResult = true;
                    using (organizerEntities db = new organizerEntities())
                    {
                        db.Entry(meeting).State = meeting.Id == 0 ?
                            System.Data.Entity.EntityState.Added :
                            System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                        Window.GetWindow(this).Close();
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
