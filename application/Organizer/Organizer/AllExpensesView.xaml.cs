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
    /// Interaction logic for AllExpensesView.xaml
    /// </summary>
    public partial class AllExpensesView : UserControl
    {
        private string mode;

        public string Mode
        {
            set
            {
                if (value == "day" || value == "week" || value == "month")
                {
                    mode = value;
                    getEvents();
                }
                else
                    mode = null;
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

        private void ExensesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Article article = (Article)ExpensesList.SelectedItem;
            RecordWindow edit = null;

                if(article is Income)
                {
                    using (organizerEntities db = new organizerEntities())
                    {
                        db.Article.Attach(article);
                        db.Entry((Income)article).Reference(i => i.IncomeSource).Load(); 
                    }
                    edit = article.GetEditWindow();
                }
                else
                {
                    using (organizerEntities db = new organizerEntities())
                    {
                        db.Article.Attach(article);
                        db.Entry((Expenditure)article).Reference(exp => exp.ExpenditureType).Load();
                        db.Entry((Expenditure)article).Reference(exp => exp.ExpenditureName).Load(); 
                    }
                    edit = article.GetEditWindow();
                    edit.Height = 400;
                }

            if (edit.ShowDialog() == true)
                getEvents();
        }

        private static void CurrentDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AllExpensesView)sender).getEvents();
        }

        private void getEvents()
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


            switch(ViewType.SelectedIndex)
            {
                case 0:
                    using (organizerEntities db = new organizerEntities())
                    {
                        var articles = db.Article.Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToList();
                        ExpensesList.ItemsSource = articles;
                        Total.Content = "";
                        Total.Content = (articles.OfType<Income>().Sum(i => i.Summ) - articles.OfType<Expenditure>().Sum(e => e.Summ)).ToString();
                    }
                    break;
                case 1:
                    using (organizerEntities db = new organizerEntities())
                    {
                       var expenses = db.Article.OfType<Expenditure>().Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToList();
                        ExpensesList.ItemsSource = expenses;
                        Total.Content = "";
                        Total.Content = expenses.Sum(e => e.Summ).ToString();
                    }
                    break;
                case 2:
                    using (organizerEntities db = new organizerEntities())
                    {
                        var income = db.Article.OfType<Income>().Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToList();
                        ExpensesList.ItemsSource = income;
                        Total.Content = "";
                        Total.Content = income.Sum(i => i.Summ).ToString();
                    }
                    break;
            }

            ExpensesList.Items.Refresh();
            OnCalendarClick();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
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

            getEvents();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
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

            getEvents();
        }

        private void ViewType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getEvents();
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
    }
}
