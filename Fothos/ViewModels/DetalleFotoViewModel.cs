using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fothos.Data;
using Fothos.Models;
using System.Collections.ObjectModel;

namespace Fothos.ViewModels
{
    // Implementamos IQueryAttributable para recibir la foto desde la Galería
    public partial class DetalleFotoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private PhotoItem fotoActual = new();

        // Propiedades para los campos editables
        [ObservableProperty]
        private string tituloEditado = string.Empty;

        [ObservableProperty]
        private string descripcionEditada = string.Empty;

        [ObservableProperty]
        private string categoriaSeleccionada = string.Empty;

        public ObservableCollection<string> Categorias { get; } = new() { "Viajes", "Diseño", "Bocetos", "Fotografía" };

        public DetalleFotoViewModel(DatabaseService db)
        {
            _db = db;
        }

        // Este método se ejecuta automáticamente cuando navegas a esta página con parámetros
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Foto"))
            {
                FotoActual = (PhotoItem)query["Foto"];

                // Llenamos los campos de edición con los datos actuales
                TituloEditado = FotoActual.Title;
                DescripcionEditada = FotoActual.Description;
                CategoriaSeleccionada = FotoActual.Category;
            }
        }

        [RelayCommand]
        private async Task GuardarCambiosAsync()
        {
            if (string.IsNullOrWhiteSpace(TituloEditado))
            {
                await Shell.Current.DisplayAlertAsync("Atención", "El título no puede estar vacío.", "OK");
                return;
            }

            // Actualizamos el objeto
            FotoActual.Title = TituloEditado;
            FotoActual.Description = DescripcionEditada;
            FotoActual.Category = CategoriaSeleccionada;

            // Guardamos en SQLite
            await _db.SavePhotoAsync(FotoActual);

            await Shell.Current.DisplayAlertAsync("Éxito", "Foto actualizada correctamente.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task EliminarAsync()
        {
            // Requisito: Confirmación antes de borrar usando DisplayAlert
            bool confirmar = await Shell.Current.DisplayAlertAsync(
                "Eliminar Foto",
                "¿Estás seguro de que quieres eliminar esta foto permanentemente?",
                "Sí, eliminar",
                "Cancelar");

            if (confirmar)
            {
                await _db.DeletePhotoAsync(FotoActual);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}