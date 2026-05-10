using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;
using FinanzPlanerApp.Views;
using Microsoft.EntityFrameworkCore;
namespace FinanzPlanerApp.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LadeKategorien();
    }

    private async Task LadeKategorien()
    {
        using var db = new AppDbContext();

        int monat = DateTime.Now.Month;
        int jahr = DateTime.Now.Year;


        var ausgaben = await db.Ausgaben
            .Include(a=>a.Kategorie)
              .Where(a => a.Datum.Month == monat && a.Datum.Year == jahr)
            .ToListAsync();

        decimal gesamt = ausgaben.Sum(a => a.Betrag);

        GbLabel.Text = gesamt.ToString("0.00") + "€";

        var kategorien = ausgaben
             .GroupBy(a => a.Kategorie.Name)
             .Select(g => new
             {
                 Name = g.Key,
                 Summe = g.Sum(a => a.Betrag)
             })
             .ToList();

        HomeKategorienCollectionView.ItemsSource = kategorien;
    }
}
