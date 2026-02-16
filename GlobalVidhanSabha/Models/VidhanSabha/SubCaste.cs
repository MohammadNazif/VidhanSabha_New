using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
	public class SubCaste
	{
		public int SubCaste_Id { get; set; }
		public int Caste_Id { get; set; }

		public string SubCasteName { get; set; }
		public int Status { get; set; }

    }
}