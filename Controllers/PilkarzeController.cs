using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
namespace Kluby.Controllers;

public class PilkarzeController : Controller {
    private void SetViewDataFromSession() {
        if (HttpContext.Session.GetString("username") == null) {
            ViewData["Username"] = "";
            ViewData["IsAdmin"] = "";

            return;
        }

        ViewData["Username"] = HttpContext.Session.GetString("username");
        ViewData["IsAdmin"] = HttpContext.Session.GetString("isadmin");
    }

    public List<Pilkarze> ReadCsvFile(string filePath, char separator)
    {
        List<Pilkarze> data = new List<Pilkarze>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine();
            string[] headerValues = headerLine.Split(separator);

            // Utwórz nowy obiekt Pilkarze z wartościami nagłówka
            var headerPilkarz = new Pilkarze
            {
                i = headerValues[0],
                n = headerValues[1],
                w = headerValues[2],
                r = headerValues[3],
                rr = headerValues[4]
            };

            // Dodaj obiekt nagłówka do listy
            data.Add(headerPilkarz);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(separator);

                var pilkarz = new Pilkarze
                {
                    imie = values[0],
                    nazwisko = values[1],
                    wiek = int.Parse(values[2]),
                    Rok_dolaczenia_do_klubu = int.Parse(values[3]),
                    Rok_zakonczenia_pracy_w_klubie = int.Parse(values[4])
                };

                data.Add(pilkarz);
            }
        }

        return data;
    }

    [Route("Pilkarze/")]
    public IActionResult Pilkarze()
    {
        SetViewDataFromSession();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Piłkarze.csv");
        List<Pilkarze> data = new List<Pilkarze>();

        if (System.IO.File.Exists(path))
        {
            data = ReadCsvFile(path, ';');
        }
        return View(data);
    }
}