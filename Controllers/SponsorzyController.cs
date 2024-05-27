using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Kluby.Controllers
{
    public class SponsorzyController : Controller
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

        public List<Sponsorzy> ReadCsvFile(string filePath, char separator)
        {
            List<Sponsorzy> data = new List<Sponsorzy>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();

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
        public IActionResult Sponsorzy(string sortOrder)
        {
            SetViewDataFromSession();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Sponsorzy.csv");
            List<Sponsorzy> data = new List<Sponsorzy>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            // Sortowanie danych
            data = sortOrder switch
            {
                "id_asc" => data.OrderBy(p => p.id_sponsora).ToList(),
                "id_desc" => data.OrderByDescending(p => p.id_sponsora).ToList(),
                "name_asc" => data.OrderBy(p => p.Nazwa).ToList(),
                "name_desc" => data.OrderByDescending(p => p.Nazwa).ToList(),
                _ => data
            };

            return View(data);
        }
    }
}
