using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public string Purpose { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Cost { get; set; }

        // Relacja do Pacjenta
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        // Relacja do Pokoju
        public int RoomId { get; set; }
        public Room Room { get; set; }

        // Relacja do Diagnozy 
        public int? DiagnosisId { get; set; }
        public Diagnosis Diagnosis { get; set; }

        // Relacja do Pracownika (Lekarza)
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
