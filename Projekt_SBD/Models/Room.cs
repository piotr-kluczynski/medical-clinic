using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_SBD.Models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int Floor { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public ICollection<Worker> Workers { get; set; } = new List<Worker>();
    }
}
