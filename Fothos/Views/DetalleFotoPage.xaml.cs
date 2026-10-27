using Fothos.ViewModels;

namespace Fothos.Views
{
    public partial class DetalleFotoPage : ContentPage
    {
        public DetalleFotoPage(DetalleFotoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}