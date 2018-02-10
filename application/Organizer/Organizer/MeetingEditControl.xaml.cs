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
    /// Interaction logic for MeetingEditControl.xaml
    /// </summary>
    public partial class MeetingEditControl : UserControl
    {
        public MeetingEditControl()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
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
