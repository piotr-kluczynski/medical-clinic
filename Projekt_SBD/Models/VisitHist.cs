using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_SBD.Models
{
    [Table("Visits_HIST")]
    public class VisitHist
    {
        [Key]
        public int HistId { get; set; }
        
        public string ActionType { get; set; }
        public DateTime ArchiveDate { get; set; }
        public string ArchiveUser { get; set; }

        public int OriginalVisitId { get; set; }
        public string Purpose { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Cost { get; set; }
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public int? DiagnosisId { get; set; }
        public int WorkerId { get; set; }
    }
}
