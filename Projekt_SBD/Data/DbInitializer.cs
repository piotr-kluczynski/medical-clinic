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
            if (context.Patients.Any())
            {
                return;
            }

            var department = new Department 
            { 
                Name = "Kardiologia", 
                Room = 101 
            };
            context.Departments.Add(department);
            context.SaveChanges();

            var room = new Room 
            { 
                Floor = 1, 
                Purpose = "Gabinet Konsultacyjny", 
                Status = "Wolny", 
                DepartmentId = department.Id 
            };
            context.Rooms.Add(room);
            context.SaveChanges();

            var doctor = new Worker 
            { 
                FirstName = "Jan",
                LastName = "Kowalski", 
                Email = "jan.kowalski@przychodnia.pl", 
                Phone = "111222333", 
                Position = "Cardiologist", 
                Salary = 15000, 
                RoomId = room.Id 
            };
            var nurse = new Worker 
            { 
                FirstName = "Anna", 
                LastName = "Nowak", 
                Email = "anna.nowak@przychodnia.pl",
                Phone = "444555666", 
                Position = "Nurse", 
                Salary = 6000 
            };
            context.Workers.AddRange(doctor, nurse);
            context.SaveChanges();

            var patient = new Patient 
            { 
                FirstName = "Michał", 
                LastName = "Testowy", 
                Email = "michal@test.pl", 
                Phone = "999888777" 
            };
            context.Patients.Add(patient);
            context.SaveChanges();

            var equipment = new Equipment 
            { 
                Name = "Defibrylator", 
                Description = "Sprzęt ratujący życie", 
                Condition = "Nowy", 
                Value = 12000, 
                PurchaseDate = DateTime.Now.AddYears(-1), 
                LastInspection = DateTime.Now.AddMonths(-1), 
                RoomId = room.Id 
            };
            context.Equipment.Add(equipment);

            var supply = new Supply 
            { 
                Name = "Strzykawka 5ml", 
                Description = "Jednorazowa", 
                Quantity = 500, 
                RoomId = room.Id 
            };
            context.Supplies.Add(supply);
            context.SaveChanges();

            var visit = new Visit 
            { 
                Purpose = "Ból w klatce piersiowej", 
                Start = DateTime.Now.AddDays(1), 
                End = DateTime.Now.AddDays(1).AddMinutes(30), 
                Cost = 200, 
                PatientId = patient.Id, 
                RoomId = room.Id 
            };
            context.Visits.Add(visit);
            context.SaveChanges();

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

            var diagnosis = new Diagnosis
            {
                DiagnosisTime = DateTime.Now.AddDays(-5),
                Symptoms = "Wysokie ciśnienie, ból głowy",
                Illness = "Nadciśnienie tętnicze",
                WorkerId = doctor.Id,
                PatientId = patient.Id
            };
            context.Diagnosis.Add(diagnosis);
            context.SaveChanges();

            visit.DiagnosisId = diagnosis.Id;
            context.SaveChanges();
        }
    }
}