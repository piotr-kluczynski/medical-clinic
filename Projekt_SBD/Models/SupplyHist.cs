using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_SBD.Models
{
    [Table("Supplies_HIST")]
    public class SupplyHist
    {
        [Key]
        public int HistId { get; set; }
        
        public string ActionType { get; set; }
        public DateTime ArchiveDate { get; set; }
        public string ArchiveUser { get; set; }

        public int OriginalSupplyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int RoomId { get; set; }
    }
}
