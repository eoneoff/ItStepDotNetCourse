using System;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Создание нового будильника
    /// <summary>
    /// Interaction logic for CreateAlarmControl.xaml
    /// </summary>
    public partial class CreateAlarmControl : UserControl
    {
        public static readonly DependencyProperty SelectedDateTimeProperty =
           DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(CreateAlarmControl));

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public CreateAlarmControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы точно хотите создать будильник?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Window.GetWindow(this).DialogResult = true;
                Window.GetWindow(this).Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
