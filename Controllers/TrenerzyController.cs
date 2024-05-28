using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kluby.Models;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;

namespace Kluby.Controllers
{
    public class TrenerzyController : Controller
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

        public List<Trenerzy> ReadCsvFile(string filePath, char separator)
        {
            List<Trenerzy> data = new List<Trenerzy>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();

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

        public void WriteCsvFile(string filePath, List<Trenerzy> data, char separator)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("id_trenera;imie;nazwisko;wiek;Rok_dolaczenia_do_klubu;Rok_zakonczenia_pracy_w_klubie");
                foreach (var trener in data)
                {
                    writer.WriteLine($"{trener.id_trenera}{separator}{trener.imie}{separator}{trener.nazwisko}{separator}{trener.wiek}{separator}{trener.Rok_dolaczenia_do_klubu}{separator}{trener.Rok_zakonczenia_pracy_w_klubie}");
                }
            }
        }

        [Route("Trenerzy/")]
        public IActionResult Trenerzy(string sortOrder)
        {
            SetViewDataFromSession();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Trenerzy.csv");
            List<Trenerzy> data = new List<Trenerzy>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            data = sortOrder switch
            {
                "id_asc" => data.OrderBy(t => t.id_trenera).ToList(),
                "id_desc" => data.OrderByDescending(t => t.id_trenera).ToList(),
                "name_asc" => data.OrderBy(t => t.imie).ToList(),
                "name_desc" => data.OrderByDescending(t => t.imie).ToList(),
                "surname_asc" => data.OrderBy(t => t.nazwisko).ToList(),
                "surname_desc" => data.OrderByDescending(t => t.nazwisko).ToList(),
                "age_asc" => data.OrderBy(t => t.wiek).ToList(),
                "age_desc" => data.OrderByDescending(t => t.wiek).ToList(),
                "year_join_asc" => data.OrderBy(t => t.Rok_dolaczenia_do_klubu).ToList(),
                "year_join_desc" => data.OrderByDescending(t => t.Rok_dolaczenia_do_klubu).ToList(),
                "year_end_asc" => data.OrderBy(t => t.Rok_zakonczenia_pracy_w_klubie).ToList(),
                "year_end_desc" => data.OrderByDescending(t => t.Rok_zakonczenia_pracy_w_klubie).ToList(),
                _ => data
            };

            return View(data);
        }

        [HttpPost]
        [Route("Trenerzy/Add")]
        public IActionResult AddTrener(Trenerzy trener)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Trenerzy.csv");
            List<Trenerzy> data = new List<Trenerzy>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            int newId = data.Any() ? data.Max(t => t.id_trenera) + 1 : 1;
            trener.id_trenera = newId;

            data.Add(trener);
            WriteCsvFile(path, data, ';');

            return RedirectToAction("Trenerzy");
        }

        [HttpPost]
        [Route("Trenerzy/Delete")]
        public IActionResult DeleteTrener(int id)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Excel", "Trenerzy.csv");
            List<Trenerzy> data = new List<Trenerzy>();

            if (System.IO.File.Exists(path))
            {
                data = ReadCsvFile(path, ';');
            }

            var trenerToRemove = data.FirstOrDefault(t => t.id_trenera == id);
            if (trenerToRemove != null)
            {
                data.Remove(trenerToRemove);
                WriteCsvFile(path, data, ';');
            }

            return RedirectToAction("Trenerzy");
        }
    }
}
