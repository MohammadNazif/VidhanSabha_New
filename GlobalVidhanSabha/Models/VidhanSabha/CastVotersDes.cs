using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class CastVotersDes
    {
        public int Id { get; set; }
        public int VoterDes { get; set;}
        public List<int> castVoterId { get; set; } = new List<int>();
        public List<int> castNameId { get; set; } = new List<int>();
        public List<int> Number { get; set; }
        public List<string> CastName { get; set; }
        //public List<string> SubCastName { get; set; } = new List<string>();

      
    }

    public class CasteList
    {
        public int Id { get; set; }
        public string cast_name { get; set; }
        public string cast_number { get; set; }
    }
}