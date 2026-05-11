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

    private async void AusgabeLöschen_Invoked(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        var ausgaben = item.BindingContext as Ausgabe;

        if (ausgaben == null)
            return;

         
        using var db = new AppDbContext();

        var ausgabeDb= db.Ausgaben.FirstOrDefault(x=>x.AusgabenID==ausgaben.AusgabenID);

        if (ausgabeDb != null) 
        {
            db.Ausgaben.Remove(ausgabeDb);
            db.SaveChanges();
        }

        await LadeAusgaben();
    }

    private async void AusgabeBearbeiten_Tapped(object sender, EventArgs e)
    {
        var grid = sender as Grid;
        var ausgabe = grid.BindingContext as Ausgabe;

        if (ausgabe == null)
            return;

        await Navigation.PushAsync(new AusgabeHinzufügenPage(ausgabe.AusgabenID));
    }
}