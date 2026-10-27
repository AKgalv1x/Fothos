using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fothos.Data;

namespace Fothos.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        // [ObservableProperty] genera automáticamente propiedades que avisan a la UI cuando cambian
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isBusy; // Para mostrar un indicador de carga

        public LoginViewModel(DatabaseService db)
        {
            _db = db;
        }

        // [RelayCommand] convierte este método en un comando que se puede enlazar (Bind) a un botón en el XAML
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Por favor, ingresa usuario y contraseña.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                // Inicializamos la BD (por si es la primera vez que se abre)
                await _db.InitAsync();

                // Verificamos credenciales
                var user = await _db.LoginAsync(Username, Password);

                if (user != null)
                {
                    // Login exitoso: Navegar a la Galería
                    // (Nota: Application.Current.MainPage está obsoleto, usaremos la forma moderna)
                    if (Application.Current?.Windows.Count > 0)
                    {
                        Application.Current.Windows[0].Page = new AppShell();
                    }
                }
                else
                {
                    ErrorMessage = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error de conexión: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}