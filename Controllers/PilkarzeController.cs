using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Kluby.Controllers
{
    public class PilkarzeController : Controller
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

        public List<Pilkarze> ReadCsvFile(string filePath, char separator)
        {
            List<Pilkarze> data = new List<Pilkarze>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(separator);

                    var pilkarz = new Pilkarze
                    {
                        id_pilkarza = int.Parse(values[0]),
                        imie = values[1],
                        nazwisko = values[2],
                        wiek = int.Parse(values[3]),
                        Rok_dolaczenia_do_klubu = int.Parse(values[4]),
                        Rok_zakonczenia_pracy_w_klubie = int.Parse(values[5])
                    };

                    data.Add(pilkarz);
                }
            }

            return data;
        }

        [Route("Pilkarze/")]
        public IActionResult Pilkarze(string sortOrder)
        {
            SetViewDataFromSession();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Piłkarze.csv");
            List<Pilkarze> data = new List<Pilkarze>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            // Sortowanie danych
            data = sortOrder switch
            {
                "id_asc" => data.OrderBy(p => p.id_pilkarza).ToList(),
                "id_desc" => data.OrderByDescending(p => p.id_pilkarza).ToList(),
                "name_asc" => data.OrderBy(p => p.imie).ToList(),
                "name_desc" => data.OrderByDescending(p => p.imie).ToList(),
                "surname_asc" => data.OrderBy(p => p.nazwisko).ToList(),
                "surname_desc" => data.OrderByDescending(p => p.nazwisko).ToList(),
                "age_asc" => data.OrderBy(p => p.wiek).ToList(),
                "age_desc" => data.OrderByDescending(p => p.wiek).ToList(),
                "year_join_asc" => data.OrderBy(p => p.Rok_dolaczenia_do_klubu).ToList(),
                "year_join_desc" => data.OrderByDescending(p => p.Rok_dolaczenia_do_klubu).ToList(),
                "year_end_asc" => data.OrderBy(p => p.Rok_zakonczenia_pracy_w_klubie).ToList(),
                "year_end_desc" => data.OrderByDescending(p => p.Rok_zakonczenia_pracy_w_klubie).ToList(),
                _ => data
            };

            return View(data);
        }
    }
}
