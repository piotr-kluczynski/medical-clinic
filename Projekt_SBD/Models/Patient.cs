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
        // Patient reference - other registered patients can access personal data
    }
}
