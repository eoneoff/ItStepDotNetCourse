using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace Organizer
{
    class AlarmChecker
    {
        private NotifyIcon TrayIcon;
        private ContextMenuStrip menu;
        private ToolStripMenuItem open;
        private ToolStripMenuItem close;

        private Thread checker;

        private bool running = true;

        public AlarmChecker()
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = new System.Drawing.Icon("trayicon.ico");
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

        private void CheckAlarms()
        {
            string message = String.Empty;
            using (organizerEntities db = new organizerEntities())
            {
                var alarms = db.Alarm.Include("Event").Where(a => a.AlarmTriggerTime < DateTime.Now).ToList();
                foreach(var a in alarms)
                {
                    message += a.AlarmTriggerTime.ToString("dd MMMM yyyy|HH:mm")+"\n";
                    foreach(var e in a.Event)
                    {
                        message += e.Name + "\n\n";
                    }

                    db.Entry(a).State = System.Data.Entity.EntityState.Deleted;

                    db.SaveChanges();
                }
            }

            if (!String.IsNullOrEmpty(message))
            {
                TrayIcon.BalloonTipText = message;
                TrayIcon.ShowBalloonTip(5000); 
            }
        }

        private void CheckerServise()
        {
            while (running)
            {
                (new Thread(CheckAlarms)).Start();
                Thread.Sleep(60000);
            }
        }

        public void Start()
        {
            TrayIcon.Visible = true;
            checker = new Thread(CheckerServise);
            checker.Start();
        }

        public void Stop()
        {
            TrayIcon.Visible = false;
            TrayIcon.Dispose();
            if (checker != null && checker.IsAlive)
                running = false;
        }

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

        private void CloseItem_Click(object sender, EventArgs e)
        {
            Stop();
            if (MainWindow.MainView != null)
            {
                MainWindow.MainView.Close();
            }
            System.Windows.Application.Current.Shutdown();
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            if (MainWindow.MainView.WindowState == System.Windows.WindowState.Normal)
                MainWindow.MainView.WindowState = System.Windows.WindowState.Minimized;
            else if (MainWindow.MainView.WindowState == System.Windows.WindowState.Minimized)
                MainWindow.MainView.WindowState = System.Windows.WindowState.Normal;
        }
    }
}
