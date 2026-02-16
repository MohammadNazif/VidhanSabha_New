using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class FilterModel
    {
        public string boothIds { get; set; }
        public string villageIds { get; set; }
        public string mandalIds { get; set; }
        public string casteIds { get; set; }
        public string desgIds { get; set; }
        public string occuIds { get; set; }
        public string sectorIds { get; set; }
    }
}