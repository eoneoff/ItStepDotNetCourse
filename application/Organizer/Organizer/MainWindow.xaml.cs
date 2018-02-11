﻿using System;
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
using MoreLinq;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainView;

        private Control view;
        private AllExpensesView expensesView;

        public event Action CalendarClick;

        public MainWindow()
        {
            MainView = this;

            InitializeComponent();
            ViewModePicker.SelectedIndex = 0;
            ExpensesViewModePicker.SelectedIndex = 0;
            using (organizerEntities db = new organizerEntities())
            {
                var top5 = db.Schedule.Include("Event").Where(s => s.TimeStamp > DateTime.Now).OrderBy(s => s.TimeStamp).DistinctBy(s => s.Event).Take(5).ToList();
                foreach (var s in top5)
                {
                    if (s.Event.EventType == "Job")
                    {
                        if (!db.Entry((Job)s.Event).Reference(j => j.Start).IsLoaded)
                            db.Entry((Job)s.Event).Reference(j => j.Start).Load();
                    }

                    else
                    {
                        if (s.Event.EventType == "Meeting")
                        {
                            if (!db.Entry((Meeting)s.Event).Reference(m => m.Start).IsLoaded)
                                db.Entry((Meeting)s.Event).Reference(m => m.Start).Load();
                        }
                    }
                }
                Top5Events.ItemsSource = top5;
            }
        }

        private void Top5Events_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Event ev = ((Schedule)Top5Events.SelectedItem).Event;
            RecordWindow window = ev.GetShowWindow();
            if (window.ShowDialog() == true)
                showEvents();
        }

        private void ViewModePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showEvents();
        }

        private void showEvents()
        {
            MainPanel.Children.Remove(view);

            Binding b = new Binding("SelectedDate");
            b.Source = CurrentDate;
            b.Mode = BindingMode.TwoWay;

            switch (ViewModePicker.SelectedIndex)
            {
                case 0:
                    view = new AllEventsView();
                    break;
                case 1:
                    view = new OneDayViewControl();
                    view.SetBinding(OneDayViewControl.CurrentDateProperty, b);
                    break;
                case 2:
                    view = new OneWeekViewControl();
                    view.SetBinding(OneWeekViewControl.CurrentDateProperty, b);
                    break;
                case 3:
                    view = new OneMonthControl();
                    view.SetBinding(OneMonthControl.CurrentDateProperty, b);
                    break;
            }

            Grid.SetRow(view, 1);
            Grid.SetColumnSpan(view, 3);
            MainPanel.Children.Add(view);
        }

        private void ExpensesViewModePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showExpenses();
        }

        private void showExpenses()
        {
            MainExpensesPanel.Children.Remove(expensesView);

            Binding b = new Binding("SelectedDate");
            b.Source = ExpensesCurrentDate;
            b.Mode = BindingMode.TwoWay;

            expensesView = new AllExpensesView();
            expensesView.SetBinding(AllExpensesView.CurrentDateProperty, b);

            switch (ExpensesViewModePicker.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    expensesView.Mode = "day";
                    break;
                case 2:
                    expensesView.Mode = "week";
                    break;
                case 3:
                    expensesView.Mode = "month";
                    break;
            }

            Grid.SetRow(expensesView, 1);
            Grid.SetColumnSpan(expensesView, 4);
            MainExpensesPanel.Children.Add(expensesView);
        }

        private void NewEvent_Click(object sender, RoutedEventArgs e)
        {
            CreateNewEvent create = new CreateNewEvent();
            create.Date.SelectedDate = CurrentDate.SelectedDate;
            if (create.ShowDialog() == true)
                showEvents();
        }

        private void NewIncome_Click(object sender, RoutedEventArgs e)
        {
            Income income = new Income() { DateTime=(DateTime)CurrentDate.SelectedDate};
            RecordWindow window = income.GetEditWindow();
            if (window.ShowDialog() == true)
                showExpenses();
        }

        private void NewExpenditure_Click(object sender, RoutedEventArgs e)
        {
            Expenditure exp = new Expenditure() { DateTime = (DateTime)CurrentDate.SelectedDate };
            RecordWindow window = exp.GetEditWindow();
            window.Height = 400;
            if (window.ShowDialog() == true)
                showExpenses();
        }

        private void PreviewCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CalendarClick?.Invoke();
        }

        private void GraphDates_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GraphMode.IsEnabled = false;

                GraphMode.ItemsSource = null;

                TimeSpan graphRange = (TimeSpan)(EndDate.SelectedDate - StartDate.SelectedDate);

                if (graphRange < TimeSpan.Zero)
                {
                    StartDate.SelectedDate = EndDate.SelectedDate;
                    return;
                }

                if (graphRange > TimeSpan.FromDays(2))
                {
                    GraphMode.IsEnabled = true;
                    List<string> modes = new List<string>();

                    modes.Add("По дням");

                    if(graphRange>TimeSpan.FromDays(7))
                    {
                        modes.Add("По неделям");
                    }

                    if (graphRange > TimeSpan.FromDays(62))
                    {
                        modes.Add("По месяцам");
                    }

                    if (graphRange > TimeSpan.FromDays(731))
                    {
                        modes.Add("По годам");
                    }

                    GraphMode.ItemsSource = modes;
                    GraphMode.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
