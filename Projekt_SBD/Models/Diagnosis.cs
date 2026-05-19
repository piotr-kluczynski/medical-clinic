using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public DateTime DiagnosisTime { get; set; }
        // Clinic worker reference (doctor who made diagnosis)
        // Patient reference
        public string Symptoms { get; set; }
        public string Illness { get; set; }
    }
}
