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
    /// Interaction logic for HolidayEditControl.xaml
    /// </summary>
    public partial class HolidayEditControl : UserControl
    {
        public HolidayEditControl()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Holiday holiday = DataContext as Holiday;
            using (organizerEntities db = new organizerEntities())
            {
                db.Entry(holiday).State = holiday.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                Window.GetWindow(this).Close();
                await db.SaveChangesAsync();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
