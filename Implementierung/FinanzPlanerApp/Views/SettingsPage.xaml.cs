using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinanzPlanerApp.Views;


public partial class SettingsPage : ContentPage
{
    private string ausgewaehlteFarbe = "";

    public List<string> farbCodes { get; set; } = new List<string>
   {
       "#E53935", // Rot
        "#D81B60", // Pink
        "#8E24AA", // Lila
        "#5E35B1", // Dunkellila
        "#3949AB", // Indigo
        "#1E88E5", // Blau
        "#039BE5", // Hellblau
        "#00ACC1", // Cyan
        "#00897B", // Türkis
        "#43A047", // Grün
        "#7CB342", // Hellgrün
        "#C0CA33", // Limette
        "#FDD835", // Gelb
        "#FFB300", // Orange
        "#F4511E", // Dunkelorange
        "#EC407A", // Rosa
        "#AB47BC", // Violett
        "#7E57C2", // Lavendel
        "#42A5F5", // Himmelblau
        "#26C6DA", // Aqua
        "#26A69A", // Mint
        "#66BB6A", // Grün
        "#9CCC65", // Hellgrün
        "#FFA726", // Orange
        "#EF5350"  // Hellrot
    };
    public SettingsPage()
	{
		InitializeComponent();
        FarbPaletteCollectionView.ItemsSource = farbCodes;
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

        if (ausgewaehlteFarbe == "")
        {
            await DisplayAlert("Fehler", "Bitte Farbe auswählen", "OK");
            return;
        }


        using var db = new AppDbContext();

        Kategorie neuekategorie = new Kategorie();
        neuekategorie.Name = KategorieNameEntry.Text;
        neuekategorie.Farbe = ausgewaehlteFarbe;

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

        await PruefeAusgabengrenze(kategorieAusDb.KategorieId);
    }



    private async Task PruefeAusgabengrenze(int kategorieId)
    {
        using var db = new AppDbContext();

        Kategorie? kategorie = await db.Kategorien
            .FirstOrDefaultAsync(k => k.KategorieId == kategorieId);

        if (kategorie == null)
        {
            return;
        }

        if (kategorie.Ausgabengrenze == null || kategorie.Ausgabengrenze <= 0)
        {
            return;
        }

        DateTime startMonat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime endeMonat = startMonat.AddMonths(1);

        decimal summeAusgaben = await db.Ausgaben
            .Where(a => a.KategorieID == kategorieId)
            .Where(a => a.Datum >= startMonat && a.Datum < endeMonat)
            .SumAsync(a => a.Betrag);

        if (summeAusgaben > kategorie.Ausgabengrenze)
        {
            await DisplayAlert(
                "Ausgabengrenze überschritten",
                $"Die Kategorie \"{kategorie.Name}\" hat die Grenze überschritten.\n\n" +
                $"Grenze: {kategorie.Ausgabengrenze:0.00} €\n" +
                $"Ausgegeben: {summeAusgaben:0.00} €",
                "OK"
            );
        }
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


    private void FarbPaletteCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FarbPaletteCollectionView.SelectedItem == null)
            return;

        ausgewaehlteFarbe = FarbPaletteCollectionView.SelectedItem.ToString();
    }

    private void KategoriePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (KategoriePicker.SelectedItem == null)
            return;

        Kategorie kategorie = (Kategorie)KategoriePicker.SelectedItem;

        KategoriePicker.BackgroundColor = Color.FromArgb(kategorie.Farbe).WithAlpha(0.35f);
    }





}