using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchForms
{
    static class Program
    {
        public static ApplicationContext ac = new ApplicationContext();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Form1 startForm = new Form1();
            ac.MainForm = startForm;
            Application.Run(ac);
        }
    }
}
