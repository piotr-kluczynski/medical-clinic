using System;
using System.Collections.Generic;
using System.Linq;
using Projekt_SBD.ConsoleUserInterface;
using Projekt_SBD.ConsoleUserInterface.DisplayScreen;
using Projekt_SBD.ConsoleUserInterface.FormScreen;
using Projekt_SBD.ConsoleUserInterface.ListScreen;
using Projekt_SBD.ConsoleUserInterface.MenuScreen;
using Projekt_SBD.Models;
using Projekt_SBD.Utilities;
using Projekt_SBD.Services;
using Microsoft.EntityFrameworkCore;

namespace Projekt_SBD.Data
{
    public enum ScreensEnum
    {
        Exit,
        MainMenu,
        LogInForm,
        RegisterForm,
        
        PatientMenu,
        AdminMenu,
        ReceptionMenu,
        DoctorMenu,

        PlSqlTestsMenu,
        ScheduleVisitForm,
        CancelVisitForm,
        ConsumeSupplyForm,
        AddSupplyDeliveryForm,
        CreateNewSupplyForm,
        MonthlyCostsForm,

        PatientList_Reception,
        PatientList_Doctor,
        PatientDetails,
        StaffList,
        StaffDetails,
        EquipmentList,
        EquipmentDetails,
        SupplyList_Reception,
        SupplyList_Doctor,
        SupplyList_Admin,
        SupplyDetails,
        DepartmentList,
        RoomList_Admin,
        VisitList_Reception,
        VisitList_Patient,
        DoctorList_Patient,
        DoctorList_Reception,
        
        DoctorAvailabilityView_Reception,
        DoctorAvailabilityView_Doctor,
        DoctorAvailabilityView_Patient,
        LowStockSuppliesView,
        MyMedicalHistoryView,
        
        ScheduleVisitForm_Reception,
        ScheduleVisitForm_Patient,
        DoctorPatientHistoryForm,
        CompleteVisitForm,
        AddWorkerForm,
        AddEquipmentForm,
        SuppliesUsageReportForm,
        CalculateAssetsValueScreen
    }

    public static class ConsoleMenuInitializer
    {
        public static List<IConsoleScreen> Initialize()
        {
            List<IConsoleScreen> screens = new List<IConsoleScreen>();

            
            Menu mainMenu = new Menu(
                ScreensEnum.MainMenu,
                "Ekran Powitalny",
                "Witaj w systemie zarządzania przychodnią Medical Clinic!",
                ScreensEnum.Exit,
                [
                    new MenuOption(1, "Zaloguj się", ScreensEnum.LogInForm),
                    new MenuOption(2, "Zarejestruj się (Tylko dla Pacjentów)", ScreensEnum.RegisterForm),
                    new MenuOption(0, "Wyjście z aplikacji", ScreensEnum.Exit)
                ]
            );
            screens.Add(mainMenu);

            Form logInForm = new Form(
                ScreensEnum.LogInForm,
                "Panel Logowania",
                "Podaj swoje dane uwierzytelniające:",
                [
                    new FormField(0, "Email", "",  _ => true),
                    new FormField(1, "Hasło", "", _ => true)
                ],
                ScreensEnum.MainMenu,
                ScreensEnum.MainMenu,
                (FormField[] formFields) =>
                {
                    string email = formFields[0].Value;
                    string password = formFields[1].Value;

                    using var context = new PrzychodniaContext();
                    
                    var worker = context.Workers.FirstOrDefault(w => w.Email == email && w.PasswordHash == password);
                    if (worker != null)
                    {
                        Session.CurrentWorker = worker;
                        Session.CurrentPatient = null;
                        return;
                    }

                    var patient = context.Patients.FirstOrDefault(p => p.Email == email && p.PasswordHash == password);
                    if (patient != null)
                    {
                        Session.CurrentPatient = patient;
                        Session.CurrentWorker = null;
                        return;
                    }

                    Console.WriteLine("Błędny email lub hasło. Spróbuj ponownie.");
                    Console.ReadKey();
                }
            );

            logInForm.LeaveAction = () => {
                if (Session.CurrentWorker != null)
                {
                    var pos = Session.CurrentWorker.Position.ToLower();
                    if (pos == "administrator" || pos == "owner" || pos == "właściciel")
                        logInForm.SucceedScreenId = ScreensEnum.AdminMenu;
                    else if (pos == "reception" || pos == "recepcja")
                        logInForm.SucceedScreenId = ScreensEnum.ReceptionMenu;
                    else
                        logInForm.SucceedScreenId = ScreensEnum.DoctorMenu;
                }
                else if (Session.CurrentPatient != null)
                {
                    logInForm.SucceedScreenId = ScreensEnum.PatientMenu;
                }
                else
                {
                    logInForm.SucceedScreenId = ScreensEnum.MainMenu;
                }
            };
            screens.Add(logInForm);

            Form registerForm = new Form(
                ScreensEnum.RegisterForm,
                "Rejestracja Pacjenta",
                "Utwórz nowe konto pacjenta w systemie.",
                [
                    new FormField(0, "Imię", "Wymagane", RegexUtilities.IsValidName),
                    new FormField(1, "Nazwisko", "Wymagane", RegexUtilities.IsValidName),
                    new FormField(2, "Telefon", "Format międzynarodowy", RegexUtilities.IsValidPhoneNumber),
                    new FormField(3, "Email", "Wymagane", RegexUtilities.IsValidEmail),
                    new FormField(4, "Hasło", "Minimum 4 znaki", RegexUtilities.IsValidPassword)
                ],
                ScreensEnum.MainMenu,
                ScreensEnum.MainMenu,
                (FormField[] formFields) =>
                {
                    using var context = new PrzychodniaContext();
                    var newPatient = new Patient
                    {
                        FirstName = formFields[0].Value,
                        LastName = formFields[1].Value,
                        Phone = formFields[2].Value,
                        Email = formFields[3].Value,
                        PasswordHash = formFields[4].Value
                    };
                    context.Patients.Add(newPatient);
                    context.SaveChanges();
                    Console.WriteLine("Rejestracja zakończona sukcesem! Możesz się zalogować.");
                    Console.ReadKey();
                }
            );
            screens.Add(registerForm);


            Menu patientMenu = new Menu(
                ScreensEnum.PatientMenu, "Panel Pacjenta", "Witaj w swoim panelu!", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Moja historia medyczna", ScreensEnum.MyMedicalHistoryView),
                    new MenuOption(2, "Moje zaplanowane wizyty", ScreensEnum.VisitList_Patient),
                    new MenuOption(3, "Lista lekarzy (Wszyscy)", ScreensEnum.DoctorList_Patient),
                    new MenuOption(4, "Sprawdź zajęte terminy", ScreensEnum.DoctorAvailabilityView_Patient),
                    new MenuOption(5, "Umów wizytę", ScreensEnum.ScheduleVisitForm_Patient),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(patientMenu);

            Menu adminMenu = new Menu(
                ScreensEnum.AdminMenu, "Panel Administratora", "Zarządzanie personelem i raportami.", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Dodaj nowego pracownika", ScreensEnum.AddWorkerForm),
                    new MenuOption(2, "Dodaj nowy sprzęt medyczny", ScreensEnum.AddEquipmentForm),
                    new MenuOption(3, "Lista pracowników", ScreensEnum.StaffList),
                    new MenuOption(4, "Lista pokoi", ScreensEnum.RoomList_Admin),
                    new MenuOption(5, "Lista materiałów", ScreensEnum.SupplyList_Admin),
                    new MenuOption(6, "Raport kosztów operacyjnych", ScreensEnum.MonthlyCostsForm),
                    new MenuOption(7, "Raport zużycia materiałów", ScreensEnum.SuppliesUsageReportForm),
                    new MenuOption(8, "Wylicz łączną wartość majątku", ScreensEnum.CalculateAssetsValueScreen),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(adminMenu);

            Menu receptionMenu = new Menu(
                ScreensEnum.ReceptionMenu, "Panel Recepcji", "Obsługa pacjentów i harmonogramów.", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Lista pacjentów", ScreensEnum.PatientList_Reception),
                    new MenuOption(2, "Lista wizyt", ScreensEnum.VisitList_Reception),
                    new MenuOption(3, "Lista materiałów", ScreensEnum.SupplyList_Reception),
                    new MenuOption(4, "Lista lekarzy (Wszyscy)", ScreensEnum.DoctorList_Reception),
                    new MenuOption(5, "Grafik lekarzy (Zajęte terminy)", ScreensEnum.DoctorAvailabilityView_Reception),
                    new MenuOption(6, "Umów wizytę", ScreensEnum.ScheduleVisitForm_Reception),
                    new MenuOption(7, "Odwołaj wizytę", ScreensEnum.CancelVisitForm),
                    new MenuOption(8, "Sprawdź braki w magazynie", ScreensEnum.LowStockSuppliesView),
                    new MenuOption(9, "Przyjmij dostawę materiałów", ScreensEnum.AddSupplyDeliveryForm),
                    new MenuOption(10, "Wprowadź nowy materiał do bazy", ScreensEnum.CreateNewSupplyForm),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(receptionMenu);

            Menu doctorMenu = new Menu(
                ScreensEnum.DoctorMenu, "Panel Lekarza", "Dostęp do wizyt i pacjentów.", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Mój grafik (Lista Wizyt)", ScreensEnum.DoctorAvailabilityView_Doctor),
                    new MenuOption(2, "Lista pacjentów", ScreensEnum.PatientList_Doctor),
                    new MenuOption(3, "Lista materiałów", ScreensEnum.SupplyList_Doctor),
                    new MenuOption(4, "Historia Medyczna Pacjenta", ScreensEnum.DoctorPatientHistoryForm),
                    new MenuOption(5, "Zużyj materiał do zabiegu", ScreensEnum.ConsumeSupplyForm),
                    new MenuOption(6, "Zakończ wizytę (Dodaj Diagnozę)", ScreensEnum.CompleteVisitForm),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(doctorMenu);


            screens.Add(new Form(ScreensEnum.ScheduleVisitForm_Reception, "Umów Wizytę (Recepcja)", "Zapełnij dane do procedury ScheduleVisit",
                [
                    new FormField(0, "ID Pacjenta", "", _ => true),
                    new FormField(1, "ID Lekarza", "Sprawdź w Grafiku Lekarzy", _ => true),
                    new FormField(2, "Data wizyty (RRRR-MM-DD)", "", _ => true),
                    new FormField(3, "Godzina (GG:MM)", "Dostępne od 07:00 do 19:30", _ => true),
                    new FormField(4, "Cel wizyty", "", _ => true),
                    new FormField(5, "Koszt", "", _ => true)
                ],
                ScreensEnum.ReceptionMenu,
                ScreensEnum.ReceptionMenu,
                (formFields) => {
                    try {
                        using var context = new PrzychodniaContext();
                        var workerId = int.Parse(formFields[1].Value);
                        var doctor = context.Workers.FirstOrDefault(w => w.Id == workerId);
                        
                        if (doctor == null) {
                            Console.WriteLine("\nBłąd: Lekarz o podanym ID nie istnieje.");
                            Console.ReadKey();
                            return;
                        }
                        if (doctor.RoomId == null) {
                            Console.WriteLine("\nBłąd: Lekarz nie ma przypisanego gabinetu.");
                            Console.ReadKey();
                            return;
                        }

                        DateTime start;
                        try {
                            start = DateTime.ParseExact(formFields[2].Value + " " + formFields[3].Value, "yyyy-MM-dd HH:mm", null);
                        } catch {
                            Console.WriteLine("\nBłąd: Nieprawidłowy format daty lub godziny.");
                            Console.ReadKey();
                            return;
                        }

                        TimeSpan time = start.TimeOfDay;
                        if (time < new TimeSpan(7, 0, 0) || time > new TimeSpan(19, 30, 0)) {
                            Console.WriteLine("\nBłąd: Godzina wizyty musi mieścić się w przedziale od 07:00 do 19:30 (Przychodnia czynna do 20:00).");
                            Console.ReadKey();
                            return;
                        }

                        var srv = new ClinicService(context);
                        srv.ScheduleVisitAsync(
                            int.Parse(formFields[0].Value), workerId, doctor.RoomId.Value,
                            start, start.AddMinutes(30),
                            formFields[4].Value, int.Parse(formFields[5].Value)
                        ).Wait();
                        Console.WriteLine("Procedura wykonana pomyślnie. Zapisano w bazie.");
                    } catch (Exception ex) {
                        Console.WriteLine("Błąd Oracle: " + ex.Message);
                    }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.ScheduleVisitForm_Patient, "Umów Wizytę (Pacjent)", "Zapełnij dane do procedury ScheduleVisit",
                [
                    new FormField(0, "ID Lekarza", "Sprawdź w dostępności lekarzy (np. 1)", _ => true),
                    new FormField(1, "Data wizyty (RRRR-MM-DD)", "", _ => true),
                    new FormField(2, "Godzina (GG:MM)", "Dostępne od 07:00 do 19:30", _ => true),
                    new FormField(3, "Cel wizyty", "Opisz powód", _ => true)
                ],
                ScreensEnum.PatientMenu,
                ScreensEnum.PatientMenu,
                (formFields) => {
                    try {
                        using var context = new PrzychodniaContext();
                        var workerId = int.Parse(formFields[0].Value);
                        var doctor = context.Workers.FirstOrDefault(w => w.Id == workerId);
                        
                        if (doctor == null) {
                            Console.WriteLine("\nBłąd: Lekarz o podanym ID nie istnieje.");
                            Console.ReadKey();
                            return;
                        }
                        if (doctor.RoomId == null) {
                            Console.WriteLine("\nBłąd: Lekarz nie ma przypisanego gabinetu.");
                            Console.ReadKey();
                            return;
                        }

                        DateTime start;
                        try {
                            start = DateTime.ParseExact(formFields[1].Value + " " + formFields[2].Value, "yyyy-MM-dd HH:mm", null);
                        } catch {
                            Console.WriteLine("\nBłąd: Nieprawidłowy format daty lub godziny.");
                            Console.ReadKey();
                            return;
                        }

                        TimeSpan time = start.TimeOfDay;
                        if (time < new TimeSpan(7, 0, 0) || time > new TimeSpan(19, 30, 0)) {
                            Console.WriteLine("\nBłąd: Godzina wizyty musi mieścić się w przedziale od 07:00 do 19:30 (Przychodnia czynna do 20:00).");
                            Console.ReadKey();
                            return;
                        }

                        var srv = new ClinicService(context);
                        srv.ScheduleVisitAsync(
                            Session.CurrentPatient.Id, workerId, doctor.RoomId.Value,
                            start, start.AddMinutes(30),
                            formFields[3].Value, 100
                        ).Wait();
                        Console.WriteLine("Procedura wykonana pomyślnie. Udało się umówić wizytę.");
                    } catch (Exception ex) {
                        Console.WriteLine("Błąd Oracle: " + ex.Message);
                    }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.CompleteVisitForm, "Zakończ wizytę (Dodaj diagnozę)", "Dodaje diagnozę i podczepia pod wizytę.",
                [
                    new FormField(0, "ID Wizyty", "Wpisz ID wizyty z Twojego grafiku", _ => true),
                    new FormField(1, "Objawy", "np. Gorączka, ból gardła", _ => true),
                    new FormField(2, "Rozpoznanie", "np. Zapalenie gardła", _ => true)
                ],
                ScreensEnum.DoctorMenu, ScreensEnum.DoctorMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        var visitId = int.Parse(f[0].Value);
                        var visit = ctx.Visits.FirstOrDefault(v => v.Id == visitId);
                        if (visit == null) {
                            Console.WriteLine("\nBłąd: Nie znaleziono takiej wizyty.");
                            Console.ReadKey();
                            return;
                        }
                        if (visit.WorkerId != Session.CurrentWorker?.Id) {
                            Console.WriteLine("\nBłąd: Ta wizyta nie należy do Ciebie!");
                            Console.ReadKey();
                            return;
                        }
                        
                        var diagnosis = new Diagnosis {
                            DiagnosisTime = DateTime.Now,
                            Symptoms = f[1].Value,
                            Illness = f[2].Value,
                            PatientId = visit.PatientId,
                            WorkerId = visit.WorkerId
                        };
                        ctx.Diagnosis.Add(diagnosis);
                        ctx.SaveChanges();

                        visit.DiagnosisId = diagnosis.Id;
                        ctx.SaveChanges();

                        Console.WriteLine("Pomyślnie dodano diagnozę! Wizyta została zakończona.");
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.CancelVisitForm, "Odwołaj Wizytę", "Procedura CancelVisit",
                [ new FormField(0, "ID Wizyty", "", _ => true) ],
                ScreensEnum.ReceptionMenu, ScreensEnum.ReceptionMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        new ClinicService(ctx).CancelVisitAsync(int.Parse(f[0].Value)).Wait();
                        Console.WriteLine("Odwołano wizytę pomyślnie (Archiwum zadziałało).");
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.ConsumeSupplyForm, "Zużyj materiał", "Procedura ConsumeSupply",
                [ new FormField(0, "ID Materiału", "", _ => true), new FormField(1, "Ilość zużyta", "", _ => true) ],
                ScreensEnum.DoctorMenu, ScreensEnum.DoctorMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        new ClinicService(ctx).ConsumeSupplyAsync(int.Parse(f[0].Value), int.Parse(f[1].Value)).Wait();
                        Console.WriteLine("Zapisano ubytek. Baza obliczyła UsedAmount!");
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.AddSupplyDeliveryForm, "Dostawa materiałów", "Procedura AddSupplyDelivery",
                [ new FormField(0, "ID Materiału", "", _ => true), new FormField(1, "Ilość dostarczona", "", _ => true) ],
                ScreensEnum.ReceptionMenu, ScreensEnum.ReceptionMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        new ClinicService(ctx).AddSupplyDeliveryAsync(int.Parse(f[0].Value), int.Parse(f[1].Value)).Wait();
                        Console.WriteLine("Dostawa wpisana pomyślnie.");
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.CreateNewSupplyForm, "Wprowadź nowy materiał do bazy", "Tworzy nowy wpis Supply",
                [ 
                    new FormField(0, "Nazwa materiału", "np. Gaza jałowa", _ => true), 
                    new FormField(1, "Opis", "np. 1x1m", _ => true),
                    new FormField(2, "Ilość początkowa", "np. 100", _ => true),
                    new FormField(3, "ID Pokoju", "Wpisz ID z listy powyżej", _ => true)
                ],
                ScreensEnum.ReceptionMenu, ScreensEnum.ReceptionMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        var supply = new Supply {
                            Name = f[0].Value,
                            Description = f[1].Value,
                            Quantity = int.Parse(f[2].Value),
                            RoomId = int.Parse(f[3].Value)
                        };
                        ctx.Supplies.Add(supply);
                        ctx.SaveChanges();
                        Console.WriteLine($"\nSukces! Dodano nowy materiał do bazy! ID przedmiotu to: {supply.Id}");
                    } catch(Exception e) { Console.WriteLine("\nBłąd: Upewnij się, że podałeś prawidłowe ID Pokoju i parametry! " + e.Message); }
                    Console.ReadKey();
                },
                () => {
                    using var ctx = new PrzychodniaContext();
                    var rooms = ctx.Rooms.Include(r => r.Department).ToList();
                    Console.WriteLine("\n=== DOSTĘPNE POKOJE W KLINICE ===");
                    foreach(var r in rooms) {
                        Console.WriteLine($"[ID: {r.Id}] {r.Purpose} -> Oddział: {r.Department?.Name}");
                    }
                    Console.WriteLine("=================================");
                }
            ));

            screens.Add(new Form(ScreensEnum.MonthlyCostsForm, "Raport Kosztów", "Funkcja MonthlyCosts (REF CURSOR)",
                [ new FormField(0, "Rok (np. 2026)", "", _ => true), new FormField(1, "Miesiąc (1-12)", "", _ => true) ],
                ScreensEnum.AdminMenu, ScreensEnum.AdminMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        var res = new ClinicService(ctx).GetMonthlyCostsAsync(int.Parse(f[0].Value), int.Parse(f[1].Value)).Result;
                        Console.WriteLine("\nRAPORT KOSZTÓW");
                        foreach(var r in res) Console.WriteLine($"Rok: {r.Year}, Miesiąc: {r.Month}, Koszt Pensji: {r.TotalSalariesCost} PLN");
                        Console.WriteLine();
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.AddWorkerForm, "Dodaj nowego pracownika", "Rejestracja w tabeli Workers",
                [
                    new FormField(0, "Imię", "", _ => true),
                    new FormField(1, "Nazwisko", "", _ => true),
                    new FormField(2, "Stanowisko", "np. Lekarz, Administrator, Recepcja", _ => true),
                    new FormField(3, "Telefon", "", _ => true),
                    new FormField(4, "Email", "", _ => true),
                    new FormField(5, "Hasło", "", _ => true),
                    new FormField(6, "Wynagrodzenie", "Podaj miesięczną pensję w PLN (np. 15000)", _ => true),
                    new FormField(7, "ID Pokoju", "Dla lekarza wymagane, dla reszty opcjonalne (wpisz ID lub zostaw puste)", _ => true)
                ],
                ScreensEnum.AdminMenu, ScreensEnum.AdminMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        if (ctx.Workers.Any(w => w.Email == f[4].Value)) {
                            Console.WriteLine("\nBłąd: Pracownik z takim adresem e-mail już istnieje w bazie!");
                            Console.ReadKey();
                            return;
                        }
                        var worker = new Worker {
                            FirstName = f[0].Value,
                            LastName = f[1].Value,
                            Position = f[2].Value,
                            Phone = f[3].Value,
                            Email = f[4].Value,
                            PasswordHash = f[5].Value,
                            Salary = int.TryParse(f[6].Value, out int s) ? s : 0
                        };

                        string roomIdStr = f[7].Value;
                        if (!string.IsNullOrWhiteSpace(roomIdStr) && int.TryParse(roomIdStr, out int rId)) {
                            var room = ctx.Rooms.FirstOrDefault(r => r.Id == rId);
                            if (room == null) {
                                Console.WriteLine("\nBłąd: Pokój o podanym ID nie istnieje w bazie!");
                                Console.ReadKey();
                                return;
                            }
                            worker.RoomId = rId;
                            room.Status = "Zajęty"; // Aktualizujemy status pokoju na Zajęty
                        } else {
                            string pos = f[2].Value.ToLower();
                            var excludedPositions = new[] { "administrator", "owner", "reception", "nurse", "recepcja", "pielęgniarka", "właściciel" };
                            if (!excludedPositions.Contains(pos)) {
                                Console.WriteLine("\nBłąd: Lekarz musi mieć obowiązkowo przypisany pokój (gabinet)!");
                                Console.ReadKey();
                                return;
                            }
                        }

                        ctx.Workers.Add(worker);
                        ctx.SaveChanges();
                        Console.WriteLine("\nDodano pracownika pomyślnie! " + (worker.RoomId != null ? "Zaktualizowano status pokoju na Zajęty." : ""));
                    } catch(Exception e) { Console.WriteLine("Błąd EF: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.AddEquipmentForm, "Dodaj nowy sprzęt medyczny", "Tworzy nowy wpis Equipment",
                [
                    new FormField(0, "Nazwa sprzętu", "np. Aparat USG", _ => true),
                    new FormField(1, "Opis", "np. Model XYZ", _ => true),
                    new FormField(2, "Stan", "np. Nowy, Używany", _ => true),
                    new FormField(3, "Wartość (PLN)", "np. 50000", _ => true),
                    new FormField(4, "Data zakupu (RRRR-MM-DD)", "np. 2026-06-15", _ => true),
                    new FormField(5, "ID Pokoju", "Wpisz ID z listy powyżej", _ => true)
                ],
                ScreensEnum.AdminMenu, ScreensEnum.AdminMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        var eq = new Equipment {
                            Name = f[0].Value,
                            Description = f[1].Value,
                            Condition = f[2].Value,
                            Value = int.TryParse(f[3].Value, out int val) ? val : 0,
                            PurchaseDate = DateTime.ParseExact(f[4].Value, "yyyy-MM-dd", null),
                            LastInspection = DateTime.Now,
                            RoomId = int.Parse(f[5].Value)
                        };
                        ctx.Equipment.Add(eq);
                        ctx.SaveChanges();
                        Console.WriteLine($"\nSukces! Dodano nowy sprzęt do bazy! ID to: {eq.Id}");
                    } catch(Exception e) { Console.WriteLine("\nBłąd: Upewnij się, że podałeś prawidłowe dane! " + e.Message); }
                    Console.ReadKey();
                },
                () => {
                    using var ctx = new PrzychodniaContext();
                    var rooms = ctx.Rooms.Include(r => r.Department).ToList();
                    Console.WriteLine("\n=== DOSTĘPNE POKOJE W KLINICE ===");
                    foreach(var r in rooms) {
                        Console.WriteLine($"[ID: {r.Id}] {r.Purpose} -> Oddział: {r.Department?.Name}");
                    }
                    Console.WriteLine("=================================");
                }
            ));

            screens.Add(new Form(ScreensEnum.SuppliesUsageReportForm, "Raport Zużycia Materiałów", "Funkcja SuppliesUsageReport",
                [ new FormField(0, "Data od (RRRR-MM-DD)", "", _ => true), new FormField(1, "Data do (RRRR-MM-DD)", "", _ => true) ],
                ScreensEnum.AdminMenu, ScreensEnum.AdminMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        var res = new ClinicService(ctx).GetSuppliesUsageReportAsync(DateTime.Parse(f[0].Value), DateTime.Parse(f[1].Value)).Result;
                        Console.WriteLine("\nRAPORT ZUŻYCIA MATERIAŁÓW");
                        foreach(var r in res) Console.WriteLine($"Materiał: {r.Name}, Sumaryczne zużycie: {r.TotalUsed}");
                        Console.WriteLine();
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Form(ScreensEnum.DoctorPatientHistoryForm, "Historia Medyczna Pacjenta", "Wyszukaj historię po ID",
                [ new FormField(0, "ID Pacjenta", "", _ => true) ],
                ScreensEnum.DoctorMenu, ScreensEnum.DoctorMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        var data = new ClinicService(ctx).GetPatientMedicalHistoryViewAsync(int.Parse(f[0].Value)).Result;
                        Console.WriteLine($"\nHISTORIA PACJENTA (ID: {f[0].Value})");
                        foreach (var d in data) Console.WriteLine($"Data: {d.VisitDate}, Diagnoza: {d.Illness}, Lekarz: {d.DoctorFirstName} {d.DoctorLastName}");
                        Console.WriteLine();
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                    Console.ReadKey();
                }
            ));

            screens.Add(new Menu(ScreensEnum.CalculateAssetsValueScreen, "Wartość majątku przychodni", "Funkcja CalculateAssetsValue", ScreensEnum.AdminMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.AdminMenu) ], 
                () => {
                    try {
                        using var context = new PrzychodniaContext();
                        var value = new ClinicService(context).CalculateAssetsValueAsync().Result;
                        Console.WriteLine($"\n[BAZA ORACLE] Łączna wartość sprawnego sprzętu medycznego: {value} PLN\n");
                    } catch(Exception e) { Console.WriteLine("Błąd: " + e.Message); }
                }
            ));

            
            
            screens.Add(new Menu(ScreensEnum.MyMedicalHistoryView, "Historia Medyczna (Widok Oracle)", "Podgląd z v_PatientMedicalHistory", ScreensEnum.PatientMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.PatientMenu) ], 
                () => {
                    if (Session.CurrentPatient != null) {
                        using var ctx = new PrzychodniaContext();
                        var data = new ClinicService(ctx).GetPatientMedicalHistoryViewAsync(Session.CurrentPatient.Id).Result;
                        Console.WriteLine($"\nHISTORIA PACJENTA {Session.CurrentPatient.FirstName}");
                        foreach (var d in data) Console.WriteLine($"Data: {d.VisitDate}, Diagnoza: {d.Illness}, Lekarz: {d.DoctorFirstName} {d.DoctorLastName}");
                        Console.WriteLine();
                    }
                }
            ));

            Action printDoctorAvailability = () => {
                using var ctx = new PrzychodniaContext();
                var data = new ClinicService(ctx).GetDoctorAvailabilityViewAsync().Result;
                Console.WriteLine("\nZAJĘTE TERMINY LEKARZY");
                foreach (var d in data) Console.WriteLine($"[ID: {d.DoctorId}] Lekarz: {d.FirstName} {d.LastName} ({d.Specialization}) -> Zajęty od {d.Start} do {d.End}");
                Console.WriteLine();
            };

            screens.Add(new Menu(ScreensEnum.DoctorAvailabilityView_Reception, "Grafik Lekarzy (Widok Oracle)", "Podgląd z v_DoctorAvailability", ScreensEnum.ReceptionMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.ReceptionMenu) ], 
                printDoctorAvailability
            ));

            screens.Add(new Menu(ScreensEnum.DoctorAvailabilityView_Patient, "Dostępność Lekarzy (Widok Oracle)", "Podgląd z v_DoctorAvailability", ScreensEnum.PatientMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.PatientMenu) ], 
                printDoctorAvailability
            ));

            screens.Add(new ConsoleList<Visit>(
                ScreensEnum.DoctorAvailabilityView_Doctor, "Mój Grafik (Moje wizyty)", "Oczekujący pacjenci (bez diagnozy)", ScreensEnum.DoctorMenu, 
                null, new List<Visit>(), 
                [
                    new ListColumn<Visit>("ID Wizyty", v => v.Id.ToString()),
                    new ListColumn<Visit>("ID Pacjenta", v => v.PatientId.ToString()),
                    new ListColumn<Visit>("Cel wizyty", v => v.Purpose),
                    new ListColumn<Visit>("Data", v => v.Start.ToString("yyyy-MM-dd HH:mm")),
                    new ListColumn<Visit>("Koniec", v => v.End.ToString("HH:mm"))
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.DoctorAvailabilityView_Doctor) as ConsoleList<Visit>;
                    if(Session.CurrentWorker != null) {
                        listScreen.Items = ctx.Visits.Where(v => v.WorkerId == Session.CurrentWorker.Id && v.DiagnosisId == null).OrderBy(v => v.Start).ToList();
                    }
                }
            ));

            screens.Add(new Menu(ScreensEnum.LowStockSuppliesView, "Braki w Magazynie (Widok Oracle)", "Podgląd z v_LowStockSupplies", ScreensEnum.ReceptionMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.ReceptionMenu) ], 
                () => {
                    using var ctx = new PrzychodniaContext();
                    var data = new ClinicService(ctx).GetLowStockSuppliesViewAsync().Result;
                    if (!data.Any()) {
                        Console.WriteLine("\nSukces! Wszystkie stany magazynowe w normie. Brak przedmiotów poniżej 10 sztuk.");
                    } else {
                        Console.WriteLine("\nUWAGA! NISKI STAN (Poniżej 10 sztuk)");
                        foreach (var d in data) Console.WriteLine($"Id: {d.SupplyId}, Sprzęt: {d.SupplyName}, Sztuk: {d.Quantity}, Gdzie: Pokój {d.RoomNumber} ({d.RoomName})");
                    }
                    Console.WriteLine();
                }
            ));

            screens.Add(new ConsoleList<Worker>(
                ScreensEnum.StaffList, "Lista Pracowników", "Pracownicy przychodni", ScreensEnum.AdminMenu, 
                null, new List<Worker>(), 
                [
                    new ListColumn<Worker>("ID", w => w.Id.ToString()),
                    new ListColumn<Worker>("Imię", w => w.FirstName),
                    new ListColumn<Worker>("Nazwisko", w => w.LastName),
                    new ListColumn<Worker>("Stanowisko", w => w.Position),
                    new ListColumn<Worker>("Telefon", w => w.Phone)
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.StaffList) as ConsoleList<Worker>;
                    listScreen.Items = ctx.Workers.ToList();
                }
            ));

            screens.Add(new ConsoleList<Patient>(
                ScreensEnum.PatientList_Reception, "Lista Pacjentów", "Zarejestrowani pacjenci", ScreensEnum.ReceptionMenu, 
                null, new List<Patient>(), 
                [
                    new ListColumn<Patient>("ID", p => p.Id.ToString()),
                    new ListColumn<Patient>("Imię", p => p.FirstName),
                    new ListColumn<Patient>("Nazwisko", p => p.LastName),
                    new ListColumn<Patient>("Telefon", p => p.Phone),
                    new ListColumn<Patient>("Email", p => p.Email)
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.PatientList_Reception) as ConsoleList<Patient>;
                    listScreen.Items = ctx.Patients.ToList();
                }
            ));

            screens.Add(new ConsoleList<Patient>(
                ScreensEnum.PatientList_Doctor, "Lista Pacjentów", "Zarejestrowani pacjenci", ScreensEnum.DoctorMenu, 
                null, new List<Patient>(), 
                [
                    new ListColumn<Patient>("ID", p => p.Id.ToString()),
                    new ListColumn<Patient>("Imię", p => p.FirstName),
                    new ListColumn<Patient>("Nazwisko", p => p.LastName),
                    new ListColumn<Patient>("Telefon", p => p.Phone)
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.PatientList_Doctor) as ConsoleList<Patient>;
                    listScreen.Items = ctx.Patients.ToList();
                }
            ));

            Action loadSupplies = () => {
                using var ctx = new PrzychodniaContext();
                var listAdmin = screens.FirstOrDefault(s => s.Id == ScreensEnum.SupplyList_Admin) as ConsoleList<Supply>;
                if (listAdmin != null) listAdmin.Items = ctx.Supplies.ToList();

                var listRec = screens.FirstOrDefault(s => s.Id == ScreensEnum.SupplyList_Reception) as ConsoleList<Supply>;
                if (listRec != null) listRec.Items = ctx.Supplies.ToList();

                var listDoc = screens.FirstOrDefault(s => s.Id == ScreensEnum.SupplyList_Doctor) as ConsoleList<Supply>;
                if (listDoc != null) listDoc.Items = ctx.Supplies.ToList();
            };

            var supplyColumns = new List<ListColumn<Supply>> {
                new ListColumn<Supply>("ID", s => s.Id.ToString()),
                new ListColumn<Supply>("Nazwa", s => s.Name),
                new ListColumn<Supply>("Ilość", s => s.Quantity.ToString()),
                new ListColumn<Supply>("Opis", s => s.Description)
            };

            screens.Add(new ConsoleList<Supply>(ScreensEnum.SupplyList_Admin, "Lista materiałów", "Stan magazynu", ScreensEnum.AdminMenu, null, new List<Supply>(), supplyColumns, loadSupplies));
            screens.Add(new ConsoleList<Supply>(ScreensEnum.SupplyList_Reception, "Lista materiałów", "Stan magazynu", ScreensEnum.ReceptionMenu, null, new List<Supply>(), supplyColumns, loadSupplies));
            screens.Add(new ConsoleList<Supply>(ScreensEnum.SupplyList_Doctor, "Lista materiałów", "Stan magazynu", ScreensEnum.DoctorMenu, null, new List<Supply>(), supplyColumns, loadSupplies));

            screens.Add(new ConsoleList<Room>(
                ScreensEnum.RoomList_Admin, "Lista Pokoi", "Wszystkie gabinety", ScreensEnum.AdminMenu, 
                null, new List<Room>(), 
                [
                    new ListColumn<Room>("ID", r => r.Id.ToString()),
                    new ListColumn<Room>("Piętro", r => r.Floor.ToString()),
                    new ListColumn<Room>("Przeznaczenie", r => r.Purpose),
                    new ListColumn<Room>("Status", r => r.Status)
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.RoomList_Admin) as ConsoleList<Room>;
                    listScreen.Items = ctx.Rooms.ToList();
                }
            ));

            screens.Add(new ConsoleList<Visit>(
                ScreensEnum.VisitList_Reception, "Lista Wizyt", "Wszystkie zaplanowane wizyty", ScreensEnum.ReceptionMenu, 
                null, new List<Visit>(), 
                [
                    new ListColumn<Visit>("ID", v => v.Id.ToString()),
                    new ListColumn<Visit>("ID Pacjenta", v => v.PatientId.ToString()),
                    new ListColumn<Visit>("Lekarz ID", v => v.WorkerId.ToString()),
                    new ListColumn<Visit>("Cel wizyty", v => v.Purpose),
                    new ListColumn<Visit>("Data", v => v.Start.ToString("yyyy-MM-dd HH:mm"))
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.VisitList_Reception) as ConsoleList<Visit>;
                    listScreen.Items = ctx.Visits.ToList();
                }
            ));

            screens.Add(new ConsoleList<Visit>(
                ScreensEnum.VisitList_Patient, "Moje zaplanowane wizyty", "Twoje nadchodzące i przeszłe wizyty", ScreensEnum.PatientMenu, 
                null, new List<Visit>(), 
                [
                    new ListColumn<Visit>("ID Wizyty", v => v.Id.ToString()),
                    new ListColumn<Visit>("Lekarz ID", v => v.WorkerId.ToString()),
                    new ListColumn<Visit>("Cel wizyty", v => v.Purpose),
                    new ListColumn<Visit>("Data", v => v.Start.ToString("yyyy-MM-dd HH:mm")),
                    new ListColumn<Visit>("Koszt", v => v.Cost.ToString())
                ],
                () => {
                    using var ctx = new PrzychodniaContext();
                    var listScreen = screens.First(s => s.Id == ScreensEnum.VisitList_Patient) as ConsoleList<Visit>;
                    if(Session.CurrentPatient != null) {
                        listScreen.Items = ctx.Visits.Where(v => v.PatientId == Session.CurrentPatient.Id).OrderBy(v => v.Start).ToList();
                    }
                }
            ));

            Action loadDoctors = () => {
                using var ctx = new PrzychodniaContext();
                var excludedPositions = new[] { "administrator", "owner", "reception", "nurse", "recepcja", "pielęgniarka", "właściciel" };
                var doctors = ctx.Workers.Where(w => !excludedPositions.Contains(w.Position.ToLower())).ToList();

                var listPat = screens.FirstOrDefault(s => s.Id == ScreensEnum.DoctorList_Patient) as ConsoleList<Worker>;
                if (listPat != null) listPat.Items = doctors;

                var listRec = screens.FirstOrDefault(s => s.Id == ScreensEnum.DoctorList_Reception) as ConsoleList<Worker>;
                if (listRec != null) listRec.Items = doctors;
            };

            var doctorColumns = new List<ListColumn<Worker>> {
                new ListColumn<Worker>("ID", w => w.Id.ToString()),
                new ListColumn<Worker>("Imię", w => w.FirstName),
                new ListColumn<Worker>("Nazwisko", w => w.LastName),
                new ListColumn<Worker>("Stanowisko", w => w.Position)
            };

            screens.Add(new ConsoleList<Worker>(ScreensEnum.DoctorList_Patient, "Lista Lekarzy", "Wszyscy lekarze w przychodni", ScreensEnum.PatientMenu, null, new List<Worker>(), doctorColumns, loadDoctors));
            screens.Add(new ConsoleList<Worker>(ScreensEnum.DoctorList_Reception, "Lista Lekarzy", "Wszyscy lekarze w przychodni", ScreensEnum.ReceptionMenu, null, new List<Worker>(), doctorColumns, loadDoctors));

            return screens;
        }
    }
}
