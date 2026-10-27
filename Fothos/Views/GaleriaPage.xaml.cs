using Fothos.ViewModels;

namespace Fothos.Views
{
    public partial class GaleriaPage : ContentPage
    {
        private readonly GaleriaViewModel _viewModel;

        public GaleriaPage(GaleriaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }
        private async void OnAgregarFotoClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NuevaFotoPage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Ejecuta el comando para buscar las fotos en la base de datos
            _viewModel.CargarFotosCommand.Execute(null);
        }
        private async void OnFotoSeleccionada(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Models.PhotoItem fotoSeleccionada)
            {
                // Limpiamos la selección para que no quede marcada
                ((CollectionView)sender).SelectedItem = null;

                // Navegamos pasando el objeto foto como parámetro
                var parametros = new Dictionary<string, object>
        {
            { "Foto", fotoSeleccionada }
        };

                await Shell.Current.GoToAsync(nameof(Views.DetalleFotoPage), parametros);
            }
        }
    }
}