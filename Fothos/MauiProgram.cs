using Fothos.Data;
using Fothos.ViewModels;
using Microsoft.Extensions.Logging;

namespace Fothos
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // INYECCIÓN DE DEPENDENCIAS
            builder.Services.AddSingleton<Data.DatabaseService>();

            // AddTransient: Se crea una nueva instancia cada vez que se necesita.
            builder.Services.AddTransient<ViewModels.LoginViewModel>();
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<ViewModels.GaleriaViewModel>();
            builder.Services.AddTransient<Views.GaleriaPage>();
            builder.Services.AddTransient<ViewModels.NuevaFotoViewModel>();
            builder.Services.AddTransient<Views.NuevaFotoPage>();
            builder.Services.AddTransient<ViewModels.DetalleFotoViewModel>();
            builder.Services.AddTransient<Views.DetalleFotoPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}