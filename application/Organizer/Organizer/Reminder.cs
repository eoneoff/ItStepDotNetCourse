//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Organizer
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    public partial class Reminder : Event
    {
        public int AlarmTimeId { get; set; }
        public string Memo { get; set; }
    
        public virtual Schedule AlarmTime { get; set; }

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
            get { return 330; }
        }

        public override int ShowControlHeight
        {
            get { return 312; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
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
