using System;
using System.Collections.Generic;
using System.Text;

using Projekt_SBD.Models;
using System;
using System.Linq;

namespace Projekt_SBD.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PrzychodniaContext context) 
        {
            // Pobieramy lub tworzymy departament
            var department = context.Departments.FirstOrDefault(d => d.Name == "Kardiologia");
            if (department == null)
            {
                department = new Department { Name = "Kardiologia", Room = 101 };
                context.Departments.Add(department);
                context.SaveChanges();
            }

            // Pobieramy lub tworzymy pokój
            var room = context.Rooms.FirstOrDefault(r => r.DepartmentId == department.Id);
            if (room == null)
            {
                room = new Room 
                { 
                    Floor = 1, 
                    Purpose = "Gabinet Konsultacyjny", 
                    Status = "Wolny", 
                    DepartmentId = department.Id 
                };
                context.Rooms.Add(room);
                context.SaveChanges();
            }

            // Pobieramy lub tworzymy lekarza
            var doctor = context.Workers.FirstOrDefault(w => w.Email == "jan.kowalski@przychodnia.pl");
            if (doctor == null)
            {
                doctor = new Worker 
                { 
                    FirstName = "Jan",
                    LastName = "Kowalski", 
                    Email = "jan.kowalski@przychodnia.pl",
                    PasswordHash = "Admin123",
                    Phone = "111222333", 
                    Position = "Cardiologist", 
                    Salary = 15000, 
                    RoomId = room.Id 
                };
                context.Workers.Add(doctor);
            }

            // Pobieramy lub tworzymy pielęgniarkę
            var nurse = context.Workers.FirstOrDefault(w => w.Email == "anna.nowak@przychodnia.pl");
            if (nurse == null)
            {
                nurse = new Worker 
                { 
                    FirstName = "Anna", 
                    LastName = "Nowak", 
                    Email = "anna.nowak@przychodnia.pl",
                    PasswordHash = "Admin123",
                    Phone = "444555666", 
                    Position = "Nurse", 
                    Salary = 6000 
                };
                context.Workers.Add(nurse);
            }
            context.SaveChanges();

            // Pobieramy lub tworzymy pacjenta
            var patient = context.Patients.FirstOrDefault(p => p.Email == "michal@test.pl");
            if (patient == null)
            {
                patient = new Patient 
                { 
                    FirstName = "Michał", 
                    LastName = "Testowy", 
                    Email = "michal@test.pl",
                    PasswordHash = "Admin123",
                    Phone = "999888777" 
                };
                context.Patients.Add(patient);
                context.SaveChanges();
            }

            // Wyposażenie
            if (!context.Equipment.Any(e => e.Name == "Defibrylator"))
            {
                var equipment = new Equipment 
                { 
                    Name = "Defibrylator", 
                    Description = "Sprzęt ratujący życie", 
                    Condition = "Nowy", 
                    Value = 12000, 
                    PurchaseDate = new DateTime(2023, 1, 15), 
                    LastInspection = new DateTime(2024, 1, 10), 
                    RoomId = room.Id 
                };
                context.Equipment.Add(equipment);
            }

            // Zasoby (Materiały)
            if (!context.Supplies.Any(s => s.Name == "Strzykawka 5ml"))
            {
                var supply = new Supply 
                { 
                    Name = "Strzykawka 5ml", 
                    Description = "Jednorazowa", 
                    Quantity = 500, 
                    RoomId = room.Id 
                };
                context.Supplies.Add(supply);
            }
            context.SaveChanges();

            // Ustawiamy stałe daty, aby móc je precyzyjnie sprawdzać w bazie
            DateTime diagnosisDate = new DateTime(2024, 2, 10, 10, 0, 0);
            DateTime visitStart = new DateTime(2024, 2, 15, 12, 0, 0);
            DateTime visitEnd = new DateTime(2024, 2, 15, 12, 30, 0);

            // Diagnoza
            var diagnosis = context.Diagnosis.FirstOrDefault(d => d.PatientId == patient.Id && d.DiagnosisTime == diagnosisDate);
            if (diagnosis == null)
            {
                diagnosis = new Diagnosis
                {
                    DiagnosisTime = diagnosisDate,
                    Symptoms = "Wysokie ciśnienie, ból głowy",
                    Illness = "Nadciśnienie tętnicze",
                    WorkerId = doctor.Id,
                    PatientId = patient.Id
                };
                context.Diagnosis.Add(diagnosis);
                context.SaveChanges();
            }

            // Wizyta 
            if (!context.Visits.Any(v => v.PatientId == patient.Id && v.Start == visitStart))
            {
                var visit = new Visit 
                { 
                    Purpose = "Ból w klatce piersiowej", 
                    Start = visitStart, 
                    End = visitEnd, 
                    Cost = 200, 
                    PatientId = patient.Id, 
                    RoomId = room.Id,
                    WorkerId = doctor.Id,
                    DiagnosisId = diagnosis.Id
                };
                context.Visits.Add(visit);
                context.SaveChanges();
            }

            // Harmonogram (Dla lekarza na dany dzień)
            if (!context.Schedules.Any(s => s.WorkerId == doctor.Id && s.Day == "Monday"))
            {
                var schedule = new Schedule
                {
                    Description = "Poranna zmiana",
                    Day = "Monday",
                    StartHour = 8,
                    EndHour = 16,
                    WorkerId = doctor.Id
                };
                context.Schedules.Add(schedule);
                context.SaveChanges();
            }
        }
    }
}