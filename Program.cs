using System;
using System.Windows.Forms;

namespace Pexo16
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainUI());
            
            //Application.Run(new Calibrations());
            //Application.Run(new EmailSetting());
        }

    }
}
