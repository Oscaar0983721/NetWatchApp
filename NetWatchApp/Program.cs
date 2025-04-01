using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Data.SeedData;
using NetWatchApp.Forms;
using System;
using System.Windows.Forms;

namespace NetWatchApp
{
    internal static class Program
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

            // Initialize database and seed data
            using (var context = new NetWatchDbContext())
            {
                context.Database.EnsureCreated();
                DataSeeder.SeedData(context);
            }

            Application.Run(new LoginForm());
        }
    }
}