using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WetalkAPP.Pages.Auth;
using WetalkAPP.Services;

namespace WetalkAPP
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // api service
            var APIService = new APIService();
            APIService.InitializeAPIService();

            Application.Run(new Login());
        }
    }
}
