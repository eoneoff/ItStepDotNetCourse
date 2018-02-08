using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for expenditureEditControl.xaml
    /// </summary>
    public partial class ExpenditureEditControl : UserControl
    {
        public ExpenditureEditControl()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Expenditure expenditure = (Expenditure)DataContext;
            if (String.IsNullOrEmpty(ExpenditureTypeSelector.Text))
            {
                MessageBox.Show("Введите или выберите тип траты", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (String.IsNullOrEmpty(ExpenditureNameSelector.Text))
            {
                MessageBox.Show("Введите или выберите название траты", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using (organizerEntities db = new organizerEntities())
            {
                if (ExpenditureTypeSelector.SelectedIndex == -1)
                {
                    if (MessageBox.Show("", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        ExpenditureType type = new ExpenditureType() { Type=ExpenditureNameSelector.Text};
                        expenditure.ExpenditureType = type;
                    }
                }

                if (ExpenditureNameSelector.SelectedIndex == -1)
                {
                    if (MessageBox.Show("", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        ExpenditureName name = new ExpenditureName() { Name = ExpenditureNameSelector.Text };
                        expenditure.ExpenditureName = name;
                    }
                }

                db.Entry(expenditure).State = expenditure.Id == 0 ?
                    System.Data.Entity.EntityState.Added :
                    System.Data.Entity.EntityState.Modified;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void NumberFilter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex number = new Regex(@"^\d*[.,]?\d{0,4}$");
            e.Handled = !number.IsMatch(e.Text);
        }
    }
}
