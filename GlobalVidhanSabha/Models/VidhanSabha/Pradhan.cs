using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
	public class Pradhan
	{
        public int Id { get; set; }

        public string Name { get; set; }
       
        public string villageId { get; set; }
      
        public string Village_Name { get; set; }

        public int? Designation { get; set; }

        public int? Gender { get; set; }
        public string GenderName { get; set; }

        public string Contact { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<string> VillageNames { get; set; }
        //public List<string> villageId { get; set; }

        public string Designationdata { get; set; }

        public int Mandal_Id { get; set; }
        public string MandalName { get; set; }
    }
}