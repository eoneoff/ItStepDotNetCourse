using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Reminder
    {
        public override string EventTypeRus
        {
            get { return "Напоминание"; }
        }

        public override string EventType
        {
            get { return "Reminder"; }
        }

        public override int EditControlHeight
        {
            get { return 370; }
        }

        public override int ShowControlHeight
        {
            get { return 365; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            Repeat = "Нет";
            Schedule alarm = new Schedule();
            alarm.TimeStamp = (DateTime)date;
            AlarmTime = alarm;
        }

        public override Control GetEditControl()
        {
            ReminderEditControl control = new ReminderEditControl();
            control.DataContext = this;
            return control;
        }

        public override Control GetShowControl()
        {
            ReminderShowControl control = new ReminderShowControl();
            control.DataContext = this;
            return control;
        }
    }
}
