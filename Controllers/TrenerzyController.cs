using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
namespace Kluby.Controllers;

public class TrenerzyController : Controller {
    private void SetViewDataFromSession() {
        if (HttpContext.Session.GetString("username") == null) {
            ViewData["Username"] = "";
            ViewData["IsAdmin"] = "";

            return;
        }

        ViewData["Username"] = HttpContext.Session.GetString("username");
        ViewData["IsAdmin"] = HttpContext.Session.GetString("isadmin");
    }    
    public List<Trenerzy> ReadCsvFile(string filePath, char separator)
    {
        List<Trenerzy> data = new List<Trenerzy>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine();
            string[] headerValues = headerLine.Split(separator);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(separator);

                var trener = new Trenerzy
                {
                    id_trenera = int.Parse(values[0]),
                    imie = values[1],
                    nazwisko = values[2],
                    wiek = int.Parse(values[3]),
                    Rok_dolaczenia_do_klubu = int.Parse(values[4]),
                    Rok_zakonczenia_pracy_w_klubie = int.Parse(values[5])
                };

                data.Add(trener);
            }
        }

        return data;
    }

    [Route("Trenerzy/")]
    public IActionResult Trenerzy(string sortOrder) {
        SetViewDataFromSession();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Trenerzy.csv");
        List<Trenerzy> data = new List<Trenerzy>();

        if (System.IO.File.Exists(path)) {
            data = ReadCsvFile(path, ';');
        }

        // Sortowanie danych (bez nagłówka)
        if (sortOrder == "age_asc") {
            data = data.OrderBy(t => t.wiek).ToList();
        } else if (sortOrder == "age_desc") {
            data = data.OrderByDescending(t => t.wiek).ToList();
        }

        return View(data);
    }
}