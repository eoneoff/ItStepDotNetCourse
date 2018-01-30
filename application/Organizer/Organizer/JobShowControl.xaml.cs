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
    /// Interaction logic for JobShowControl.xaml
    /// </summary>
    public partial class JobShowControl : UserControl
    {
        public JobShowControl()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow edit = ((Job)DataContext).GetEditWindow();

            edit.Show();

            InitializeComponent();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (organizerEntities db = new organizerEntities())
            {
                Job job = new Job { Id = ((Job)DataContext).Id };
                db.Event.Attach(job);
                db.Event.Remove(job);
                Window.GetWindow(this).Close();
                await db.SaveChangesAsync();
            }
        }
    }
}
