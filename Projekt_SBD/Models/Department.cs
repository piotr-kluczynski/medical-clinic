using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
