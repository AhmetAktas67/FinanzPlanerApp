using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;
using FinanzPlanerApp.Views;
using Microsoft.EntityFrameworkCore;

namespace FinanzPlanerApp.Views;

public partial class AusgabenPage : ContentPage
{
	public AusgabenPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LadeAusgaben();
    }

    private async Task LadeAusgaben()
    {
        using var db = new AppDbContext();

        var ausgaben = await db.Ausgaben
            .Include(a => a.Kategorie)
            .OrderByDescending(a => a.Datum)
            .ToListAsync();

        AusgabenCollectionView.ItemsSource = ausgaben;
    }

    private async void AusgabeHinzufügen_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AusgabeHinzufügenPage());
    }
}