using System;
using System.Windows.Forms;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                // Initialize database and seed data
                Console.WriteLine("Initializing database...");
                InitializeDatabase();
                Console.WriteLine("Database initialization completed.");

                // Start the application
                Application.Run(new Forms.LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}\n\n{ex.StackTrace}",
                    "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void InitializeDatabase()
        {
            try
            {
                using (var context = new NetWatchDbContext())
                {
                    // Ensure database is created
                    context.Database.EnsureCreated();
                    Console.WriteLine("Database created or already exists.");

                    // Check if we need to seed data
                    bool needsSeed = !context.Users.Any();

                    if (needsSeed)
                    {
                        Console.WriteLine("Database needs seeding. Starting data seeder...");
                        var seeder = new DataSeeder(context);
                        seeder.Seed();
                        Console.WriteLine("Database seeding completed.");
                    }
                    else
                    {
                        Console.WriteLine("Database already contains data. Skipping seed operation.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Re-throw to show error to user
            }
        }
    }
}

