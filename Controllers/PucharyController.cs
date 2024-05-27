using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Kluby.Controllers
{
    public class PucharyController : Controller
    {
        private void SetViewDataFromSession()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
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

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(separator);

                    var puchar = new Puchary
                    {
                        id_pucharu = int.Parse(values[0]),
                        Nazwa_Turnieju = values[1],
                        Miejsce = values[2],
                        Rok_zdobycia = int.Parse(values[3])
                    };

                    data.Add(puchar);
                }
            }

            return data;
        }

        [Route("Puchary/")]
        public IActionResult Puchary(string sortOrder)
        {
            SetViewDataFromSession();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Puchary.csv");
            List<Puchary> data = new List<Puchary>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            // Sortowanie danych
            data = sortOrder switch
            {
                "id_asc" => data.OrderBy(p => p.id_pucharu).ToList(),
                "id_desc" => data.OrderByDescending(p => p.id_pucharu).ToList(),
                "name_asc" => data.OrderBy(p => p.Nazwa_Turnieju).ToList(),
                "name_desc" => data.OrderByDescending(p => p.Nazwa_Turnieju).ToList(),
                "place_asc" => data.OrderBy(p => p.Miejsce).ToList(),
                "place_desc" => data.OrderByDescending(p => p.Miejsce).ToList(),
                "year_asc" => data.OrderBy(p => p.Rok_zdobycia).ToList(),
                "year_desc" => data.OrderByDescending(p => p.Rok_zdobycia).ToList(),
                _ => data
            };

            return View(data);
        }
    }
}
