using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// This class simply makes a new form application and presents it to the spreadsheet panel class
/// @Author: Ajay Bagali && Ritesh Sharma
/// </summary>
namespace SpreadsheetGUI
{

    class FormApplication : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static FormApplication appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private FormApplication()
        {
        }

        /// <summary>
        /// Returns the one FormApplicationContext.
        /// </summary>
        public static FormApplication getAppContext()
        {
            if (appContext == null)
            {
                appContext = new FormApplication();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }

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
            Application.Run(new Form1());
        }
    }
}
