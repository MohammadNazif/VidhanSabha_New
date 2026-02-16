using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace VishanSabha.Models
{
    public class PannaPramukh
    {
        public int PannaPramukh_Id { get; set; }

        public int Mandal_Id { get; set; }
        public string MandalName { get; set; }

        public int Sector_Id { get; set; }
        public string SectorName { get; set; }
        public string PollingStationBoothName { get; set; }

        public int Booth_Id { get; set; }
        public string BoothName { get; set; }
        public string BoothNumber { get; set; }
        public string Pannapramukh { get; set; }
        public string PannaNumber { get; set; }
        public string PannaNumTo { get; set; }
        public string Cast { get; set; }
        public string Castename { get; set; }
        public string Category { get; set; }
        public string Categoryname { get; set; }
        public string VoterNumber { get; set; }
        public string village { get; set; }
      
        public string Address { get; set; }
        public string Mobile { get; set; }
        public bool Status { get; set; }
        public string SubCasteName { get; set; }
        public HttpPostedFileBase ProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }


        public string Villages { get; set; }
        public string VillageListId { get; set; }
    }
}