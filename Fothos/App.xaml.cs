using Microsoft.Extensions.DependencyInjection;

namespace Fothos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Usamos el ServiceProvider para obtener la página con sus dependencias
            var loginPage = IPlatformApplication.Current!.Services.GetService<Views.LoginPage>()!;
            return new Window(loginPage);
        }
    }
}