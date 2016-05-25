using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SQLComparator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
         //   MessageBox.Show(Args.Length.ToString());
            foreach (string s in Args)
            {
          //      MessageBox.Show(s);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}