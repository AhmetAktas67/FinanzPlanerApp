using FinanzPlanerApp.Data;
namespace FinanzPlanerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            using var db = new AppDbContext();
            db.InitializeDatabase();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}