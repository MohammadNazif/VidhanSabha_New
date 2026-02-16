using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
	public class PartyResultViewModel
	{
        public string PartyName { get; set; }
        public int Won { get; set; }
        public decimal VotePercent { get; set; }
    }
}