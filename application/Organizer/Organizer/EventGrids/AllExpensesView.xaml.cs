using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for AllExpensesView.xaml
    /// </summary>
    public partial class AllExpensesView : UserControl
    {
        private string mode;

        public string Mode//Промежуток времени для показа
        {
            set
            {
                if (value == "day" || value == "week" || value == "month")
                {
                    mode = value;
                }
                else
                    mode = null;

                getEvents();
            }
        }

        public static readonly DependencyProperty CurrentDateProperty = 
            DependencyProperty.Register("CurrentDate", typeof(DateTime?), typeof(AllExpensesView),
                new PropertyMetadata(default(DateTime?), new PropertyChangedCallback(CurrentDateChanged)));

        public DateTime? CurrentDate
        {
            get { return (DateTime?)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        public AllExpensesView()
        {
            InitializeComponent();
            ViewType.SelectedIndex = 0;
            getEvents();
            MainWindow.MainView.CalendarClick += OnCalendarClick;
        }

        private async void ExensesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Article article = (Article)ExpensesList.SelectedItem;
            RecordWindow edit = article.GetEditWindow();
            if (edit.ShowDialog() == true)
                await getEvents();
        }

        private async static void CurrentDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            await ((AllExpensesView)sender).getEvents();
        }

        private async Task getEvents()//Получение списка финансовых операций в зависимости от пунктов, выбранных в главном окне
        {
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;
            Previous.Visibility = Visibility.Hidden;
            Next.Visibility = Visibility.Hidden;

            switch (mode)
            {
                case "day":
                    start = ((DateTime)CurrentDate).Date;
                    end = start.AddDays(1);
                    Previous.Visibility = Visibility.Visible;
                    Next.Visibility = Visibility.Visible;
                    break;
                case "week":
                    start = ((DateTime)CurrentDate).Date;
                    start = start.AddDays(DayOfWeek.Monday-start.DayOfWeek);
                    end = start.AddDays(7);
                    Previous.Visibility = Visibility.Visible;
                    Next.Visibility = Visibility.Visible;
                    break;
                case "month":
                    start = ((DateTime)CurrentDate).Date;
                    start = new DateTime(start.Year, start.Month, 1);
                    end = start.AddMonths(1);
                    Previous.Visibility = Visibility.Visible;
                    Next.Visibility = Visibility.Visible;
                    break;
            }

            decimal total=0;
            switch(ViewType.SelectedIndex)//Показ доходов/расходов/всех операций
            {
                case 0:
                    using (organizerEntities db = new organizerEntities())
                    {
                        var articles = await db.Article.Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToListAsync();
                        ExpensesList.ItemsSource = articles;
                        total = (articles.OfType<Income>().Sum(i => i.Summ) - articles.OfType<Expenditure>().Sum(e => e.Summ)??0);
                    }
                    break;
                case 1:
                    using (organizerEntities db = new organizerEntities())
                    {
                       var expenses = await db.Article.OfType<Expenditure>().Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToListAsync();
                        ExpensesList.ItemsSource = expenses;
                        total = expenses.Sum(e => e.Summ)??0;
                    }
                    break;
                case 2:
                    using (organizerEntities db = new organizerEntities())
                    {
                        var income = await db.Article.OfType<Income>().Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToListAsync();
                        ExpensesList.ItemsSource = income;
                        total = income.Sum(i => i.Summ);
                    }
                    break;
            }

            Total.Content = total.ToString("0.####");

            ExpensesList.Items.Refresh();
            OnCalendarClick();
        }


        //Переход вперед/назад
        private async void Previous_Click(object sender, RoutedEventArgs e)
        {
            switch(mode)
            {
                case "day":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(-1);
                    break;
                case "week":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(-7);
                    break;
                case "month":
                    CurrentDate = ((DateTime)CurrentDate).AddMonths(-1);
                    break;
            }

            await getEvents();
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            switch (mode)
            {
                case "day":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(1);
                    break;
                case "week":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(7);
                    break;
                case "month":
                    CurrentDate = ((DateTime)CurrentDate).AddMonths(1);
                    break;
            }

            await getEvents();
        }

        private async void ViewType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await getEvents();
        }

        private void OnCalendarClick()
        {
            if (ExpensesList.DataContext!=null)
            {
                List<Article> articles = (List<Article>)ExpensesList.DataContext;
                Article selected = articles.Where(a => a.DateTime >= (DateTime)MainWindow.MainView.ExpensesCurrentDate.SelectedDate).FirstOrDefault();
                if (selected != null)
                    ExpensesList.SelectedItem = selected; 
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы точно хотите удалить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                Article article = (Article)ExpensesList.SelectedItem;
                using (organizerEntities db = new organizerEntities())
                {
                    db.Article.Attach(article);
                    db.Article.Remove(article);

                    db.SaveChanges();
                }

                await getEvents();
            }
        }
    }
}
