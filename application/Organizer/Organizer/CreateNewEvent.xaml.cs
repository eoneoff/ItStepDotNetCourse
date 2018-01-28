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
using System.Windows.Shapes;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for CreateNewEvent.xaml
    /// </summary>
    public partial class CreateNewEvent : Window
    {
        private Control editor;

        public CreateNewEvent()
        {
            InitializeComponent();
        }

        private void EventTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editor != null)
            {
                Win.Children.Remove(editor);
                editor = null;
            }

            if(EventTypeSelector.SelectedIndex==0)
            {
                Height = 120;
            }
            else
            {
                Event ev = Event.GetEventOfType(((TextBlock)EventTypeSelector.SelectedItem).Text);
                ev.Initialize((DateTime)Date.SelectedDate);
                editor = ev.GetEditControl();
                Grid.SetRow(editor, 2);
                Win.Children.Add(editor);
                Height = 120+ev.EditControlHeight;
            }
        }
    }
}
