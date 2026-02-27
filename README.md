# FxWallet

Aplikacja do portfeli wielowalutowych z kursami NBP.

Pełna treść zadania: [docs/task.md](docs/task.md)

## Co robi aplikacja

- **Kursy walut** – cykliczne pobieranie kursów z NBP i zapis w bazie danych.
- **Portfele** – tworzenie, nazywanie, listowanie portfeli wraz z zawartością.
- **Transakcje** – zasilenie (dowolna kwota w walucie), wypłata, konwersja (przez PLN, na aktualnych kursach).

## Technologie

.NET 10, Entity Framework Core, API.

## Uruchomienie

Wymagany .NET SDK. Z katalogu głównego repozytorium (tam gdzie znajduje się plik solution):

```bash
dotnet run --project src/FxWallet.Api
```

Connection string do bazy ustaw w `appsettings.json` lub `appsettings.Development.json`.
