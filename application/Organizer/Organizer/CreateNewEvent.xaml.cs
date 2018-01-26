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

            switch (EventTypeSelector.SelectedIndex)
            {
                case 0:
                    Height=120;
                    break;
                case 1:
                    Height = 440;
                    break;
                case 2:
                    Height = 585;
                    break;
                case 3:
                    Height = 530;
                    break;
                case 4:
                    Height = 360;
                    break;
                case 5:
                    Height = 360;
                    break;
                default:
                    return;
            }


            if (EventTypeSelector.SelectedIndex > 0)
            {
                editor = Accessories.EventEditControlFactoryMethod(((TextBlock)EventTypeSelector.SelectedItem).Text, Date.SelectedDate);
                Grid.SetRow(editor, 2);
                Win.Children.Add(editor);
            }
        }
    }
}
