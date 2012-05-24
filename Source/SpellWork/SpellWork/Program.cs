using System;
using System.IO;
using System.Windows.Forms;

namespace SpellWork
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists("./dbc"))
            {
                MessageBox.Show("Could not find DBC files, please place them in a folder called dbc within the executable directory");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new Loader();
            Application.Run(new FormMain());
        }
    }
}