using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Meeting
    {
        public override string EventTypeRus
        {
            get { return "Встреча"; }
        }

        public override string EventType
        {
            get { return "Meeting"; }
        }

        public override int EditControlHeight
        {
            get { return 520; }
        }

        public override int ShowControlHeight
        {
            get { return 400; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            Repeat = "Нет";
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

        public override async Task DeleteRepeat()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var schedule = db.Schedule.Where(s => s.EventId == Id&&s.Id!=MeetingStartId&&s.Id!=MeetingEndId).ToList();
                db.Schedule.RemoveRange(schedule);
                await db.SaveChangesAsync();
            }
        }
    }
}
