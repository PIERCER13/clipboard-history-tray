// @author piercer
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PiercerClipboardTray
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            var history = new List<string>();
            var menu = new ContextMenuStrip();
            var tray = new NotifyIcon { Icon = SystemIcons.Application, Text = "Piercer Clipboard History", Visible = true, ContextMenuStrip = menu };

            Action rebuildMenu = () =>
            {
                menu.Items.Clear();
                foreach (string item in history)
                {
                    string copy = item;
                    string label = copy.Length > 48 ? copy.Substring(0, 48).Replace("\n", " ") + "..." : copy.Replace("\n", " ");
                    menu.Items.Add(label, null, (s, e) => Clipboard.SetText(copy));
                }
                menu.Items.Add(new ToolStripSeparator());
                menu.Items.Add("Limpar", null, (s, e) => { history.Clear(); menu.Items.Clear(); });
                menu.Items.Add("Sair", null, (s, e) => { tray.Visible = false; Application.Exit(); });
            };

            string last = "";
            var timer = new Timer { Interval = 700 };
            timer.Tick += (s, e) =>
            {
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        string cur = Clipboard.GetText();
                        if (!string.IsNullOrEmpty(cur) && cur != last)
                        {
                            last = cur;
                            history.Remove(cur);
                            history.Insert(0, cur);
                            if (history.Count > 20) history.RemoveAt(history.Count - 1);
                            rebuildMenu();
                        }
                    }
                }
                catch { }
            };
            timer.Start();
            rebuildMenu();
            Application.Run();
        }
    }
}
