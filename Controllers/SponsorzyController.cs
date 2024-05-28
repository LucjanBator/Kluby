using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kluby.Models;

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

        public void WriteCsvFile(string filePath, List<Sponsorzy> data, char separator)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("id_sponsora;Nazwa");
                foreach (var sponsor in data)
                {
                    writer.WriteLine($"{sponsor.id_sponsora}{separator}{sponsor.Nazwa}");
                }
            }
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

        [HttpPost]
        [Route("Sponsorzy/Add")]
        public IActionResult AddSponsor(Sponsorzy sponsor)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Sponsorzy.csv");
            List<Sponsorzy> data = new List<Sponsorzy>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            int newId = data.Any() ? data.Max(p => p.id_sponsora) + 1 : 1;
            sponsor.id_sponsora = newId;

            data.Add(sponsor);
            WriteCsvFile(path, data, ';');

            return RedirectToAction("Sponsorzy");
        }

        [HttpPost]
        [Route("Sponsorzy/Delete")]
        public IActionResult DeleteSponsor(int id)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Sponsorzy.csv");
            List<Sponsorzy> data = new List<Sponsorzy>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            var sponsorToRemove = data.FirstOrDefault(p => p.id_sponsora == id);
            if (sponsorToRemove != null)
            {
                data.Remove(sponsorToRemove);
                WriteCsvFile(path, data, ';');
            }

            return RedirectToAction("Sponsorzy");
        }
    }
}
