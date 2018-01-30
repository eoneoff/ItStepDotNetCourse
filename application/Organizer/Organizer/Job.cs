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

    public partial class Job : Event
    {
        public int JobStartId { get; set; }
        public int JobDeadlineId { get; set; }
    
        public virtual Schedule Deadline { get; set; }
        public virtual Schedule Start { get; set; }

        public override string EventTypeRus
        {
            get { return "�������"; }
        }

        public override string EventType
        {
            get { return "Job"; }
        }

        public override int EditControlHeight
        {
            get { return 450; }
        }

        public override int ShowControlHeight
        {
            get { return 290; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            Repeat = "���";
            Schedule jobStart = new Schedule();
            jobStart.TimeStamp = (DateTime)date;
            Schedule deadline = new Schedule();
            deadline.TimeStamp = jobStart.TimeStamp + TimeSpan.FromHours(1);
            Start = jobStart;
            Deadline = deadline;
        }

        public override Control GetEditControl()
        {
            JobEditControl control = new JobEditControl();
            control.DataContext = this;
            return control;
        }

        public override Control GetShowControl()
        {
            JobShowControl control = new JobShowControl();
            control.DataContext = this;
            return control;
        }
    }
}
