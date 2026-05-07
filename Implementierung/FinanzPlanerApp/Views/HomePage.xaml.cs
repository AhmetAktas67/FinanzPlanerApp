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

        var kategorien = await db.Kategorien
            .OrderBy(k => k.Name)
            .ToListAsync();

        HomeKategorienCollectionView.ItemsSource = kategorien;
    }
}
