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

    public partial class Meeting : Event
    {
        public int MeetingStartId { get; set; }
        public int MeetingEndId { get; set; }
        public string Location { get; set; }
    
        public virtual Schedule End { get; set; }
        public virtual Schedule Start { get; set; }

        public override string EventTypeRus
        {
            get { return "�������"; }
        }

        public override string EventType
        {
            get { return "Meeting"; }
        }

        public override int EditControlHeight
        {
            get { return 500; }
        }

        public override int ShowControlHeight
        {
            get { return 350; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            Repeat = "���";
            Schedule start = new Schedule();
            start.TimeStamp = (DateTime)date;
            Schedule end = new Schedule();
            end.TimeStamp = start.TimeStamp + TimeSpan.FromHours(1);
            Start = start;
            End = end;
        }

        public override Control GetEditControl()
        {
            MeetingEditControl control = new MeetingEditControl();
            control.DataContext = this;
            return control;
        }

        public override Control GetShowControl()
        {
            MeetingShowControl control = new MeetingShowControl();
            control.DataContext = this;
            return control;
        }
    }
}
