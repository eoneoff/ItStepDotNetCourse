using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Organizer
{
    ///Форма для создания/редактирования дохода
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class IncomeEditControl : UserControl
    {
        public IncomeEditControl()
        {
            InitializeComponent();
            using (organizerEntities db = new organizerEntities())
            {
                var incomeSources = db.IncomeSource.ToList();
                IncomeSourceSelector.ItemsSource = incomeSources;
            }
        }

        //Шаблон для проверки суммы дохода
        private void IncomeSum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex number = new Regex(@"^\d*[.]?\d{0,4}$");
            e.Handled = !number.IsMatch(e.Text);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Income income = (Income)DataContext;
            if(String.IsNullOrEmpty(IncomeSourceSelector.Text))
            {
                MessageBox.Show("Введите или выберите источник дохода", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите сохранить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    Window.GetWindow(this).DialogResult = true;
                    Window.GetWindow(this).Close();
                    using (organizerEntities db = new organizerEntities())
                    {
                        if (!db.IncomeSource.Any(s => s.Name == IncomeSourceSelector.Text))
                        {
                            if (MessageBox.Show($"Вы хотите задать новый источник дохода \"{IncomeSourceSelector.Text}\"?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                IncomeSource newSource = new IncomeSource() { Name = IncomeSourceSelector.Text };
                                income.IncomeSource = newSource;
                            }
                            else return;
                        }
                        else
                        {
                            income.IncomeSource = (IncomeSource)IncomeSourceSelector.SelectedItem;
                        }

                        db.Entry(income).State = income.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                        db.Entry(income.IncomeSource).State = income.IncomeSource.Id == 0 ?
                            System.Data.Entity.EntityState.Added :
                            System.Data.Entity.EntityState.Unchanged;

                        await db.SaveChangesAsync();
                    }

                    MainWindow.MainView.UpdateView();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
