using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using MoreLinq;

namespace Organizer
{
    class CalendarMarkerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = DateTime.Today;
            List<DateTime> dates = new List<DateTime>();

            if(value!=null&&MainWindow.MainView!=null)
            {
                date = (DateTime)value;
                dates = MainWindow.MainView.DatesOfEvents.Select(d=>d.Date).ToList();

                if (dates.Contains(date))
                    return "Gold";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
