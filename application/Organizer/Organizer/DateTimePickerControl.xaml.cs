using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Контрол для выбора даты/времени
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HoursPicker.SelectedIndex = SelectedDateTime != null ? ((DateTime)SelectedDateTime).Hour : 0;
            MinutesPicker.SelectedIndex = SelectedDateTime != null ? ((DateTime)SelectedDateTime).Minute : 0; ;
        }
    }
}
