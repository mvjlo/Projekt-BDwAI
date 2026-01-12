# VST Vault - System Zarządzania Wtyczkami VST

System zarządzania wtyczkami VST stworzony w ASP.NET Core MVC z Entity Framework Core.

## Funkcjonalności

### Encje i Relacje
System zawiera 4 encje połączone relacjami:

1. **Manufacturer (Producent)** - Producenci wtyczek VST
2. **Category (Kategoria)** - Kategorie wtyczek (Syntezator, Kompresor, Reverb, EQ)
3. **Plugin (Wtyczka)** - Główna encja z informacjami o wtyczkach
   - Relacja Many-to-One z Manufacturer
   - Relacja Many-to-One z Category
4. **License (Licencja)** - Licencje użytkowników
   - Relacja Many-to-One z Plugin
   - Relacja Many-to-One z User (ASP.NET Identity)

### Formularze z Walidacją

1. **Formularz dodawania wtyczki**
   - Nazwa (wymagana)
   - Wybór producenta (lista rozwijana)
   - Wybór kategorii (lista rozwijana)
   - Cena (nie może być ujemna)
   - Wymagania systemowe
   - Opis

2. **Formularz rejestracji licencji**
   - Wybór wtyczki
   - Klucz seryjny (format: XXXX-XXXX-XXXX-XXXX, regex validation)
   - Data zakupu (nie może być z przyszłości)
   - Data wygaśnięcia (opcjonalna)

3. **Formularz edycji producenta/kategorii**
   - Nazwa (wymagana)
   - Strona WWW (walidacja URL)
   - Email wsparcia (walidacja email)

### Autoryzacja i Role

#### Administrator
- Zarządzanie słownikami (Producenci, Kategorie)
- Pełny dostęp CRUD do bazy wtyczek
- Dodawanie/edycja/usuwanie wtyczek
- Widzi wszystkie operacje

#### Zalogowany Użytkownik
- Przeglądanie bazy wtyczek
- Wyszukiwanie i filtrowanie wtyczek
- Dodawanie własnych licencji
- Zarządzanie swoimi licencjami

#### Niezalogowany Użytkownik
- Przeglądanie bazy wtyczek (tylko odczyt)
- Wyszukiwanie i filtrowanie

### API
Dostępne endpointy REST API:

- `GET /api/plugins` - Lista wszystkich wtyczek
- `GET /api/plugins/{id}` - Szczegóły wtyczki
- `GET /api/plugins/search?query=...&categoryId=...&manufacturerId=...` - Wyszukiwanie wtyczek

## Technologie

- **ASP.NET Core MVC** (.NET 10)
- **Entity Framework Core** (SQLite)
- **ASP.NET Core Identity** (Autoryzacja i uwierzytelnianie)
- **Bootstrap 5** (UI)

## Instalacja i Uruchomienie

### Wymagania
- .NET 10 SDK
- SQLite (wbudowane w EF Core)

### Konfiguracja

1. Sklonuj repozytorium:
```bash
git clone https://github.com/mvjlo/Projekt-BDwAI.git
cd Projekt-BDwAI/WebApplication1
```

2. Connection string jest już skonfigurowany w `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=VSTVault.db"
  }
}
```

3. Utwórz bazę danych i zastosuj migracje:
```bash
dotnet ef database update
```

4. Uruchom aplikację:
```bash
dotnet run
```

5. Aplikacja będzie dostępna pod adresem: `https://localhost:5227`

### Domyślne Konto Administratora
- **Email:** admin@vstvault.com
- **Hasło:** Admin123!

### Baza Danych
Projekt używa **SQLite** - plik bazy danych `VSTVault.db` jest tworzony automatycznie w folderze projektu. Nie wymaga instalacji osobnego serwera bazy danych!

## Struktura Projektu

```
WebApplication1/
├── Controllers/
│   ├── Api/
│   │   └── PluginsController.cs    # REST API
│   ├── CategoriesController.cs     # Zarządzanie kategoriami
│   ├── HomeController.cs           # Strona główna
│   ├── LicensesController.cs       # Zarządzanie licencjami
│   ├── ManufacturersController.cs  # Zarządzanie producentami
│   └── PluginsController.cs        # Zarządzanie wtyczkami
├── Data/
│   ├── ApplicationDbContext.cs     # Kontekst Entity Framework
│   └── SeedData.cs                 # Dane początkowe
├── Models/
│   ├── ViewModels/
│   │   └── LicenseCreateViewModel.cs
│   ├── Category.cs
│   ├── License.cs
│   ├── Manufacturer.cs
│   └── Plugin.cs
├── Views/
│   ├── Categories/                 # Widoki kategorii
│   ├── Home/                       # Strona główna
│   ├── Licenses/                   # Widoki licencji
│   ├── Manufacturers/              # Widoki producentów
│   ├── Plugins/                    # Widoki wtyczek
│   └── Shared/                     # Współdzielone widoki
├── wwwroot/                        # Pliki statyczne
└── VSTVault.db                     # Baza danych SQLite (tworzona automatycznie)
```

## Funkcjonalności Szczegółowe

### Wyszukiwanie i Filtrowanie
- Wyszukiwanie po nazwie wtyczki
- Filtrowanie po producencie
- Filtrowanie po kategorii
- Kombinacje filtrów

### Zarządzanie Licencjami
- Dodawanie kluczy seryjnych
- Śledzenie dat zakupu
- Obsługa dat wygaśnięcia
- Status licencji (aktywna/wygasła/wygasa wkrótce)
- Kopiowanie klucza do schowka

### Panel Administracyjny
- Zarządzanie producentami
- Zarządzanie kategoriami
- CRUD wtyczek
- Pełna kontrola nad danymi

## Dane Początkowe

System automatycznie tworzy:
- Role: Administrator, User
- Przykładowych producentów: FabFilter, Xfer Records, Native Instruments
- Kategorie: Syntezator, Kompresor, Reverb, EQ
- Konto administratora

## Bezpieczeństwo

- Walidacja po stronie serwera i klienta
- Ochrona przed CSRF
- Autoryzacja oparta na rolach
- Bezpieczne przechowywanie haseł (ASP.NET Identity)
- Walidacja danych wejściowych
