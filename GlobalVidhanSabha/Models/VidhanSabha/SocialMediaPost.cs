using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class SocialMediaPost
    {
        public int PostId { get; set; }


        public string title { get; set; }
        public string Description { get; set; }

        public string[] Platform { get; set; }

        public string Link { get; set; } 

        public HttpPostedFileBase post  { get; set; }
        public string PostUrl { get; set; }

        public string Status { get; set; } 

        public List<int?> BoothIds { get; set; } = new List<int?>();
        public List<int?> SectorIds { get; set; } = new List<int?>();

        public int Booth_id { get; set; }
        public int Boothnumber { get; set; }
        public string boothName { get; set; }
        public string BoothIncharge { get; set; }
        public string SectorName { get; set; }
        public string SectorInchargeName { get; set; }
        public string PollingStationBoothName { get; set; }

        public DateTime CreatedAt { get; set; }


    }

    public class SocialMediaPostLink
    {
        public int PostId { get; set; }
        public string title { get; set; }
        public string Description { get; set; }

        public string[] Platform { get; set; }
        public string PostUrl { get; set; }
        public string IsPost { get; set; }
    }

    public class FacebookPostStatusModel
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Status { get; set; }  // e.g. "done"
    }

}