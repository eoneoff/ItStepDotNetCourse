using System;
using System.Windows;
using System.Windows.Controls;

namespace Organizer
{
    ///Окно для выбора типа и создания нового события
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
