using Fothos.ViewModels;

namespace Fothos.Views
{
    public partial class NuevaFotoPage : ContentPage
    {
        public NuevaFotoPage(NuevaFotoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}