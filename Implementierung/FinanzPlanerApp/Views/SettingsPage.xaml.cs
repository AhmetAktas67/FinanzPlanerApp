using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

    private async void KategorieHinzufuegenButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(KategorieNameEntry.Text))
        {
            await DisplayAlert("Fehler", "Bitte Kategorie eingeben", "OK");
            return;
        }

        using var db = new AppDbContext();

        Kategorie neuekategorie = new Kategorie();
        neuekategorie.Name = KategorieNameEntry.Text;

        db.Kategorien.Add(neuekategorie);
        db.SaveChanges();

        KategorieNameEntry.Text = "";

        await LadeKategorien();
    }
}