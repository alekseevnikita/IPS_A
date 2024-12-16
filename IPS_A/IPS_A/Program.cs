using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace IPS_A
{
    internal static class Program
    {
        static string ico = @"ico\s4_Mail_Agent_mail_Черный.ico";
        static Icon icoIcon = new Icon(ico);
        static NotifyIcon notifyIcon = new NotifyIcon();
        static Form1 form;
        static TaskStartStop taskStartStop;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();

            //создадим меню
            ContextMenu oMenu = new ContextMenu();
            //создадим пункт меню 1
            MenuItem menuItem1 = new MenuItem();
            oMenu.MenuItems.AddRange(new MenuItem[] { menuItem1 });
            menuItem1.Index = 0;
            //global::s4_Mail_Agent.Properties.Resources._2_16;
            menuItem1.Text = "&Выключено";  //В - кнопка по нажатию которой произойдет действие
            menuItem1.Click += new EventHandler(menuItem1_Click);


            //создадим пункт меню 2
            MenuItem menuItem2 = new MenuItem();
            oMenu.MenuItems.AddRange(new MenuItem[] { menuItem2 });
            menuItem2.Index = 1;
            menuItem2.Text = "&Информация";  //x - кнопка по нажатию которой произойдет выход
            menuItem2.Click += new EventHandler(menuItem2_Click);


            //создадим пункт меню 3
            MenuItem menuItem3 = new MenuItem();
            oMenu.MenuItems.AddRange(new MenuItem[] { menuItem3 });
            menuItem3.Index = 2;
            menuItem3.Text = "Вы&ход из программы";  //x - кнопка по нажатию которой произойдет выход
            menuItem3.Click += new EventHandler(menuItem3_Click);


            notifyIcon.Icon = icoIcon;
            notifyIcon.Text = "";
            notifyIcon.ContextMenu = oMenu;
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);


            AcyncWhile(100);

            var f = new WinApi();
            f.WindowEvent += (sender, data) => form.textBox2.AppendText(data + "\r\n");

            Application.Run();
        }

        async static void AcyncWhile(int ms)
        {
            while (true)
            {
                await Task.Delay(ms);
                if (Interop.GetAsyncKeyState(162) == -32767)
                {

                    System.Threading.Thread.Sleep(100);
                    if (Interop.GetAsyncKeyState(49) == -32767)
                    {
                        System.Threading.Thread.Sleep(200);
                        //SendKeys.SendWait((Environment.UserName + "@" + Environment.UserDomainName + ".ru").ToLower());
                        Interop.SendForWindows((Environment.UserName + "@" + Environment.UserDomainName + ".ru").ToLower());
                        SendKeys.SendWait("{Tab}");

                    }
                    if (Interop.GetAsyncKeyState(50) == -32767)
                    {
                        System.Threading.Thread.Sleep(200);
                        //SendKeys.SendWait(Settings.Restore());
                        Interop.SendForWindows(Settings.Restore());
                        SendKeys.SendWait("{Enter}");
                    }
                }
            }
        }


        private static void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //MessageBox.Show((Environment.UserName + "@" + Environment.UserDomainName + ".ru").ToLower());
                //MessageBox.Show(Settings.Restore());
                //MessageBox.Show(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            }
        }

        private static void menuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void menuItem2_Click(object sender, EventArgs e)
        {
            form.ShowDialog();
        }

        private static void menuItem1_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (sender as MenuItem);
            menuItem.Checked = !menuItem.Checked;

            

            if(menuItem.Checked)
            {
                menuItem.Text = "&Включено";
                taskStartStop = new TaskStartStop();
                taskStartStop.Start();
            }
            else
            {
                menuItem.Text = "&Выключено";
                taskStartStop.Stop();
                taskStartStop.Dispose();
            }
        }
    }
}
