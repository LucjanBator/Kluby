
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
                id = headerValues[0],
                n = headerValues[1],
                m = headerValues[2],
                r = headerValues[3]
            };

            // Dodaj obiekt nagłówka do listy
            data.Add(headerPuchary);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(separator);

                var Puchary = new Puchary
                {
                    id_pucharu = int.Parse(values[0]),
                    Nazwa_Turnieju = values[1],
                    Miejsce = values[2],
                    Rok_zdobycia = int.Parse(values[3])
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