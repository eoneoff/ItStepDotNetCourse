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
            using (organizerEntities db = new organizerEntities())
            {
                var expenditureTypes = db.ExpenditureType.ToList();
                var expenditureNames = db.ExpenditureName.ToList();
                ExpenditureTypeSelector.ItemsSource = expenditureTypes;
                ExpenditureNameSelector.ItemsSource = expenditureNames;
            }
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

            if (MessageBox.Show("Вы точно хотите сохранить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Window.GetWindow(this).DialogResult = true;
                using (organizerEntities db = new organizerEntities())
                {
                    if (ExpenditureTypeSelector.SelectedIndex == -1)
                    {
                        if (MessageBox.Show($"Вы хотите добавить статью расходов {ExpenditureTypeSelector.Text}?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ExpenditureType type = new ExpenditureType() { Type = ExpenditureTypeSelector.Text };
                            expenditure.ExpenditureType = type;
                        }
                    }
                    //else
                    //{
                    //    expenditure.ExpenditureType = (ExpenditureType)ExpenditureTypeSelector.SelectedItem;
                    //}

                    if (ExpenditureNameSelector.SelectedIndex == -1)
                    {
                        if (MessageBox.Show($"Вы ходите добавить название {ExpenditureNameSelector.Text}?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ExpenditureName name = new ExpenditureName() { Name = ExpenditureNameSelector.Text };
                            expenditure.ExpenditureName = name;
                        }
                    }
                    else
                    {
                        
                    }

                    db.Entry(expenditure).State = expenditure.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                    db.Entry(expenditure.ExpenditureType).State = expenditure.ExpenditureType.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Unchanged;

                    db.Entry(expenditure.ExpenditureName).State = expenditure.ExpenditureName.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Unchanged;

                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                } 
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void NumberFilter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex number = new Regex(@"^\d*[.]?\d{0,4}$");
            e.Handled = !number.IsMatch(e.Text);
        }

        private void IntFilter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex number = new Regex(@"^\d*$");
            e.Handled = !number.IsMatch(e.Text);
        }

        private void SetTotal(object sender, TextChangedEventArgs e)
        {
            try
            {
                string price = ExpenditurePrice.Text.Replace('.', ',');
                decimal total = Convert.ToDecimal(price) * Convert.ToDecimal(ExpenditureQuantity.Text);
                ExpenditureTotal.Content = String.Format("{0:0.####}", total);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
