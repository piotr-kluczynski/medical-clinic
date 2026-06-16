# Medical Clinic Management System

Projekt to konsolowa aplikacja do zarządzania przychodnią lekarską (Medical Clinic). Umożliwia kompleksową obsługę pacjentów, wizyt, magazynu oraz kadr. Cała logika biznesowa opiera się na zaawansowanych mechanizmach bazy danych Oracle (procedury składowane, wyzwalacze, widoki oraz pakiety PL/SQL), podczas gdy C# pełni rolę solidnego interfejsu i warstwy dostępowej.

## Wykorzystane Technologie i Biblioteki

Aplikacja została zbudowana w oparciu o architekturę **C# Console Application (.NET 10.0)**.

**Backend (NuGet) i Baza Danych:**
*   **Microsoft.EntityFrameworkCore (v10.0.8)** - Główny system ORM do komunikacji z bazą.
*   **Oracle.EntityFrameworkCore (v10.23.26200)** - Oficjalny dostawca bazy danych Oracle dla Entity Framework.
*   **Microsoft.EntityFrameworkCore.Tools & Design (v10.0.8)** - Narzędzia wspierające migracje z poziomu konsoli.
*   **BCrypt.Net-Next (v4.2.0)** - Zaawansowana biblioteka kryptograficzna używana do bezpiecznego hashowania oraz weryfikacji haseł użytkowników.
*   **Oracle Database 23c Free (Docker)** - Silnik bazy danych wykorzystywany do przechowywania i przetwarzania logiki PL/SQL (Trigger, View, Packages).

---

## Wymagania Wstępne

Aby uruchomić projekt na swoim środowisku lokalnym, musisz posiadać:

1.  **.NET 10.0 SDK** (lub nowszy)
2.  **Docker Desktop** (do uruchomienia kontenera z bazą Oracle)
3.  **Oracle SQL Developer** (do wygodnego zarządzania bazą)
4.  Edytor kodu (zalecane: **Visual Studio 2022** lub **JetBrains Rider**)

---

## Instrukcja Instalacji i Konfiguracji

### 1. Klonowanie repozytorium

Pobierz projekt na swój dysk:

```bash
git clone <https://github.com/piotr-kluczynski/medical-clinic>
cd medical-clinic/Projekt_SBD
```

### 2. Konfiguracja bazy danych (Docker - Oracle Free)

Aby uniknąć trudnej instalacji Oracle bezpośrednio na systemie Windows, wykorzystamy darmowy kontener w Dockerze. Otwórz wiersz poleceń (CMD lub PowerShell) i uruchom komendę:

```bash
docker run -d --name oracle-db -p 1521:1521 -e ORACLE_PASSWORD=Admin123 gvenzl/oracle-free:latest
```

*Po wykonaniu tej komendy poczekaj kilka minut, aż silnik Oracle wewnątrz Dockera zostanie w pełni uruchomiony.*

### 3. Utworzenie Użytkownika i Schematu w Oracle

Zanim wykonasz migracje w C#, musisz stworzyć dedykowanego użytkownika w bazie. 
Połącz się z utworzoną bazą danych poprzez **Oracle SQL Developer** na poświadczenia administratorskie:
*   **Host:** `localhost`
*   **Port:** `1521`
*   **SID/Service Name:** `FREEPDB1` (lub `FREE`)
*   **User:** `System`
*   **Password:** `Admin123`
*   **Rola:** Domyślna (lub `SYSDBA`)

Następnie otwórz i wykonaj plik `DatabaseScripts/00_Administrator.sql`. Skrypt ten utworzy użytkownika `Administrator` i nada mu odpowiednie uprawnienia, na których będzie pracować aplikacja.

> **Uwaga:** Pozostałe skrypty z folderu `DatabaseScripts` (od 01 do 07) uruchom dopiero po wykonaniu migracji EF Core w Kroku 4, ponieważ wymagają one istnienia tabel.

### 4. Wykonanie Migracji EF Core

Teraz możesz utworzyć wszystkie tabele w schemacie Administratora. 
Otwórz terminal w katalogu projektu (tam gdzie plik `Projekt_SBD.csproj`) i wykonaj:

**(Dla .NET CLI / Terminal):**
```bash
dotnet ef database update
```

**(Dla Package Manager Console w Visual Studio):**
```powershell
Update-Database
```

### 5. Kompilacja logiki PL/SQL (Pakiety, Triggery)

Po utworzeniu tabel przez Entity Framework, musisz wgrać logikę biznesową przychodni (procedury rejestracji, pakiety, wyzwalacze).
Połącz się ponownie w **Oracle SQL Developer**, ale tym razem używając nowo utworzonego konta przychodni:
*   **User:** `Administrator`
*   **Password:** `Admin123`

Teraz otwórz i wykonaj po kolei zawartość wszystkich pozostałych skryptów SQL z folderu `DatabaseScripts/` (czyli pliki `01`, `02`, `03`, `04`, `05`, `06`, `07`). To absolutnie kluczowe, ponieważ na nich opiera się logika aplikacji!

### 6. Uruchomienie Aplikacji

Wszystko jest już gotowe! Otwórz terminal w folderze projektu i wpisz:

```bash
dotnet run
```

Aplikacja uruchomi się, a plik `DbInitializer.cs` automatycznie wypełni bazę danymi początkowymi (dodając m.in. przykładowe pokoje, personel oraz pacjentów). Gotowe!
