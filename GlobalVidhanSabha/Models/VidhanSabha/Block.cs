using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class Block
    {
        public int Block_Id { get; set; }
        public string BlockName { get; set; }
        public string InchargeName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public int? Category { get; set; }
        public int? Caste { get; set; }
        public string CategoryName { get; set; } // for display
        public string CasteName { get; set; } // for display

        public string Occupation { get; set; }
        public string partyName { get; set; }

        public int? party { get; set; }
        public string ProfileImage { get; set; }
    }

}