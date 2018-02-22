using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task DeleteRepeat()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var schedule = db.Schedule.Where(s => s.EventId == Id&&s.Id!=AlarmTimeId).ToList();
                db.Schedule.RemoveRange(schedule);
                await db.SaveChangesAsync();
            }
        }
    }
}
