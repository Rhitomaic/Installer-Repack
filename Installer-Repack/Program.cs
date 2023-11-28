using System;
using System.Windows.Forms;

namespace Installer_Repack
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void ChangeTextAsProgram(this Control control) =>
            control.Text = control.Text.Replace("{PROGRAM}", MainForm.programName);
    }
}
