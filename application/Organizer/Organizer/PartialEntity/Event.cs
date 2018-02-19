using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace Organizer
{
    public partial class Event
    {
        public virtual string EventTypeRus
        {
            get { return "Неопределенное событие"; }
        }

        public virtual string EventType
        {
            get { return "Unspecified Event"; }
        }


        //Возвращает пустое событие определенного типа
        public static Event GetEventOfType(string eventType)
        {
            Event ev = null;

            switch (eventType)
            {
                case "Birthday":
                case "День рождения":
                    ev = new Birthday();
                    break;
                case "Holiday":
                case "Праздник":
                    ev = new Holiday();
                    break;
                case "Job":
                case "Задание":
                    ev = new Job();
                    break;
                case "Meeting":
                case "Встреча":
                    ev = new Meeting();
                    break;
                case "Reminder":
                case "Напоминание":
                    ev = new Reminder();
                    break;
            }

            return ev;
        }


        //Метод для потомков — возвращает форму редактирования/создания
        public virtual Control GetEditControl()
        {
            throw new ArgumentException();
        }


        //Метод для потомков — возвращает форму показа подробностей
        public virtual Control GetShowControl()
        {
            throw new ArgumentException();
        }

        //Возвращает окно с формой показа подробностей
        public RecordWindow GetShowWindow()
        {
            RecordWindow window = new RecordWindow();

            Control showControl = GetShowControl();
            Grid.SetRow(showControl, 0);
            window.Win.Children.Add(showControl);
            window.Height = 35 + ShowControlHeight;
            window.Title = Name;

            return window;
        }

        //Возвращает окно с формой редактирования/создания
        public RecordWindow GetEditWindow()
        {
            RecordWindow window = new RecordWindow();

            Control editControl = GetEditControl();
            Grid.SetRow(editControl, 0);
            window.Win.Children.Add(editControl);
            window.Height = 35 + EditControlHeight;
            window.Title = Name;

            return window;
        }

        //Метод для потомков
        //Инициализирует новое событие
        public virtual void Initialize(DateTime date) { }

        public virtual int EditControlHeight { get; }

        public virtual int ShowControlHeight { get; }
    }
}
