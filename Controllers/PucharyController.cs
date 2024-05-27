
using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
namespace Kluby.Controllers;
public class PucharyController : Controller {
    private void SetViewDataFromSession() {
        if (HttpContext.Session.GetString("username") == null) {
            ViewData["Username"] = "";
            ViewData["IsAdmin"] = "";

            return;
        }

        ViewData["Username"] = HttpContext.Session.GetString("username");
        ViewData["IsAdmin"] = HttpContext.Session.GetString("isadmin");
    }
    public List<Puchary> ReadCsvFile(string filePath, char separator)
    {
        List<Puchary> data = new List<Puchary>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine();
            string[] headerValues = headerLine.Split(separator);

            // Utwórz nowy obiekt Pilkarze z wartościami nagłówka
            var headerPuchary = new Puchary
            {
                n = headerValues[0],
                m = headerValues[1],
                r = headerValues[2]
            };

            // Dodaj obiekt nagłówka do listy
            data.Add(headerPuchary);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(separator);

                var Puchary = new Puchary
                {
                    Nazwa_Turnieju = values[0],
                    Miejsce = values[1],
                    Rok_zdobycia = int.Parse(values[2])
                };

                data.Add(Puchary);
            }
        }

        return data;
    }

    [Route("Puchary/")]
    public IActionResult Puchary()
    {
        SetViewDataFromSession();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Puchary.csv");
        List<Puchary> data = new List<Puchary>();

        if (System.IO.File.Exists(path))
        {
            data = ReadCsvFile(path, ';');
        }

        return View(data);
    }
}