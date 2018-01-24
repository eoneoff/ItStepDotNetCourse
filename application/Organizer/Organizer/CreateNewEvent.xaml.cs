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
                Height = 120;
                editor = null;
            }

            switch(EventTypeSelector.SelectedIndex)
            {
                case 1:
                    Reminder reminder = new Reminder();
                    reminder.Priority = 1;
                    ReminderEditControl reminderControl = new ReminderEditControl();
                    Schedule alarm = new Schedule();
                    alarm.TimeStamp = (DateTime)Date.SelectedDate;
                    reminder.AlarmTime = alarm;
                    reminderControl.DataContext = reminder;
                    editor = reminderControl;
                    Height = 440;
                    break;
                case 2:
                    Meeting meeting = new Meeting();
                    meeting.Priority = 1;
                    Schedule start = new Schedule();
                    start.TimeStamp = (DateTime)Date.SelectedDate;
                    Schedule end = new Schedule();
                    end.TimeStamp = start.TimeStamp + TimeSpan.FromHours(1);
                    meeting.Start = start;
                    meeting.End = end;
                    MeetingEditControl meetingControl = new MeetingEditControl();
                    meetingControl.DataContext = meeting;
                    editor = meetingControl;
                    Height = 585;
                    break;
                case 3:
                    break;
                case 4:
                    Birthday birthday = new Birthday();
                    birthday.Priority = 1;
                    birthday.DateOfBirth = (DateTime) Date.SelectedDate;
                    BirthdayEditControl birthdayControl = new BirthdayEditControl();
                    birthdayControl.DataContext = birthday;
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
