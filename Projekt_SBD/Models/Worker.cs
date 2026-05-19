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

        public string Position { get; set; } // Job the worker is performing
        public int Salary { get; set; }
        // Room reference - room assigned for the worker (optionally)
    }
}
