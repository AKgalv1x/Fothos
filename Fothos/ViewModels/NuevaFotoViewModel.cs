using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fothos.Data;
using Fothos.Models;
using System.Collections.ObjectModel;

namespace Fothos.ViewModels
{
    public partial class NuevaFotoViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private string titulo = string.Empty;

        [ObservableProperty]
        private string descripcion = string.Empty;

        [ObservableProperty]
        private string categoriaSeleccionada;

        [ObservableProperty]
        private string rutaImagenTemporal = string.Empty;

        public ObservableCollection<string> Categorias { get; } = new() { "Viajes", "Diseño", "Bocetos", "Fotografía" };

        public NuevaFotoViewModel(DatabaseService db)
        {
            _db = db;
            CategoriaSeleccionada = Categorias[0];
        }

        [RelayCommand]
        private async Task TomarFotoAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlertAsync("Permiso denegado", "Necesitamos acceso a la cámara para tomar la foto.", "OK");
                    return;
                }
            }

            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var foto = await MediaPicker.Default.CapturePhotoAsync();
                    if (foto != null)
                    {
                        string rutaLocal = await GuardarArchivoLocalAsync(foto);
                        RutaImagenTemporal = rutaLocal;
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("No soportado", "La cámara no está disponible en este dispositivo.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task ElegirFotoAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Photos>();
                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlertAsync("Permiso denegado", "Necesitamos acceso a tus fotos.", "OK");
                    return;
                }
            }

            try
            {
                // En .NET 10, se usa PickPhotosAsync (en plural)
                var fotos = await MediaPicker.Default.PickPhotosAsync();
                var foto = fotos?.FirstOrDefault();

                if (foto != null)
                {
                    string rutaLocal = await GuardarArchivoLocalAsync(foto);
                    RutaImagenTemporal = rutaLocal;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"No se pudo elegir la foto: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(Titulo) || string.IsNullOrWhiteSpace(RutaImagenTemporal))
            {
                await Shell.Current.DisplayAlertAsync("Faltan datos", "Ponle un título y selecciona una foto.", "OK");
                return;
            }

            var nuevaFoto = new PhotoItem
            {
                Title = Titulo,
                Description = Descripcion,
                Category = CategoriaSeleccionada,
                DateTaken = DateTime.Now,
                ImagePath = RutaImagenTemporal
            };

            await _db.SavePhotoAsync(nuevaFoto);
            await Shell.Current.GoToAsync("..");
        }

        private async Task<string> GuardarArchivoLocalAsync(FileResult foto)
        {
            string nombreArchivo = $"{Guid.NewGuid()}.jpg";
            string rutaDestino = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

            using (var streamOrigen = await foto.OpenReadAsync())
            using (var streamDestino = File.Create(rutaDestino))
            {
                await streamOrigen.CopyToAsync(streamDestino);
            }

            return rutaDestino;
        }
    }
}