using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
	public class Sector
	{
        public int Id { get; set; }
        public int Mandal_Id { get; set; }
        public string SectorNo { get; set; }
        public string PollingStationBoothName { get; set; }
        public string SectorName { get; set; }
        public int SectorInchargeName { get; set; }
        public string InchargeName { get; set; }
        public string FatherName { get; set; }
        public int? Age { get; set; }
        public string castename { get; set; }
        public string subcaste { get; set; }
        public string CasteId { get; set; }
        public string subCasteId { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public bool Status { get; set; }
        public int TotalBooth { get; set; }
        public int TotalSectors { get; set; }
        public string MandalName { get; set; }

        public string SubCasteName { get; set; }
        public string CasteCategoryName { get; set; }
        public string Username { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }

        public List<string> VillageNames { get; set; }
        public List<string> villageId { get; set; }

        public int VillageCount { get; set; }


        public int SectorId { get; set; }
        public int TotalBooths { get; set; }
        public int TotalVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVotes { get; set; }
        public int Pravasi { get; set; }
        public int SeniorCount { get; set; }
        public int DisabledCount { get; set; }
        public string Village { get; set; }


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

        //public string pasword { get; set; }
        //public string role { get; set; }
    }


    public class AllSectorVotesCountsByMandalId
    {
        public int Id { get; set; }
 
        public string SectorName { get; set; }

        public string InchargeName { get; set; }
        public string FatherName { get; set; }
        public int Age { get; set; }
        public string castename { get; set; }

        public string PhoneNumber { get; set; }


    
    }

    public class SectorDashboardCardsCount
    {
        public int SectorId { get; set; }
        public int TotalBooths { get; set; }
        public int TotalPannaPramukh { get; set; }
        public int TotalSatisfied { get; set; }
        public int TotalUnsatisfied { get; set; }
        public int TotalPravasi { get; set; }
        public int TotalNewVoter { get; set; }
        public int totalDoubleVoter { get; set; }
        public int TotalEffectivePerson { get; set; }
        public int TotalBoothSamiti { get; set; }
        public int TotalSenior { get; set; }
        public int TotalDisabled { get; set; }
        public int TotalBoothVoterDes { get; set; }
        public int TotalPost { get; set; }
        public int TotalActivity { get; set; }
 
    }
    public class SectorInchargeProfile
    {
        public int SectorId { get; set; }

        public string SectorName { get; set; }
        public string SectorInchargeName { get; set; }
        public string profileImg { get; set; }
    }


}