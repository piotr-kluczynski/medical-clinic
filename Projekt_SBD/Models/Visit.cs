using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Visit
    {
        public int Id { get; set; }
        // Patient reference
        public string Purpose { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        // Assigned room reference
        public int Cost { get; set; }
        // Diagnosis reference
    
    }
}
