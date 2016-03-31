using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ImageViewer
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int SW_SHOWMAXIMIZED = 3;
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public Form1()
        {
            InitializeComponent();
            //ControlBox = false;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1])) pictureBox1.ImageLocation = args[1];
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
                p.Kill();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            pictureBox1.ImageLocation = ((string[])(e.Data.GetData(DataFormats.FileDrop, true)))[0];
        }

        private void maximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
                ShowWindow(p.MainWindowHandle, SW_SHOWMAXIMIZED);
        }
    }
}
