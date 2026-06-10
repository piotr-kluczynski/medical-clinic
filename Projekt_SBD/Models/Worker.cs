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
        public string PasswordHash { get; set; }

        public string Position { get; set; }
        public int Salary { get; set; }

        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    }
}
