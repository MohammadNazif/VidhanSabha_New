using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalVidhanSabha.Models.SamithiMember
{
    public class VidhanSabhaModel
    {
        public class SamithiMemberModel
        {
            public int? Id { get; set; }
            public int? DesignationType { get; set; }
            public string SamithiMember { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }
            public int? Category { get; set; }
            public int? Caste { get; set; }
            public string Education { get; set; }
            public string Profession { get; set; }
            public string Address { get; set; }
            public string ProfilePath { get; set; }
        }

    }
}