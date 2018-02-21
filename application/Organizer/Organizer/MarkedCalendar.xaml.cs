using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for MarkedCalendar.xaml
    /// </summary>
    public partial class MarkedCalendar : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(MarkedCalendar));

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(MarkedCalendar));

        public static readonly DependencyProperty SelectedDatesChangedProperty =
            DependencyProperty.Register("SelectedDatesChanged", typeof(EventHandler<SelectionChangedEventArgs>), typeof(MarkedCalendar));

        public static readonly DependencyProperty MarkedDatesProperty =
            DependencyProperty.Register("MarkedDates", typeof(ICollection<DateTime>), typeof(MarkedCalendar), new PropertyMetadata(new List<DateTime>(), new PropertyChangedCallback(MarkedDates_PropertyChangedCallback)));

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        public EventHandler<SelectionChangedEventArgs> SelectedDatesChanged
        {
            get { return (EventHandler<SelectionChangedEventArgs>)GetValue(SelectedDatesChangedProperty); }
            set { SetValue(SelectedDatesChangedProperty, value); }
        }

        public ICollection<DateTime> MarkedDates
        {
            get { return (ICollection<DateTime>)GetValue(MarkedDatesProperty); }
            set { SetValue(MarkedDatesProperty, value); }
        }

        public MarkedCalendar()
        {
            InitializeComponent();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDatesChanged != null)
                SelectedDatesChanged(sender, e);
        }

        private static void MarkedDates_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkedCalendar)d).InitializeComponent();
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            Calendar.UpdateLayout();
        }

        public void UpdateCalendar()
        {
            Style style = (Style)FindResource("ShowDates");
            Calendar.CalendarDayButtonStyle = null;
            Calendar.CalendarDayButtonStyle = style;
        }
    }
}
