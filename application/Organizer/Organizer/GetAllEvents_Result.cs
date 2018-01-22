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
    
    public partial class GetAllEvents_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string Note { get; set; }
        public bool Done { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<System.DateTime> MeetingStart { get; set; }
        public Nullable<System.DateTime> Ending { get; set; }
        public string Location { get; set; }
        public Nullable<System.DateTime> AlarmTime { get; set; }

        //���������� ����� ������ �������
        public DateTime TimeStamp
        {
            get
            {
                if (DateOfBirth!=null)
                {
                    DateTime timestamp = new DateTime(DateTime.Now.Year, ((DateTime)DateOfBirth).Month, ((DateTime)DateOfBirth).Day);
                    if (DateTime.Today > timestamp)
                        timestamp = timestamp.AddYears(1);

                    return timestamp;
                }

                if (Date != null)
                    return (DateTime) Date;

                if (Start !=null)
                    return (DateTime.Now < Start) ? (DateTime) Start : (DateTime) Deadline;

                if (MeetingStart != null)
                    return (DateTime.Now < Start) ? (DateTime) MeetingStart : (DateTime) Ending;

                return (DateTime)AlarmTime;
            }
        }
    }
}
