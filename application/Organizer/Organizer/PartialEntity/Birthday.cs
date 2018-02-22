using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Birthday
    {
        public override string EventTypeRus
        {
            get { return "День рождения"; }
        }

        public override string EventType
        {
            get { return "Birthday"; }
        }

        public override int EditControlHeight
        {
            get { return 260; }
        }

        public override int ShowControlHeight
        {
            get { return 340; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            DateOfBirth = (DateTime)date;
            Repeat = "Ежегодно";
        }

        public override Control GetEditControl()
        {
            BirthdayEditControl control = new BirthdayEditControl();
            control.DataContext = this;
            return control;
        }

        public override Control GetShowControl()
        {
            BirthdayShowControl control = new BirthdayShowControl();
            control.DataContext = this;
            return control;
        }

        public override async Task DeleteRepeat()
        {
            using (organizerEntities db = new organizerEntities())
            {
                var schedule = db.Schedule.Where(s => s.EventId == Id&&s.Id!=NextBirthdayId).ToList();
                db.Schedule.RemoveRange(schedule);
                await db.SaveChangesAsync();
            }
        }
    }
}
