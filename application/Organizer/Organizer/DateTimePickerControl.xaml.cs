using System;
using System.Collections.Generic;
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
    /// Interaction logic for DateTimePickerControl.xaml
    /// </summary>
    public partial class DateTimePickerControl : UserControl
    {
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(DateTimePickerControl));

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public int Hours
        {
            get { return (SelectedDateTime == null) ? 0 : ((DateTime)SelectedDateTime).Hour; }
        }

        public int Minutes
        {
            get { return (SelectedDateTime == null) ? 0 : ((DateTime)SelectedDateTime).Minute; }
        }

        public DateTimePickerControl()
        {
            InitializeComponent();
            HoursPicker.ItemsSource = Enumerable.Range(0, 25).Select(x=>x.ToString("D2"));
            MinutesPicker.ItemsSource = Enumerable.Range(0, 61).Select(x => x.ToString("D2"));
        }

        private void ChangeDateTime (object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime day = (DateTime)DatePicker.SelectedDate;
                SelectedDateTime = new DateTime(day.Year, day.Month, day.Day,
                HoursPicker.SelectedIndex, MinutesPicker.SelectedIndex, 0);
            }
            catch(Exception ex)
            {

            }

        }
    }
}
