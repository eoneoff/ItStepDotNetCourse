using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Organizer
{
    public partial class Article
    {
        public RecordWindow GetEditWindow()
        {
            RecordWindow window = new RecordWindow();
            Control control = GetEditControl();
            Grid.SetRow(control, 0);
            window.Win.Children.Add(control);
            window.Title = FullName;
            if (window.Title == "New Income" || window.Title == "New Expenditure")
                window.Title = "Добавить";
            window.Height = WindowHeight;
            string iconPath = @"pack://application:,,,/Images/" + Type + ".ico";
            window.Icon = BitmapFrame.Create(new Uri(iconPath));
            return window;
        }

        public virtual Control GetEditControl()
        {
            throw new ArithmeticException();
        }

        public virtual decimal Money { get; }
        public virtual string FullName { get; }
        public virtual int WindowHeight { get; }
        public virtual string Type { get; }
    }
}
