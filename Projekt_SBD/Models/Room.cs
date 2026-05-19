using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Room
    {
        public int Id { get; set; } // Also the room number
        public int Floor { get; set; }
        public string Purpose { get; set; } // Change to enum later
        
        // Department reference
    }
}
