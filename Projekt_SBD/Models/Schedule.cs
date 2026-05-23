using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Description { get; set; } // Details about the type of workshift
        public string Day { get; set; } // Change later to enum
        public int StartHour { get; set; }
        public int EndHour { get; set; }

        // Relacja: Grafik należy do konkretnego Pracownika
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
