using FinanzPlanerApp.Views;

namespace FinanzPlanerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TutorialPage), typeof(TutorialPage));
        }
    }
}
