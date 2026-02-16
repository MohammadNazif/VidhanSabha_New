using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class SeniorOrDisabled
    {
        public int Id { get; set; }
        public int boothId { get; set; }
        public int SectorId { get; set; }
        public int BoothNumber { get; set; }

        public string SeniorOrDisabledStatus { get; set; }
      

        public string Name { get; set; }
        public string PollingStationBoothName { get; set; }

        public string Address { get; set; }

        public int? Caste { get; set; }
        public int? Category { get; set; }
        public string categoryid { get; set; }


        public string Mobile { get; set; }
        public string village { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string CategoryName { get; set; }
        public string SubCasteName { get; set; }
        public int Status { get; set; }

        public int BoothNo { get; set; }
        public string Contact { get; set; }
        public string SeniorOrDisabledType { get; set; }
        public int SeniorOrDisableddata { get; set; }
        public string CasteName { get; set; }

        public string BoothIncharge { get; set; }
        public string SectorIncharge { get; set; }
        public string SectorName { get; set; }
        public string VillageListId { get; set; }
        public List<string> VillageNames { get; set; }

    }

    public class SeniorOrDisabledRequest
    {
        public int boothId { get; set; }
        public int seniorOrDisableddata { get; set; }
        public List<SeniorOrDisabled> Members { get; set; }
    }
    public class SeniorCitizenList
    {
        public int id { get; set; }

        public string MandalName { get; set; }
        public string SectorName { get; set; }
        public string Boothno { get; set; }
        public string PollingStationBoothName { get; set; }
        public string BoothName { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Caste { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }

        public string Village { get; set; }
        public string SeniorOrDiasabled { get; set; }
    
    }
}