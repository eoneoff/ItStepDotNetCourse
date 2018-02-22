using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer
{
    public partial class Schedule
    {
        public async Task CreateRepeat(Event ev)
        {
            DateTime current = TimeStamp;

            List<Schedule> repeats = new List<Schedule>();

            while(current<TimeStamp.AddYears(3))
            {
                switch(ev.Repeat)
                {
                    case "Ежедневно":
                        current = current.AddDays(1);
                        break;
                    case "Еженедельно":
                        current = current.AddDays(7);
                        break;
                    case "Ежемесячно":
                        current = current.AddMonths(1);
                        break;
                    case "Ежегодно":
                        current = current.AddYears(1);
                        break;
                    default:
                        current = DateTime.MaxValue;
                        break;
                }

                repeats.Add(new Schedule { TimeStamp = current, Event = ev });
            }

            using (organizerEntities db = new organizerEntities())
            {
                db.Event.Attach(ev);
                db.Schedule.AddRange(repeats);
                await db.SaveChangesAsync();
            }
        }
    }
}
