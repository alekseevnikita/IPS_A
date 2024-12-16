using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace IPS_A
{
    internal class WinApi : Form
    {
        private readonly int msgNotify;
        public delegate void EventHandler(object sender, string data);
        public event EventHandler WindowEvent;
        protected virtual void OnWindowEvent(string data)
        {
            var handler = WindowEvent;
            if (handler != null)
            {
                handler(this, data);
            }
        }

        public WinApi()
        {
            // Hook on to the shell
            msgNotify = Interop.RegisterWindowMessage("SHELLHOOK");
            Interop.RegisterShellHookWindow(this.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == msgNotify)
            {
                // Receive shell messages
                switch ((Interop.ShellEvents)m.WParam.ToInt32())
                {
                    case Interop.ShellEvents.HSHELL_WINDOWCREATED:
                    case Interop.ShellEvents.HSHELL_WINDOWDESTROYED:
                    case Interop.ShellEvents.HSHELL_WINDOWACTIVATED:
                        string wName = GetWindowName(m.LParam);
                        var action = (Interop.ShellEvents)m.WParam.ToInt32();

                        Process process = null;
                        try
                        {
                            uint processID = 0;
                            uint threadID = Interop.GetWindowThreadProcessId(m.LParam, out processID);
                            process = Process.GetProcessById(Convert.ToInt32(processID));
                        }
                        catch { }

                        if (wName == "S-Terra Client v.4.3 - Login")
                        {
                            //int ret = Interop.LoadKeyboardLayout("00000409", 1);
                            //Interop.PostMessage(Interop.GetForegroundWindow(), 0x50, 1, ret);

                            ////если язык русский то и раскладку испольхуем такую же
                            //if(Interop.GetKeyboardLayout(Interop.GetWindowThreadProcessId(Interop.GetForegroundWindow(), IntPtr.Zero)).ToString() ==  "68748313")
                            //    InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
                            ////InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));

                            //Interop.SetForegroundWindow(m.LParam);
                            //foreach (char ch in "user")
                            //{
                            //    if (Interop.SetForegroundWindow(m.LParam) != 0) SendKeys.Send(ch.ToString());
                            //}
                            Interop.SendForWindows(m.LParam, "user");
                            if (Interop.SetForegroundWindow(m.LParam) != 0) SendKeys.Send("{Enter}");
                        }
                        if (wName == "XAuth request dialog" | wName == "Безымянный - Блокнот")
                        {
                            Interop.SendForWindows(m.LParam, (Environment.UserName + "@" + Environment.UserDomainName + ".ru").ToLower());
                            if (Interop.SetForegroundWindow(m.LParam) != 0) SendKeys.Send("{Tab}");
                            Interop.SendForWindows(m.LParam, Settings.Restore());
                            if (Interop.SetForegroundWindow(m.LParam) != 0) SendKeys.Send("{Enter}");
                        }

                        try
                        {
                            OnWindowEvent(string.Format("{0} - {1}: {2} ({3})", action, m.LParam, wName, 1)); //process.ProcessName
                        }
                        catch { }
                        break;
                }
            }
            base.WndProc(ref m);
        }



        private string GetWindowName(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder();
            int longi = Interop.GetWindowTextLength(hwnd) + 1;
            sb.Capacity = longi;
            Interop.GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            try { Interop.DeregisterShellHookWindow(this.Handle); }
            catch { }
            base.Dispose(disposing);
        }
    }
}
