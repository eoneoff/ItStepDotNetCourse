using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Organizer
{
    class Accessories
    {
        //Возвращает тип события на английском или русском
        public static string GetEventType(Event ev, bool russian = false)
        {
            if (ev is Birthday)
                return (russian)?"День рождения":"Birthday";
            if (ev is Holiday)
                return (russian)?"Праздник":"Holiday";
            if (ev is Job)
                return (russian)?"Задание":"Job";
            if (ev is Meeting)
                return (russian)?"Встреча":"Meeting";
            if (ev is Reminder)
                return (russian)?"Напоминание":"Reminder";

            return (russian)?"Неопознанное событие":"Unundentified Event";
        }


        //Возвращает форму редактирования события с контекстом из нового пустого события
        public static Control EventEditControlFactoryMethod(string eventType, DateTime? date)
        {
            Control control = null;

            if (date == null)
                date = DateTime.Now;

            switch (eventType)
            {
                case "Birthday":
                case "День рождения":
                    Birthday birthday = new Birthday();
                    birthday.Priority = 1;
                    birthday.DateOfBirth = (DateTime)date;
                    control = new BirthdayEditControl();
                    control.DataContext = birthday;
                    break;
                case "Holiday":
                case "Праздник":
                    Holiday holiday = new Holiday();
                    holiday.Priority = 1;
                    Schedule holidayDate = new Schedule();
                    holidayDate.TimeStamp = (DateTime)date;
                    holiday.Date = holidayDate;
                    control = new HolidayEditControl();
                    control.DataContext = holiday;
                    break;
                case "Job":
                case "Задание":
                    Job job = new Job();
                    job.Priority = 1;
                    Schedule jobStart = new Schedule();
                    jobStart.TimeStamp = (DateTime)date;
                    Schedule deadline = new Schedule();
                    deadline.TimeStamp = jobStart.TimeStamp + TimeSpan.FromHours(1);
                    job.Start = jobStart;
                    job.Deadline = deadline;
                    control = new JobEditControl();
                    control.DataContext = job;
                    break;
                case "Meeting":
                case "Встреча":
                    Meeting meeting = new Meeting();
                    meeting.Priority = 1;
                    Schedule start = new Schedule();
                    start.TimeStamp = (DateTime)date;
                    Schedule end = new Schedule();
                    end.TimeStamp = start.TimeStamp + TimeSpan.FromHours(1);
                    meeting.Start = start;
                    meeting.End = end;
                    control = new MeetingEditControl();
                    control.DataContext = meeting;
                    break;
                case "Reminder":
                case "Напоминание":
                    Reminder reminder = new Reminder();
                    reminder.Priority = 1;
                    control = new ReminderEditControl();
                    Schedule alarm = new Schedule();
                    alarm.TimeStamp = (DateTime)date;
                    reminder.AlarmTime = alarm;
                    control.DataContext = reminder;
                    break;
            }

            return control;
        }

        public static Control EventShowControlFactoryMethod(Event ev)
        {
            Control control = null;

                switch (GetEventType(ev))
                {
                    case "Birthday":
                        control = new BirthdayShowControl();
                        break;
                    case "Holiday":
                        control = new HolidayShowControl();
                        break;
                    case "Job":
                        control = new JobShowControl();
                        //using (organizerEntities db = new organizerEntities())
                        //{
                        //    db.Entry(ev).Reference(j => j.Start).Load();
                        //    db.Entry(ev).Reference(j => j.Deadline).Load();
                        //}
                        break;
                    case "Meeting":
                        //using (organizerEntities db = new organizerEntities())
                        //{
                        //    db.Entry((Meeting)ev).Reference(m => m.Start).Load();
                        //    db.Entry((Meeting)ev).Reference(m => m.End).Load();
                        //}
                        control = new MeetingShowControl();
                        break;
                    case "Reminder":
                        control = new ReminderShowControl();
                        break;
                }

            return control;
        }
    }
}
