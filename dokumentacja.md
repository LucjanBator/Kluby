# Projekt Real Madryt

Projekt Real Madryt to aplikacja internetowa zbudowana przy użyciu ASP.NET Core, umożliwiająca zarządzanie danymi dotyczącymi klubu piłkarskiego Real Madryt. Aplikacja pozwala na zarządzanie tabelami w bazie danych, w tym piłkarzami, trenerami, sponsorami i pucharami, oraz zapewnia funkcje sortowania, dodawania i usuwania rekordów. Dodatkowo, projekt zawiera system logowania z hashowaniem oraz opcje akceptowania i dodawania użytkowników przez administratora.

## Tabele w bazie danych

### Piłkarze

Tabela `Piłkarze` zawiera informacje o piłkarzach Realu Madryt. Każdy rekord w tabeli reprezentuje jednego piłkarza.

| Kolumna                       | Typ    | Opis                                      |
|-------------------------------|--------|-------------------------------------------|
| `id_pilkarza`                 | int    | Identyfikator piłkarza                    |
| `imie`                        | string | Imię piłkarza                             |
| `nazwisko`                    | string | Nazwisko piłkarza                         |
| `wiek`                        | int    | Wiek piłkarza                             |
| `Rok_dolaczenia_do_klubu`     | int    | Rok dołączenia do klubu                   |
| `Rok_zakonczenia_pracy_w_klubie` | int | Rok zakończenia pracy w klubie (jeśli dotyczy) |

### Trenerzy

Tabela `Trenerzy` zawiera informacje o trenerach Realu Madryt. Każdy rekord w tabeli reprezentuje jednego trenera.

| Kolumna                       | Typ    | Opis                                      |
|-------------------------------|--------|-------------------------------------------|
| `id_trenera`                  | int    | Identyfikator trenera                     |
| `imie`                        | string | Imię trenera                              |
| `nazwisko`                    | string | Nazwisko trenera                          |
| `wiek`                        | int    | Wiek trenera                              |
| `Rok_dolaczenia_do_klubu`     | int    | Rok dołączenia do klubu                   |
| `Rok_zakonczenia_pracy_w_klubie` | int | Rok zakończenia pracy w klubie (jeśli dotyczy) |

### Sponsorzy

Tabela `Sponsorzy` zawiera informacje o sponsorach Realu Madryt. Każdy rekord w tabeli reprezentuje jednego sponsora.

| Kolumna                       | Typ    | Opis                                      |
|-------------------------------|--------|-------------------------------------------|
| `id_sponsora`                 | int    | Identyfikator sponsora                    |
| `Nazwa`                       | string | Nazwa sponsora                            |

### Puchary

Tabela `Puchary` zawiera informacje o pucharach zdobytych przez Real Madryt. Każdy rekord w tabeli reprezentuje jeden puchar.

| Kolumna                       | Typ    | Opis                                      |
|-------------------------------|--------|-------------------------------------------|
| `id_pucharu`                  | int    | Identyfikator pucharu                     |
| `Nazwa`                       | string | Nazwa turnieju                            |
| `Miejsce`                     | int    | Miejsce zdobyte w turnieju                |
| `Rok_zdobycia`                | int    | Rok zdobycia pucharu                      |

## Funkcje

### Sortowanie

Aplikacja umożliwia sortowanie rekordów w tabelach według różnych kryteriów, takich jak identyfikator, imię, nazwisko, wiek, rok dołączenia do klubu, rok zakończenia pracy w klubie, nazwa sponsora, nazwa turnieju, miejsce zdobyte w turnieju oraz rok zdobycia pucharu.

### Dodawanie i usuwanie rekordów

Aplikacja pozwala na dodawanie i usuwanie rekordów w tabelach. Formularze do dodawania nowych piłkarzy, trenerów, sponsorów i pucharów są dostępne dla zalogowanych użytkowników. Użytkownicy mogą także usuwać istniejące rekordy.

### System logowania

Aplikacja zawiera system logowania, który wykorzystuje hashowanie haseł dla zapewnienia bezpieczeństwa. Administratorzy mają możliwość akceptowania i dodawania nowych użytkowników, a także przydzielania im uprawnień administracyjnych.

#### Hashowanies
W systemie logowania dane są odpowiednio zabezpieczone przed atakami z róznych stron. Hasło jest bronione przed atakami dzięki użyciu algorytmu hashowania SHA-256.
