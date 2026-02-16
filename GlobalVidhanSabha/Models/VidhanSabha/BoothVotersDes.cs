using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class    BoothVotersDes
    {
        public int Id { get; set; }
        public String BoothName { get; set; }
        public String BoothNumber { get; set; }
        public String PollingStationBoothName { get; set; }
        public int TotalVoters { get; set; }
        public int TotalMan { get; set; }
        public int TotalWoman { get; set; }
        public int TotalOther { get; set; }
        public int BoothId { get; set; }

        // Optional: add if needed
        public string SectorName { get; set; }
        public string MandalName { get; set; }
        public string VillageListId { get; set; }
        public string VillageName { get; set; }

     

    }
}