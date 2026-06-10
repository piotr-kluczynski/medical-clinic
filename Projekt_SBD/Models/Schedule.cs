using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Day { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
