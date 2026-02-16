using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class Influencer
    {
        public int? Id { get; set; }
        public bool IsEffective { get; set; }
        public int? Designation { get; set; }
        public int? PersonId { get; set; }
        public int? BoothId { get; set; }
        public string PersonName { get; set; }
        public string PollingStationBoothName { get; set; }
        public int? Category { get; set; }
        public int? Caste { get; set; }
        public long? Mobile { get; set; }
        public string Description { get; set; }
        public string EffectiveDesignationdata { get; set; }
        public string BoothNumber { get; set; }
        public string CategoryName { get; set; }
        public string SubCasteName { get; set; }
        public string VillageListId { get; set; }
        public string VillageListName{ get; set; }
    }
}