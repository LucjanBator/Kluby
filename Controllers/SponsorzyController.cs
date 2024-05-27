
using Kluby.Models;
using Microsoft.AspNetCore.Mvc;
namespace Kluby.Controllers;

public class SponsorzyController : Controller{
    private void SetViewDataFromSession() {
        if (HttpContext.Session.GetString("username") == null) {
            ViewData["Username"] = "";
            ViewData["IsAdmin"] = "";

            return;
        }

        ViewData["Username"] = HttpContext.Session.GetString("username");
        ViewData["IsAdmin"] = HttpContext.Session.GetString("isadmin");
    }
    public List<Sponsorzy> ReadCsvFile(string filePath, char separator)
    {
        List<Sponsorzy> data = new List<Sponsorzy>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine();
            string[] headerValues = headerLine.Split(separator);

            // Utwórz nowy obiekt Pilkarze z wartościami nagłówka
            var headerSponsor = new Sponsorzy
            {
                id = headerValues[0],
                n = headerValues[1]
            };

            // Dodaj obiekt nagłówka do listy
            data.Add(headerSponsor);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(separator);

                var sponsor = new Sponsorzy
                {
                    id_sponsora = int.Parse(values[0]),
                    Nazwa = values[1]
                };

                data.Add(sponsor);
            }
        }
        return data;
    }
    
    [Route("Sponsorzy/")]
    public IActionResult Sponsorzy()
    {
        SetViewDataFromSession();   
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Sponsorzy.csv");
        List<Sponsorzy> data = new List<Sponsorzy>();

        if (System.IO.File.Exists(path))
        {
            data = ReadCsvFile(path, ';');
        }

        return View(data);
    }
}