using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public DateTime DiagnosisTime { get; set; }   
        public string Symptoms { get; set; }
        public string Illness { get; set; }
        // Relacja: Diagnozę wystawia konkretny Pracownik (Lekarz)
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        // Relacja: Diagnoza jest przypisana do konkretnego Pacjenta
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
