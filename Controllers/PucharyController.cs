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

        private void WriteCsvFile(string filePath, List<Puchary> data, char separator)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("id_pucharu;Nazwa_Turnieju;Miejsce;Rok_zdobycia");

                foreach (var puchar in data)
                {
                    writer.WriteLine($"{puchar.id_pucharu}{separator}{puchar.Nazwa_Turnieju}{separator}{puchar.Miejsce}{separator}{puchar.Rok_zdobycia}");
                }
            }
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

        [HttpPost]
        public IActionResult AddPuchar(Puchary puchar)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Puchary.csv");

            // Append the new puchar to the CSV file
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"{puchar.id_pucharu};{puchar.Nazwa_Turnieju};{puchar.Miejsce};{puchar.Rok_zdobycia}");
            }

            // Redirect back to the Puchary list
            return RedirectToAction("Puchary");
        }

        [HttpPost]
        public IActionResult DeletePuchar(int id)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Puchary.csv");
            List<Puchary> data = new List<Puchary>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
                var pucharToRemove = data.FirstOrDefault(p => p.id_pucharu == id);
                if (pucharToRemove != null)
                {
                    data.Remove(pucharToRemove);
                    WriteCsvFile(path, data, ';');
                }
            }

            // Redirect back to the Puchary list
            return RedirectToAction("Puchary");
        }
    }
}
