namespace FinanzPlanerApp.Views;

public partial class TutorialPage : ContentPage
{
	public TutorialPage()
	{
		InitializeComponent();
	}


    private void TutorialCarousel_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (e.CurrentPosition == 0)
        {
            TutorialTitelLabel.Text = "Willkommen bei FinanzPlaner";
            TutorialTextLabel.Text = "Behalte deine monatlichen Ausgaben einfach im Blick.";
        }

        if (e.CurrentPosition == 1)
        {
            TutorialTitelLabel.Text = "Ausgaben hinzufügen";
            TutorialTextLabel.Text = "Mit dem + Button kannst du Ausgaben erstellen, eine Kategorie wählen und den Betrag speichern.";
        }

        if (e.CurrentPosition == 2)
        {
            TutorialTitelLabel.Text = "Grenzen setzen";
            TutorialTextLabel.Text = "Lege Ausgabengrenzen in den Einstellungen fest und erhalte eine Warnung, wenn du darüber liegst.";
        }



        if (e.CurrentPosition == 2)
        {
            SkipButton.IsVisible = false;
            StartButton.IsVisible = true;
        }
        else
        {
            SkipButton.IsVisible = true;
            StartButton.IsVisible = false;
        }
    }

    private void SkipButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("TutorialGesehen", true);
        Application.Current.MainPage = new AppShell();
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("TutorialGesehen", true);
        Application.Current.MainPage = new AppShell();
    }
}
