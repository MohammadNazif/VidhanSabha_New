using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class EffectivePerson
    {
        public int effectivePersonId { get; set; }
        public int Booth_Id { get; set; }

        public string PollingStationBoothName { get; set; }
        public string Designation { get; set; }
        public int DesignationId { get; set; }
     
        public string Name { get; set; }

        public string Category { get; set; }
        public string Cast { get; set; }
        public string CategoryName { get; set; }
        public string Castename { get; set; }

        public string Mobile { get; set; }
        public string village { get; set; }
        public string Description { get; set; }
        public int BoothNumber { get; set; }
        public bool Status { get; set; }
        public int BoothId { get; set; }
        public int SectorId { get; set; }

        public string BoothIncharge { get; set; }

        public string SectorIncharge { get; set; }
        public string SectorName { get; set; }
        public List<string> VillageNames { get; set; }

        public string Designationdata { get; set; }
        public string VillageListId { get; set; }
        public string VillageListName { get; set; }
    }

    public class EffectivePersonList
    {
        public int id { get; set; }
        public string MandalName { get; set; }
        public string SectorName { get; set; }
        public string BoothNo { get; set; }
        public string BoothName { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Caste { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public string Village { get; set; }
        public string PollingStationBoothName { get; set; }
    }
}