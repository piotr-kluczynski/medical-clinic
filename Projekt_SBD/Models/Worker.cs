using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Position { get; set; } // praca jaką wykonuje
        public int Salary { get; set; }

        // Relacja: Pracownik może mieć przypisany swój stały pokój/gabinet
        public int? RoomId { get; set; }
        public Room Room { get; set; }

        // Relacje odwrotne by łatwo przeglądac poszczególe rzeczy przypisane do pracownika
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    }
}
