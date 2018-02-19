using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Holiday
    {
        public override string EventTypeRus
        {
            get { return "Праздник"; }
        }

        public override string EventType
        {
            get { return "Holiday"; }
        }

        public override int EditControlHeight
        {
            get { return 280; }
        }

        public override int ShowControlHeight
        {
            get { return 340; }
        }

        public override void Initialize(DateTime date)
        {
            Priority = 1;
            Repeat = "Нет";
            Schedule holidayDate = new Schedule();
            holidayDate.TimeStamp = (DateTime)date;
            Date = holidayDate;
        }

        public override Control GetEditControl()
        {
            HolidayEditControl control = new HolidayEditControl();
            control.DataContext = this;
            return control;
        }

        public override Control GetShowControl()
        {
            HolidayShowControl control = new HolidayShowControl();
            control.DataContext = this;
            return control;
        }
    }
}
