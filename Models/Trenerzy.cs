namespace Kluby.Models;
public class Trenerzy
{
    public int id_trenera { get; set; }
    public string imie { get; set; }
    public string nazwisko { get; set; }
    public int wiek { get; set; }
    public int Rok_dolaczenia_do_klubu { get; set; }

    public int Rok_zakonczenia_pracy_w_klubie { get; set; }
}