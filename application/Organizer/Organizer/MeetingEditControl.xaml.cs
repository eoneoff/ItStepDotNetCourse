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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Meeting meeting = (Meeting)DataContext;
            if (meeting.Name == String.Empty || StartPicker.SelectedDateTime == null || EndPicker.SelectedDateTime == null)
            {
                MessageBox.Show("Заполните обязательне поля(название, начало и конец встречи)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if(StartPicker.SelectedDateTime >= EndPicker.SelectedDateTime)
            {
                MessageBox.Show("Дата начала встречи должна предшестовать окончанию","Внимание",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
            else if (DateTime.Now < EndPicker.SelectedDateTime || DateTime.Now < StartPicker.SelectedDateTime ||
                MessageBox.Show("Вы точно хотите создать встречу в прошедшем времени?", "Вы уверены", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Add(meeting);
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
