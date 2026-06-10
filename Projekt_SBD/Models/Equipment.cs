using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
        public int Value { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime LastInspection { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
