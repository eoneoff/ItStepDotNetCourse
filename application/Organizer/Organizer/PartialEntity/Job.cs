using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Job
    {
        public override string EventTypeRus
        {
            get { return "Задание"; }
        }

        public override string EventType
        {
            get { return "Job"; }
        }

        public override int EditControlHeight
        {
            get { return 455; }
        }

        public override int ShowControlHeight
        {
            get { return 340; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            Repeat = "Нет";
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

        public override async Task DeleteRepeat()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var schedule = db.Schedule.Where(s => s.EventId == Id&&s.Id!=JobStartId&&s.Id!=JobDeadlineId).ToList();
                db.Schedule.RemoveRange(schedule);
                await db.SaveChangesAsync();
            }
        }
    }
}
