using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class Mandal
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TotalSectors { get; set; }
        public int TotalBooths { get; set; }
        public int TotalVotes { get; set; }
        public int SeniorCount { get; set; }
        public int DisabledCount { get; set; }
        public int DoubleVotes { get; set; }
        public int PravasiCount { get; set; }
        public int EffectivePerson { get; set; }
        public int MandalId { get; set; }
        public int SectorId { get; set; }
        public string SectorName { get; set; }

    }

    public class MandalSectorBoothCount
    {
        public int mandalId { get; set; }
        public string MandalName { get; set; }
        public int sectorcount { get; set; }
        public int boothcount { get; set; }
    }

    public class DashboardCount
    {
        public int TotalMandal { get; set; }
        public int TotalSector { get; set; }
        public int TotalBooth { get; set; }
        public int TotalPanna { get; set; }
        public int TotalPravasiVoter { get; set; }
        public int TotalNewVoter { get; set; }
        public int TotalDoublevoter { get; set; }
        public int TotalEffectivePerson { get; set; }
        public int TotalSeniorCitizen { get; set; }
        public int TotalHandicap { get; set; }
        public int TotalBoothVoterDes { get; set; }
        public int TotalSatisfied { get; set; }
        public int TotalUnsatisfied { get; set; }
        public int TotalActivity { get; set; }
        public int TotalBDC { get; set; }
        public int TotalBlock { get; set; }
        public int TotalBoothSamiti { get; set; }
        public int TotalSocialMediaPost { get; set; }
        public int TotalInfluencer { get; set; }
       
    }

    public class VillageList
    {
        public int villageId { get; set; }
        public int AnshikId { get; set; }
        public string Villagename { get; set; }
       
    }

 

}