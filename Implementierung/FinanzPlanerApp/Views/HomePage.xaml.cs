using FinanzPlanerApp.Data;
using FinanzPlanerApp.Models;
using FinanzPlanerApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace FinanzPlanerApp.Views;

public partial class HomePage : ContentPage
{
    private List<DateTime> monate = new List<DateTime>();
    public HomePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LadeMonate();
    }

    private async Task LadeKategorien(int monat,int jahr)
    {
        using var db = new AppDbContext();

      


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



    private async Task LadeMonate()
    {
        using var db = new AppDbContext();

        var ausgaben = await db.Ausgaben.ToListAsync();

        monate = ausgaben
            .Select(a => new DateTime(a.Datum.Year, a.Datum.Month, 1))
            .Distinct()
            .OrderByDescending(m => m)
            .ToList();

        if (monate.Count == 0)
        {
            monate.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        }
       
        MonatPicker.SelectedIndex = -1;
        MonatPicker.ItemsSource = null;
        MonatPicker.ItemsSource = monate.Select(m => m.ToString("MMMM yyyy")).ToList();
        MonatPicker.SelectedIndex = 0;
    }





    private async void MonatPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (MonatPicker.SelectedIndex == -1)
            return;

        DateTime monat = monate[MonatPicker.SelectedIndex];

        await LadeKategorien(monat.Month, monat.Year);
    }
}
