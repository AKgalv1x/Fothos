namespace Fothos
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.NuevaFotoPage), typeof(Views.NuevaFotoPage));
            Routing.RegisterRoute(nameof(Views.DetalleFotoPage), typeof(Views.DetalleFotoPage));
        }
    }
}
