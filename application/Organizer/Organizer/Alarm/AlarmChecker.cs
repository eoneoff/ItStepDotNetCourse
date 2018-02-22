using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Organizer
{
    ///Класс отвечает за фоновую проверку событий и будильников
    class AlarmChecker
    {
        private NotifyIcon TrayIcon;
        private ContextMenuStrip menu;
        private ToolStripMenuItem open;
        private ToolStripMenuItem close;
        private DateTime lastChecked = DateTime.MinValue;//Время последней проверки событий

        private Thread checker;

        private bool running = true;

        public AlarmChecker()
        {
            //Создание иконки в трее
            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = new System.Drawing.Icon("Images/trayicon.ico");
            TrayIcon.BalloonTipTitle = "Напоминания";

            menu = new ContextMenuStrip();
            open = new ToolStripMenuItem();
            close = new ToolStripMenuItem();
            menu.SuspendLayout();

            open.Name = "OpenApp";
            open.Size = new Size(152, 22);
            open.Text = "Открыть программу";
            open.Click += OpenItem_Click;

            close.Name = "CloseTray";
            close.Size = new Size(152, 22);
            close.Text = "Закрыть программу";
            close.Click += CloseItem_Click;

            menu.Items.AddRange(new ToolStripItem[] { open, close});
            menu.Name = "TrayMenu";
            menu.Size = new Size(153, 46);

            menu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = menu;
            TrayIcon.DoubleClick += TrayIcon_DoubleClick;

        }

        //Проверка будильников и событий
        private void CheckAlarms()
        {
            string message = String.Empty;
            using (organizerEntities db = new organizerEntities())
            {
                var alarms = db.Alarm.Include("Event").Where(a => a.AlarmTriggerTime < DateTime.Now).OrderBy(a=>a.AlarmTriggerTime).ToList();
                foreach(var a in alarms)
                {
                    message += a.AlarmTriggerTime.ToString("dd MMMM yyyy|HH:mm")+"\n";
                    foreach(var e in a.Event)
                    {
                        message += e.Name + "\n\n";
                    }

                    db.Entry(a).State = System.Data.Entity.EntityState.Deleted;//Сработавшие будильники удаляются

                    db.SaveChanges();
                }

                var notDoneEvents = db.Schedule.Include("Event").
                    Where(s => s.TimeStamp > lastChecked && s.TimeStamp <= DateTime.Now && s.Event.Done == false).
                    OrderBy(s=>s.TimeStamp).ToList();
                foreach(var s in notDoneEvents)
                {
                    message += s.TimeStamp.ToString("dd MMMM yyyy|HH:mm")+"\n"+
                        s.Event.Name+"\n* * *\n";
                }

                //Первая проверка возвращает все невыполненные события до текущего момента
                //последующие невыполненные события с момента последней проверки

                lastChecked = DateTime.Now;
            }

            if (!String.IsNullOrEmpty(message))
            {
                TrayIcon.BalloonTipText = message;
                TrayIcon.ShowBalloonTip(5000); 
            }
        }

        //Процесс, регулярно запускающий проверку событий в отдельном потоке
        private void CheckerServise()
        {
            while (running)
            {
                (new Thread(CheckAlarms)).Start();
                Thread.Sleep(60000);
            }
        }

        //Запуск процесса запуска проверки в отдельном потоке
        public void Start()
        {
            TrayIcon.Visible = true;
            checker = new Thread(CheckerServise);
            checker.Start();
        }


        //Отсановка проверки и закрытие приложения
        public void Stop()
        {
            TrayIcon.Visible = false;
            TrayIcon.Dispose();
            if (checker != null && checker.IsAlive)
                running = false;
        }


        //Пункт меню, открывающий окно приложения
        private void OpenItem_Click(object sender, EventArgs e)
        {
            if (MainWindow.MainView != null && MainWindow.MainView.WindowState == System.Windows.WindowState.Minimized)
                MainWindow.MainView.WindowState = System.Windows.WindowState.Normal;
            else
            {
                MainWindow window = new MainWindow();
                window.Show();
            }
        }


        //Пункт меню, завершающий работу приложения
        private void CloseItem_Click(object sender, EventArgs e)
        {
            Stop();
            if (MainWindow.MainView != null)
            {
                MainWindow.MainView.Close();
            }
            System.Windows.Application.Current.Shutdown();
        }


        //Двойно щелчок по иконке в трее показывает или скрывает приложение
        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            if (MainWindow.MainView.WindowState == System.Windows.WindowState.Normal)
                MainWindow.MainView.WindowState = System.Windows.WindowState.Minimized;
            else if (MainWindow.MainView.WindowState == System.Windows.WindowState.Minimized)
                MainWindow.MainView.WindowState = System.Windows.WindowState.Normal;
        }
    }
}
