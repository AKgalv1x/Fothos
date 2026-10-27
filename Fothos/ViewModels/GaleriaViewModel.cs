using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fothos.Data;
using Fothos.Models;
using System.Collections.ObjectModel;

namespace Fothos.ViewModels
{
    public partial class GaleriaViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        // ObservableCollection es ideal para listas: si agregas o quitas algo, la UI se actualiza sola.
        [ObservableProperty]
        private ObservableCollection<PhotoItem> photos;

        [ObservableProperty]
        private bool isBusy;

        public GaleriaViewModel(DatabaseService db)
        {
            _db = db;
            Photos = new ObservableCollection<PhotoItem>();
        }

        // Este comando se ejecuta cuando la página aparece
        [RelayCommand]
        private async Task CargarFotosAsync()
        {
            IsBusy = true;
            try
            {
                // Traemos todas las fotos de la base de datos
                var lista = await _db.GetPhotosAsync();

                Photos.Clear();
                foreach (var foto in lista)
                {
                    Photos.Add(foto);
                }
            }
            catch (Exception ex)
            {
                // Manejo de error simple
                Console.WriteLine($"Error al cargar fotos: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}