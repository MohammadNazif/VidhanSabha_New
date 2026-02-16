using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class PravasiVoter
    {
        public int id { get; set; }
        public int boothno { get; set; }   // not boothid (match form & DB SP param)
        public string name { get; set; }
        public string PollingStationBoothName { get; set; }
        public int Category { get; set; }
        public int Caste { get; set; }
        public string mobile { get; set; }
        public string currentAddress { get; set; }

        // Extra (for display only, not for insert/update)
        public string CasteName { get; set; }
        public string BoothNumber { get; set; }
        //public int Boothnumber { get; set; }
        public string CategoryName { get; set; }
        public int boothid { get; set; }
        public string Occupation { get; set; }
        public int Occupations { get; set; }

        public string BoothIncharge { get; set; }
        public string SectorIncharge { get; set; }
        public string SectorName { get; set; }
        public List<string> VillageNames { get; set; }

        //Booth pravasi
        public string BoothName { get; set; }
        public string MandalName { get; set; }
        public string VillageListId { get; set; }
        public string VillageListName { get; set; }
        public string SectorId { get; set; }
        //public string sectorName { get; set; }
        public string SectorInchargeName { get; set; }

    }

    public class PravasiList
    {
        public int id { get; set; }

        public string MandalName { get; set; }
        public string SectorName { get; set; }
        public string BoothNo { get; set; }
        public string BoothName { get; set; }
        public string name { get; set; }
        public string Category { get; set; }
        public string Caste { get; set; }
        public string Mobile { get; set; }
        public string CurrAddress { get; set; }
        public string Occupation { get; set; }
        public string Village { get; set; }
        public string PollingStationBoothName { get; set; }


    }

}