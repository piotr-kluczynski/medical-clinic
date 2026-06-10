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
        MonthlyCostsForm,

        PatientList,
        PatientDetails,
        StaffList,
        StaffDetails,
        EquipmentList,
        EquipmentDetails,
        SupplyList,
        SupplyDetails,
        DepartmentList,
        
        DoctorAvailabilityView_Reception,
        DoctorAvailabilityView_Doctor,
        DoctorAvailabilityView_Patient,
        LowStockSuppliesView,
        MyMedicalHistoryView,
        
        ScheduleVisitForm_Reception,
        ScheduleVisitForm_Patient,
        DoctorPatientHistoryForm,
        AddWorkerForm,
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
                    new MenuOption(2, "Sprawdź dostępność lekarzy", ScreensEnum.DoctorAvailabilityView_Patient),
                    new MenuOption(3, "Umów wizytę", ScreensEnum.ScheduleVisitForm_Patient),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(patientMenu);

            Menu adminMenu = new Menu(
                ScreensEnum.AdminMenu, "Panel Administratora", "Zarządzanie personelem i raportami.", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Dodaj nowego pracownika", ScreensEnum.AddWorkerForm),
                    new MenuOption(2, "Raport kosztów operacyjnych", ScreensEnum.MonthlyCostsForm),
                    new MenuOption(3, "Raport zużycia materiałów", ScreensEnum.SuppliesUsageReportForm),
                    new MenuOption(4, "Wylicz łączną wartość majątku", ScreensEnum.CalculateAssetsValueScreen),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(adminMenu);

            Menu receptionMenu = new Menu(
                ScreensEnum.ReceptionMenu, "Panel Recepcji", "Obsługa pacjentów i harmonogramów.", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Lista pacjentów", ScreensEnum.PatientList),
                    new MenuOption(2, "Grafik lekarzy (Widok)", ScreensEnum.DoctorAvailabilityView_Reception),
                    new MenuOption(3, "Umów wizytę (Procedura)", ScreensEnum.ScheduleVisitForm_Reception),
                    new MenuOption(4, "Odwołaj wizytę (Procedura)", ScreensEnum.CancelVisitForm),
                    new MenuOption(5, "Sprawdź braki w magazynie (Widok)", ScreensEnum.LowStockSuppliesView),
                    new MenuOption(6, "Przyjmij dostawę materiałów (Procedura)", ScreensEnum.AddSupplyDeliveryForm),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(receptionMenu);

            Menu doctorMenu = new Menu(
                ScreensEnum.DoctorMenu, "Panel Lekarza", "Dostęp do wizyt i pacjentów.", ScreensEnum.MainMenu,
                [
                    new MenuOption(1, "Mój grafik (Lista Wizyt)", ScreensEnum.DoctorAvailabilityView_Doctor),
                    new MenuOption(2, "Historia Medyczna Pacjenta (Widok)", ScreensEnum.DoctorPatientHistoryForm),
                    new MenuOption(3, "Zużyj materiał do zabiegu (Procedura)", ScreensEnum.ConsumeSupplyForm),
                    new MenuOption(0, "Wyloguj", ScreensEnum.MainMenu, null, _ => Session.Logout())
                ]
            );
            screens.Add(doctorMenu);


            screens.Add(new Form(ScreensEnum.ScheduleVisitForm_Reception, "Umów Wizytę (Recepcja)", "Zapełnij dane do procedury ScheduleVisit",
                [
                    new FormField(0, "ID Pacjenta", "", _ => true),
                    new FormField(1, "ID Lekarza", "Sprawdź w Grafiku Lekarzy", _ => true),
                    new FormField(2, "ID Pokoju", "", _ => true),
                    new FormField(3, "Cel wizyty", "", _ => true),
                    new FormField(4, "Koszt", "", _ => true)
                ],
                ScreensEnum.ReceptionMenu,
                ScreensEnum.ReceptionMenu,
                (formFields) => {
                    try {
                        using var context = new PrzychodniaContext();
                        var srv = new ClinicService(context);
                        srv.ScheduleVisitAsync(
                            int.Parse(formFields[0].Value), int.Parse(formFields[1].Value), int.Parse(formFields[2].Value),
                            DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1),
                            formFields[3].Value, int.Parse(formFields[4].Value)
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
                    new FormField(0, "ID Lekarza", "Sprawdź w dostępności lekarzy (np. 1, 2, 3)", _ => true),
                    new FormField(1, "ID Pokoju", "Domyślny pokój zabiegowy (np. 1)", _ => true),
                    new FormField(2, "Cel wizyty", "Opisz powód", _ => true)
                ],
                ScreensEnum.PatientMenu,
                ScreensEnum.PatientMenu,
                (formFields) => {
                    try {
                        using var context = new PrzychodniaContext();
                        var srv = new ClinicService(context);
                        srv.ScheduleVisitAsync(
                            Session.CurrentPatient.Id, int.Parse(formFields[0].Value), int.Parse(formFields[1].Value),
                            DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1),
                            formFields[2].Value, 100
                        ).Wait();
                        Console.WriteLine("Procedura wykonana pomyślnie. Udało się umówić wizytę.");
                    } catch (Exception ex) {
                        Console.WriteLine("Błąd Oracle: " + ex.Message);
                    }
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
                    new FormField(5, "Hasło", "", _ => true)
                ],
                ScreensEnum.AdminMenu, ScreensEnum.AdminMenu,
                (f) => {
                    try {
                        using var ctx = new PrzychodniaContext();
                        ctx.Workers.Add(new Worker {
                            FirstName = f[0].Value,
                            LastName = f[1].Value,
                            Position = f[2].Value,
                            Phone = f[3].Value,
                            Email = f[4].Value,
                            PasswordHash = f[5].Value
                        });
                        ctx.SaveChanges();
                        Console.WriteLine("Dodano pracownika pomyślnie!");
                    } catch(Exception e) { Console.WriteLine("Błąd EF: " + e.Message); }
                    Console.ReadKey();
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

            screens.Add(new Menu(ScreensEnum.DoctorAvailabilityView_Doctor, "Mój Grafik (Widok Oracle)", "Podgląd z v_DoctorAvailability", ScreensEnum.DoctorMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.DoctorMenu) ], 
                printDoctorAvailability
            ));

            screens.Add(new Menu(ScreensEnum.LowStockSuppliesView, "Braki w Magazynie (Widok Oracle)", "Podgląd z v_LowStockSupplies", ScreensEnum.ReceptionMenu, 
                [ new MenuOption(0, "Wróć", ScreensEnum.ReceptionMenu) ], 
                () => {
                    using var ctx = new PrzychodniaContext();
                    var data = new ClinicService(ctx).GetLowStockSuppliesViewAsync().Result;
                    Console.WriteLine("\nUWAGA! NISKI STAN");
                    foreach (var d in data) Console.WriteLine($"Id: {d.SupplyId}, Sprzęt: {d.SupplyName}, Sztuk: {d.Quantity}, Gdzie: Pokój {d.RoomNumber} ({d.RoomName})");
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
                ScreensEnum.PatientList, "Lista Pacjentów", "Zarejestrowani pacjenci", ScreensEnum.ReceptionMenu, 
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
                    var listScreen = screens.First(s => s.Id == ScreensEnum.PatientList) as ConsoleList<Patient>;
                    listScreen.Items = ctx.Patients.ToList();
                }
            ));

            return screens;
        }
    }
}
