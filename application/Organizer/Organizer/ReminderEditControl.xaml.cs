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

namespace Organizer
{
    /// <summary>
    /// Interaction logic for ReminderEditControl.xaml
    /// </summary>
    public partial class ReminderEditControl : UserControl
    {
        public ReminderEditControl()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = DataContext as Reminder;
            if (String.IsNullOrEmpty(reminder.Name) ||DateTimePicker.SelectedDateTime==null)
            {
                MessageBox.Show("Заполните обязательне поля(название и время)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (DateTime.Now < DateTimePicker.SelectedDateTime||
                MessageBox.Show("Вы точно хотите создать напоминание в прошедшем времени?","Вы уверены",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                using (organizerEntities db = new organizerEntities())
                {
                    db.Event.Add(reminder);
                    Window.GetWindow(this).Close();
                    await db.SaveChangesAsync();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
