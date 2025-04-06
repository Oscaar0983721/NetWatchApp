using System;
using System.Windows.Forms;
using NetWatchApp.Services;
using System.IO;

namespace NetWatchApp
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

            try
            {
                // Initialize JSON data service
                Console.WriteLine("Initializing JSON data storage...");
                var jsonDataService = new JsonDataService();
                Console.WriteLine("JSON data storage initialized successfully.");

                // Initialize and run data seeder
                Console.WriteLine("Checking if data seeding is needed...");
                var dataSeeder = new Data.SeedData.DataSeeder();
                dataSeeder.Seed();
                Console.WriteLine("Data seeding process completed.");

                // Start the application
                Application.Run(new Forms.LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}\n\n{ex.StackTrace}",
                    "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

