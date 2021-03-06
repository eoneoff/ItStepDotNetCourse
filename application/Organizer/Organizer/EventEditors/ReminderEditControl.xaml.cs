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
    ///Форма для создания/редактирования напоминания
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
                if (MessageBox.Show("Вы точно хотите сохранить запись?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    Window.GetWindow(this).DialogResult = true;
                    Window.GetWindow(this).Close();

                    await reminder.DeleteRepeat();

                    using (organizerEntities db = new organizerEntities())
                    {
                        db.Entry(reminder).State = reminder.Id == 0 ?
                            System.Data.Entity.EntityState.Added :
                            System.Data.Entity.EntityState.Modified;

                        await db.SaveChangesAsync();
                    }

                    if (!String.IsNullOrEmpty(reminder.Repeat) && reminder.Repeat!="Нет")
                    {
                        Schedule prime = reminder.AlarmTime;
                        await prime.CreateRepeat(reminder); 
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
