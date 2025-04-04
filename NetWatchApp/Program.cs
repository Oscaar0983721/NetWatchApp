using System;
using System.Windows.Forms;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
                // Initialize database and seed data
                Console.WriteLine("Initializing database...");
                ForceRecreateDatabase();
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

        private static void ForceRecreateDatabase()
        {
            try
            {
                // Forzar la eliminación de la base de datos existente
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetWatchApp.db");

                // Cerrar cualquier conexión abierta
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Eliminar el archivo si existe
                if (File.Exists(dbPath))
                {
                    try
                    {
                        File.Delete(dbPath);
                        Console.WriteLine("Base de datos existente eliminada.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No se pudo eliminar la base de datos: {ex.Message}");

                        // Si no se puede eliminar, intentar con otro nombre
                        dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetWatchApp_new.db");
                        Console.WriteLine($"Usando ruta alternativa: {dbPath}");
                    }
                }

                // Crear una nueva instancia del contexto
                using (var context = new NetWatchDbContext())
                {
                    // Asegurar que la base de datos se crea con el esquema correcto
                    context.Database.EnsureDeleted(); // Intenta eliminar la base de datos a nivel de EF Core
                    context.Database.EnsureCreated();
                    Console.WriteLine("Base de datos creada correctamente.");

                    // Sembrar datos iniciales
                    Console.WriteLine("Iniciando siembra de datos...");
                    var seeder = new DataSeeder(context);
                    seeder.Seed();
                    Console.WriteLine("Siembra de datos completada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Re-lanzar para mostrar el error al usuario
            }
        }
    }
}

