using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;

using Microsoft.EntityFrameworkCore;

namespace FinanzPlanerApp.Views;

public partial class AusgabeHinzufügenPage : ContentPage
{

    private int ausgabeid = 0;
    private List<Kategorie> kategorien = new List<Kategorie>();
    public AusgabeHinzufügenPage()
	{
		InitializeComponent();
	}


    public AusgabeHinzufügenPage(int id)
    {
        InitializeComponent();
        ausgabeid = id;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        using var db = new AppDbContext();

         kategorien = await db.Kategorien.ToListAsync();

        KategoriePicker.ItemsSource = kategorien;
       
        KategoriePicker.ItemDisplayBinding = new Binding("Name");


        if (ausgabeid != 0)
        {
           var ausgabe = db.Ausgaben.FirstOrDefault(x => x.AusgabenID == ausgabeid);

            if (ausgabe != null)
            {
                BetragEntry.Text = ausgabe.Betrag.ToString();

                DatumPicker.Date = ausgabe.Datum;

                BeschreibungEditor.Text = ausgabe.Beschreibung;


                var kategorie = kategorien.FirstOrDefault(x => x.KategorieId == ausgabe.KategorieID);
                KategoriePicker.SelectedItem = kategorie;
            }
        }
    }


    private async void BestätigenButton_Clicked(object sender, EventArgs e)
    {
        using var db = new AppDbContext();

        if (BetragEntry.Text == null || KategoriePicker.SelectedItem == null)
        {
            await DisplayAlert("Fehler", "Bitte alles ausfüllen", "OK");
            return;
        }

        Kategorie kategorie = (Kategorie)KategoriePicker.SelectedItem;

        if (ausgabeid == 0)
        {
            Ausgabe ausgabe = new Ausgabe();

            ausgabe.Betrag = decimal.Parse(BetragEntry.Text);
            ausgabe.Datum = DatumPicker.Date;
            ausgabe.Beschreibung = BeschreibungEditor.Text;
            ausgabe.KategorieID = kategorie.KategorieId;


            db.Ausgaben.Add(ausgabe);
        }
        else
        {
            var ausgabe = db.Ausgaben.FirstOrDefault(x => x.AusgabenID == ausgabeid);

            if (ausgabe != null)
            {
                ausgabe.Betrag = decimal.Parse(BetragEntry.Text);
                ausgabe.Datum = DatumPicker.Date;
                ausgabe.Beschreibung = BeschreibungEditor.Text;
                ausgabe.KategorieID = kategorie.KategorieId;
            }
        }
           

       
      

        db.SaveChanges();

        await Navigation.PopAsync();
    }
}