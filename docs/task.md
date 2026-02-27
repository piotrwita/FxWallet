# Zadanie: Aplikacja FxWallet

## Wymagania funkcjonalne

### 1. Pobieranie kursów walut
- Aplikacja **cyklicznie** pobiera kursy walut z **tabeli B** udostępnionej przez NBP.
- Pobrane kursy są **zapisywane w bazie danych**.

### 2. Portfele
- Umożliwienie **tworzenia** portfeli.
- Możliwość **nazywania** portfeli.
- **Listowanie** portfeli wraz z ich **zawartością**.

### 3. Transakcje
Aplikacja pozwala wykonywać transakcje:

- **Zasilenie portfela** - dowolna kwota w dowolnej walucie dostępnej w tabeli B.
- **Wypłata z portfela** - dowolna kwota dostępna w portfelu.
- **Konwersja** - zamiana części zawartości portfela na inną walutę z wykorzystaniem **najnowszych dostępnych kursów**. Przeliczenie musi odbyć się **za pośrednictwem waluty PLN**.

---

## Wymagania techniczne

- Aplikacja w **.NET 8** lub wyższym.
- **Entity Framework** do komunikacji z bazą danych.
- **UI nie jest wymagane** i nie jest częścią zadania.
