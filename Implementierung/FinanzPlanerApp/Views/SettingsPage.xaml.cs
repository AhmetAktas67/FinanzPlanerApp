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


        KategorieLöschenPicker.ItemsSource = kategorien;
        KategorieLöschenPicker.ItemDisplayBinding = new Binding("Name");
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

    private async void GrenzeSpeichernButton_Clicked(object sender, EventArgs e)
    {
        if (KategoriePicker.SelectedItem == null)
        {
            await DisplayAlert("Fehler", "Bitte Kategorie auswählen", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(AusgabengrenzeEntry.Text))
        {
            await DisplayAlert("Fehler", "Bitte Grenze eingeben", "OK");
            return;
        }

        decimal grenze;

        bool zahlOk = decimal.TryParse(AusgabengrenzeEntry.Text, out grenze);

        if (zahlOk == false)
        {
            await DisplayAlert("Fehler", "Bitte eine gültige Zahl eingeben", "OK");
            return;
        }

        Kategorie ausgewaehlteKategorie = (Kategorie)KategoriePicker.SelectedItem;

        using var db = new AppDbContext();

        Kategorie? kategorieAusDb = db.Kategorien
            .FirstOrDefault(k => k.KategorieId == ausgewaehlteKategorie.KategorieId);

        if (kategorieAusDb == null)
        {
            await DisplayAlert("Fehler", "Kategorie wurde nicht gefunden", "OK");
            return;
        }

        kategorieAusDb.Ausgabengrenze = grenze;

        db.SaveChanges();

        await DisplayAlert("Gespeichert", "Ausgabengrenze wurde gespeichert", "OK");
    }

    private async void KategorieLöschenButton_Clicked(object sender, EventArgs e)
    {
        if (KategorieLöschenPicker.SelectedItem == null)
        {
            await DisplayAlert("Fehler", "Bitte Kategorie zum Löschen auswählen", "OK");
            return;
        }

         Kategorie ausgewaehlteKategorie = (Kategorie)KategorieLöschenPicker.SelectedItem;

        using var db = new AppDbContext();

          Kategorie? kategorieAusDb = await db.Kategorien
            .FirstOrDefaultAsync(k => k.KategorieId == ausgewaehlteKategorie.KategorieId);

        if (kategorieAusDb == null)
        {
            await DisplayAlert("Fehler", "Kategorie wurde nicht gefunden", "OK");
            return;
        }

        db.Kategorien.Remove(kategorieAusDb);
        await db.SaveChangesAsync();

        KategorieLöschenPicker.SelectedItem = null;

        await LadeKategorien();

        await DisplayAlert("Gelöscht", "Kategorie wurde gelöscht", "OK");
    }
}