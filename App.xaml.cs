using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using My_Pos.DbContexts;
using My_Pos.Services;
using My_Pos.ViewModels;
using My_Pos.Views;
using System;
using System.Configuration; // Add this
using System.Windows;

namespace My_Pos
{
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var services = new ServiceCollection();

                // Configure DbContext with connection string
                ConfigureDbContext(services);

                // Register application services
                //RegisterServices(services);

                // Register ViewModels
                RegisterViewModels(services);

                // Register Views
                RegisterViews(services);

                // Build service provider
                ServiceProvider = services.BuildServiceProvider();

                // Initialize and show main window
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                HandleStartupError(ex);
            }
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            // Read connection string from App.config
            string? connectionString = ConfigurationManager
                .ConnectionStrings["SqlServerConnection"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string not found in App.config.");
            }

            services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(connectionString),
    contextLifetime: ServiceLifetime.Scoped,
    optionsLifetime: ServiceLifetime.Scoped
);

            services.AddScoped<ProductService>();


        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ProductService>();

            // Add other services here
        }

        private void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<ProductViewModel>();
            services.AddTransient<SizeViewModel>();
            services.AddTransient<CrustViewModel>();
            // Add other ViewModels here
        }

        private void RegisterViews(IServiceCollection services)
        {
            services.AddTransient<ProductView>();
            services.AddTransient<LoginView>();
            services.AddTransient<SizeView>();
            services.AddTransient<CrustView>();
            // Add other Views here
        }

        private void ShowMainWindow()
        {
            var loginView = ServiceProvider?.GetRequiredService<LoginView>();
            loginView?.Show();
        }

        private void HandleStartupError(Exception ex)
        {
            MessageBox.Show(
                $"Failed to initialize application:\n{ex.Message}",
                "Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );

            Shutdown(1);
        }
    }
}
