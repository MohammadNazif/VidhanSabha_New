using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
	public class Booth
	{
        public int Booth_Id { get; set; }
        public int Mandal_Id { get; set; }
        public int Sector_Id { get; set; }
        public string MandalName { get; set; }
        public string SectorName { get; set; }
        public string BoothNumber { get; set; }
        public string PollingStationBoothName { get; set; }
        public string BoothName { get; set; }
        public int CasteId { get; set; }
        public string SubCasteId { get; set; }
        public int BoothInchargeName { get; set; }
        public string InchargeName { get; set; }
        public string FatherName { get; set; }
        public string boothLocation { get; set; }
        public int? Age { get; set; }
        public string castname { get; set; }
        public string subcaste { get; set; }
        public string SubCasteName { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public string Username { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Village { get; set; }

        public List<string> VillageNames { get; set; }
        public List<string> villageId { get; set; }

        public bool Status { get; set; }
        //public string SubCasteName { get; set; }

        //--reportlist
        public int BoothId { get; set; }
        public int MandalId { get; set; }
        public int SectorId { get; set; }
        public string BoothNo { get; set; }
        //public string CastName { get; set; }
        public string Mobile { get; set; }

        public int TotalVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVotes { get; set; }
        public int Pravasi { get; set; }
        public string SectorIncName { get; set; }
        public string SectorIncPhone { get; set; }

        //public string SectorIncName { get; set; }
        //public string SectorIncPhone { get; set; }

        public string Aanshik { get; set; } = "No";

    }
    public class BoothDashboardCardsCount
    {
        public int boothId { get; set; }
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
    public class BoothInchargeProfile
    {
        public int BoothId { get; set; }
        public string BoothName { get; set; }
        public string BoothIncharge { get; set; }
        public string profileImg { get; set; }
    }

}