# Medical Clinic Management System

Projekt to konsolowa aplikacja do zarządzania przychodnią lekarską (Medical Clinic). Umożliwia kompleksową obsługę pacjentów, wizyt, magazynu oraz kadr. Cała logika biznesowa opiera się na zaawansowanych mechanizmach bazy danych Oracle (procedury składowane, wyzwalacze, widoki oraz pakiety PL/SQL), podczas gdy C# pełni rolę solidnego interfejsu i warstwy dostępowej.

## Wykorzystane Technologie i Biblioteki

Aplikacja została zbudowana w oparciu o architekturę **C# Console Application (.NET 10.0)**.

**Backend (NuGet) i Baza Danych:**
*   **Microsoft.EntityFrameworkCore (v10.0.8)** - Główny system ORM do komunikacji z bazą.
*   **Oracle.EntityFrameworkCore (v10.23.26200)** - Oficjalny dostawca bazy danych Oracle dla Entity Framework.
*   **Microsoft.EntityFrameworkCore.Tools & Design (v10.0.8)** - Narzędzia wspierające migracje z poziomu konsoli.
*   **Oracle Database 23c Free (Docker)** - Silnik bazy danych wykorzystywany do przechowywania i przetwarzania logiki PL/SQL (Trigger, View, Packages).

---

## Wymagania Wstępne

Aby uruchomić projekt na swoim środowisku lokalnym, musisz posiadać:

1.  **.NET 10.0 SDK** (lub nowszy)
2.  **Docker Desktop** (do uruchomienia kontenera z bazą Oracle)
3.  Dowolne narzędzie do obsługi Oracle (np. **DBeaver**, **DataGrip** lub **Oracle SQL Developer**)
4.  Edytor kodu (zalecane: **Visual Studio 2022** lub **JetBrains Rider**)

---

## Instrukcja Instalacji i Konfiguracji

### 1. Klonowanie repozytorium

Pobierz projekt na swój dysk:

```bash
git clone <adres-twojego-repozytorium>
cd medical-clinic/Projekt_SBD
```

### 2. Konfiguracja bazy danych (Docker - Oracle Free)

Aby uniknąć trudnej instalacji Oracle bezpośrednio na systemie Windows, wykorzystamy darmowy kontener w Dockerze. Otwórz wiersz poleceń (CMD lub PowerShell) i uruchom komendę:

```bash
docker run -d --name oracle-db -p 1521:1521 -e ORACLE_PASSWORD=Admin123 gvenzl/oracle-free:latest
```

*Po wykonaniu tej komendy poczekaj kilka minut, aż silnik Oracle wewnątrz Dockera zostanie w pełni uruchomiony.*

### 3. Utworzenie Użytkownika i Schematu w Oracle

Zanim wykonasz migracje, musisz stworzyć dedykowanego użytkownika dla przychodni. 
Połącz się z utworzoną bazą danych poprzez DataGrip / DBeaver na poświadczenia administratorskie:
*   **Host:** `localhost`
*   **Port:** `1521`
*   **SID/Service Name:** `FREEPDB1` (lub `FREE`)
*   **User:** `SYS`
*   **Password:** `Admin123`
*   **Rola:** `SYSDBA`

Następnie otwórz i wykonaj plik `DatabaseScripts/00_Administrator.sql`. Skrypt ten utworzy użytkownika `Administrator` i nada mu odpowiednie uprawnienia.

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

Po utworzeniu tabel musisz wgrać logikę biznesową przychodni (procedury rejestracji, audyty, logi).
Zaloguj się do bazy używając nowo utworzonego konta:
*   **User:** `Administrator`
*   **Password:** `Admin123`

I wykonaj po kolei zawartość wszystkich plików od `01` do `07` znajdujących się w folderze `DatabaseScripts/`.

### 6. Uruchomienie Aplikacji

Wszystko jest już gotowe! Otwórz terminal w folderze projektu i wpisz:

```bash
dotnet run
```

Aplikacja uruchomi się, a plik `DbInitializer.cs` automatycznie wypełni bazę danymi początkowymi (dodając m.in. przykładowe pokoje, personel oraz pacjentów). Gotowe!
