//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Organizer
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows;

    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.Schedule = new HashSet<Schedule>();
            this.Alarm = new HashSet<Alarm>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string Note { get; set; }
        public bool Done { get; set; }
        public string Repeat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alarm> Alarm { get; set; }

        public virtual string EventTypeRus
        {
            get { return "�������������� �������"; }
        }

        public virtual string EventType
        {
            get { return "Unspecified Event"; }
        }


        //���������� ������ ������� ������������� ����
        public static Event GetEventOfType(string eventType)
        {
            Event ev = null;

            switch (eventType)
            {
                case "Birthday":
                case "���� ��������":
                    ev = new Birthday();
                    break;
                case "Holiday":
                case "��������":
                    ev = new Holiday();
                    break;
                case "Job":
                case "�������":
                    ev = new Job();
                    break;
                case "Meeting":
                case "�������":
                    ev = new Meeting();
                    break;
                case "Reminder":
                case "�����������":
                    ev = new Reminder();
                    break;
            }

            return ev;
        }


        //����� ��� �������� � ���������� ����� ��������������/��������
        public virtual Control GetEditControl()
        {
            throw new ArgumentException();
        }


        //����� ��� �������� � ���������� ����� ������ ������������
        public virtual Control GetShowControl()
        {
            throw new ArgumentException();
        }

        //���������� ���� � ������ ������ ������������
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

        //���������� ���� � ������ ��������������/��������
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

        //����� ��� ��������
        //�������������� ����� �������
        public virtual void Initialize(DateTime date) { }

        public virtual int EditControlHeight { get; }

        public virtual int ShowControlHeight { get; }

    }
}
