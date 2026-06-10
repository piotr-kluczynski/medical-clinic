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

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int? DiagnosisId { get; set; }
        public Diagnosis Diagnosis { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
