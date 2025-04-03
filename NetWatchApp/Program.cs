using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Data.SeedData;
using NetWatchApp.Forms;
using System;
using System.IO;
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

            try
            {
                // Inicializar base de datos y sembrar datos
                Log("Inicializando base de datos y sembrando datos...");
                using (var context = new NetWatchDbContext())
                {
                    var seeder = new DataSeeder(context);
                    seeder.Seed();
                }
                Log("Base de datos inicializada correctamente.");

                // Iniciar con el formulario de login
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error al iniciar la aplicación: {ex.Message}\n\nDetalles: {ex.StackTrace}";
                MessageBox.Show(errorMessage, "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log(errorMessage);
            }
        }

        /// <summary>
        /// Método para registrar logs en un archivo de texto
        /// </summary>
        private static void Log(string message)
        {
            string logPath = "app_log.txt";
            File.AppendAllText(logPath, $"{DateTime.Now}: {message}\n");
        }
    }
}
