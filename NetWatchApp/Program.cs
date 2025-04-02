using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Data.SeedData;
using NetWatchApp.Forms;
using System;
using System.Windows.Forms;

namespace NetWatchApp
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

            // Initialize database and seed data
            using (var context = new NetWatchDbContext())
            {
                var seeder = new DataSeeder(context);
                seeder.Seed();
            }

            // Start with login form
            Application.Run(new LoginForm());
        }
    }
}

