using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class NewVoters
    {

        public int Id { get; set; }
        public int Booth_Id { get; set; }
        public int SubCasteId { get; set; }
        public int CasteId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string caste { get; set; }

        public DateTime? DOB { get; set; }
        public string DOBString => DOB.HasValue ? DOB.Value.ToString("yyyy-MM-dd") : "";
        public string Education { get; set; }
        public string MobileNumber { get; set; }
        //public string village { get; set; }
        public string CategoryName { get; set; }
        public string BoothNumber { get; set; }
        public string PollingStationBoothName { get; set; }
        public string SubCasteName { get; set; }



        public int BoothNo { get; set; }
        public string totalage { get; set; }

        public string VoterName { get; set; }
        public DateTime dateofbirth { get; set; }
        public string Category { get; set; }

        public string BoothIncharge { get; set; }
        public string SectorIncharge { get; set; }
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public string VillageListId { get; set; }
        public string Village{ get; set; }
        public List<string> VillageNames { get; set; }

    }
    public class NewVoterList
    {
        public int id { get; set; }
        public string MandalName { get; set; }
        public string SectorName { get; set; }
        public string BoothNo { get; set; }
        public string BoothName { get; set; }
        public string name { get; set; }
        public string fathername { get; set; }
        public string Category { get; set; }
        public string Caste { get; set; }
        public string education { get; set; }
        public string Mobile { get; set; }
        public string dob { get; set; }
        public string totalAge { get; set; }
        public string PollingStationBoothName { get; set; }

        public string Village { get; set; }
    }
}