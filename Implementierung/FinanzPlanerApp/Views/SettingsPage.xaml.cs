using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;
using FinanzPlanerApp.Views;
using Microsoft.EntityFrameworkCore;

namespace FinanzPlanerApp.Views;


public partial class SettingsPage : ContentPage
{
	public SettingsPage()
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

        KategoriePicker.ItemsSource = kategorien;
        KategoriePicker.ItemDisplayBinding = new Binding("Name");
    }
}