using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class BDC
    {
        public int BDC_Id { get; set; }
        public int Block_Id { get; set; }
        public string BlockName { get; set; }
        public string Name { get; set; }
        public string villageId { get; set; }
        public string Village { get; set; }
        public int? Category { get; set; }
        public string CategoryName { get; set; }
        public int? Caste { get; set; }
        public string CasteName { get; set; }
        public int? Age { get; set; }

        public int party { get; set; }
        public string partyName { get; set; }
        public string Contact { get; set; }
        public string Education { get; set; }
        public string ProfileImage { get; set; } // path to image
        public int WardNumber { get; set; }
    }

}