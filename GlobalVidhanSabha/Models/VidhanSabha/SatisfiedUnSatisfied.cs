using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class SatisfiedUnSatisfied
    {
        public int id { get; set; }
        public int boothNo { get; set; }
        public string village { get; set; }
        public int sahmatAsahmat { get; set; }
        public int boothid { get; set; }

        public string sahmatAsahmatName { get; set; }
        public string PollingStationBoothName { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string mobile { get; set; }
        public string party { get; set; }

        public int partyId { get; set; }
        public string reason { get; set; }
        public string BoothName { get; set; }
        public string BoothNumber { get; set; }
        public string Occupation { get; set; }
        public int Occupations { get; set; }
        public List<string> VillageNames { get; set; }

        public List<string> villageId { get; set; }
        public string village_Id { get; set; }
        public string villageName { get; set; }

        //using booth dashboard
        public string sectorName { get; set; }
        public string MandalName { get; set; }


    }
    public class SatisfiedList
    {
        public int id { get; set; }
        public string mandalname { get; set; }
        public string sectorname { get; set; }
        public string boothno { get; set; }
        public string PollingStationBoothName { get; set; }
        public string boothname { get; set; }
        public string village { get; set; }
        public string name { get; set; }
        public string Satisfied { get; set; }
        public string Unsatisfied { get; set; }
        public int age { get; set; }
        public string mobile { get; set; }
        public string party { get; set; }
        public string reason { get; set; }
        public List<string> villageName { get; set; }

    }
}


