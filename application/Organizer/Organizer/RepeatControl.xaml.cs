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
    /// Interaction logic for RepeatControl.xaml
    /// </summary>
    public partial class RepeatControl : UserControl
    {
        static private string[] repeatMode ={"Нет","Ежедневно","Еженедельно","Ежемесячно","Ежегодно"};

        public RepeatControl()
        {
            InitializeComponent();
            RepeatPicker.ItemsSource = repeatMode;
        }
    }
}
