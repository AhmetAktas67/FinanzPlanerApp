using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;

using Microsoft.EntityFrameworkCore;

namespace FinanzPlanerApp.Views;

public partial class AusgabeHinzufügenPage : ContentPage
{
	public AusgabeHinzufügenPage()
	{
		InitializeComponent();
	}


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        using var db = new AppDbContext();

        var kategorien = await db.Kategorien.ToListAsync();

        KategoriePicker.ItemsSource = kategorien;
       
        KategoriePicker.ItemDisplayBinding = new Binding("Name");
    }


    private async void BestätigenButton_Clicked(object sender, EventArgs e)
    {
        if (BetragEntry.Text == null || KategoriePicker.SelectedItem == null)
        {
            await DisplayAlert("Fehler", "Bitte alles ausfüllen", "OK");
            return;
        }

        Kategorie kategorie = (Kategorie)KategoriePicker.SelectedItem;

        Ausgabe ausgabe = new Ausgabe();

        ausgabe.Betrag = decimal.Parse(BetragEntry.Text);
        ausgabe.Datum = DatumPicker.Date;
        ausgabe.Beschreibung = BeschreibungEditor.Text;
        ausgabe.KategorieID = kategorie.KategorieId;

        using var db = new AppDbContext();

        db.Ausgaben.Add(ausgabe);
        db.SaveChanges();

        await Navigation.PopAsync();
    }
}