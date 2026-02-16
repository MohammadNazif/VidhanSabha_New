using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class AllowAccess
    {
        public int value { get; set; }

        public string Data { get; set; }
    }

    public class AllowAccessList
    {
        public int id { get; set; }

        public string type { get; set; }
        public string allowFor { get; set; }
        public string allowStatus { get; set; }
        public string sectorOrBoothId { get; set; }

        public string sectorname { get; set; }
    }
}