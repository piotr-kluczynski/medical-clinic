using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        // Relacja: Pacjent może mieć przypisanego innego powiązanego pacjenta
        public int? RelatedPatientId { get; set; }
        public Patient RelatedPatient { get; set; }
        // Historia pacjenta
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>(); 
    }
}
