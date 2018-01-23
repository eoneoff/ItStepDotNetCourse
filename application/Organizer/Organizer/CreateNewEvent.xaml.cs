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
using System.Windows.Shapes;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for CreateNewEvent.xaml
    /// </summary>
    public partial class CreateNewEvent : Window
    {
        private Control editor;

        public CreateNewEvent()
        {
            InitializeComponent();
        }

        private void EventTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editor != null)
            {
                Win.Children.Remove(editor);
                Height = 110;
                editor = null;
            }

            switch(EventTypeSelector.SelectedIndex)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    Birthday birthday = new Birthday();
                    birthday.DateOfBirtn = new Schedule();
                    birthday.DateOfBirtn.TimeStamp = DateTime.Today;
                    birthday.Priority = 1;
                    BirthdayEditControl birthdayControl = new BirthdayEditControl();
                    birthdayControl.DataContext = birthday;
                    birthdayControl.BirthDate.DataContext = birthday.DateOfBirtn;
                    editor = birthdayControl;
                    Height = 360;
                    break;
                case 5:
                    break;
                default:
                    return;
            }

            Grid.SetRow(editor, 2);

            Win.Children.Add(editor);
        }
    }
}
