using FinanzPlanerApp.Data;
using FinanzPlanerApp.Views;

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
            bool tutorialGesehen = Preferences.Get("TutorialGesehen", false);

            if (tutorialGesehen == false)
            {
                return new Window(new NavigationPage(new TutorialPage()));
            }

            return new Window(new AppShell());
        }

      
    }
}