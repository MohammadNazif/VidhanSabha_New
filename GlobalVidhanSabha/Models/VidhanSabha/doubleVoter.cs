using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class doubleVoter
    {
        public int id { get; set; }
        public int boothid { get; set; }
        public int BoothNumber { get; set; }
        public string BoothName { get; set; }
        public string PollingStationBoothName { get; set; }
        public string name { get; set; }
        public string fathername { get; set; }
        public string voterno { get; set; }
        public string currAddress { get; set; }
        public string description { get; set; }
        public string pastAddress { get; set; }


        //public int Boothnumber { get; set; }
        public string currentAddress { get; set; }
        public string reason { get; set; }
        public string village { get; set; }


        public string BoothIncharge { get; set; }
        public string SectorIncharge { get; set; }
        public string SectorName { get; set; }
        public string MandalName { get; set; }
        public string VillageListId { get; set; }
        public string VillageListName { get; set; }

        public List<string> VillageNames { get; set; }
    }
    public class DoubleVoteList
    {
        public int id { get; set; }

        public string MandalName { get; set; }  
        public string SectorName { get; set; }  
        public string BoothNo { get; set; }  
        public string BoothName { get; set; }  
        public string Name { get; set; }  
        public string FatherName { get; set; }  
        public string voterid { get; set; }  
        public string currentAdd { get; set; }  
        public string preAdd { get; set; }  
        public string reason { get; set; }  
        public string PollingStationBoothName { get; set; }  

        public string Village { get; set; }

        public List<string> VillageNames { get; set; }
    }
}