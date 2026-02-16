using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class CombinedSectorBoothReport
    {
        public string MandalName { get; set; }
        public string SectorName { get; set; }
        public string PollingStationBoothName { get; set; }
        public string InchargeName { get; set; }
        public string SectorPhoneNumber { get; set; }
        public List<string> SectorVillageNames { get; set; }
        public string SectorFatherName { get; set; }
        public int? SectorAge { get; set; }
        public string SectorCaste { get; set; }
        public string SectorAddress { get; set; }
        public string SectorEducation { get; set; }
        public string SectorProfileImage { get; set; }

        // Booth data
        public string BoothNumber { get; set; }
        public string BoothName { get; set; }
        public string BoothIncharge { get; set; }
        public string BoothPhone { get; set; }
        public List<string> BoothVillageNames { get; set; }
        public string BoothFatherName { get; set; }
        public int? BoothAge { get; set; }
        public string BoothCaste { get; set; }
        public string BoothAddress { get; set; }
        public string BoothEducation { get; set; }
        public string BoothProfileImage { get; set; }
    }
}