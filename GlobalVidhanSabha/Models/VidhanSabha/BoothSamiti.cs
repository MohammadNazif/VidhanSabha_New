using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VishanSabha.Models
{
    public class BoothSamiti
    {
        [Key]
        public int BoothSamiti_Id { get; set; }

        public int TotalMembers { get; set; }
        [Required]
        public int BoothId { get; set; }
        public virtual Booth Booth { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; }
        public int DesignationId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Cast { get; set; }
        public string PollingStationBoothName { get; set; }

        public int? Age { get; set; }

        [StringLength(15)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        public bool Status { get; set; }

        public string BoothIncharge { get; set; }

        [Required]
        public string SubCasteName { get; set; }

        [StringLength(100)]
        public string BoothName { get; set; }

        public string BoothNumber { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public string MandalName { get; set; }
        public string sectorName { get; set; }

        public int Samiti_Id { get; set; }

        public string village { get; set; }



        public DateTime CreatedAt { get; set; }
    }
}