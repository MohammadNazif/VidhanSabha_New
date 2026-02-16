using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class Login
    {
        public int Id { get; set; }

        public string Contact { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public int Status { get; set; }
        public int? VidhanSabhaId { get; set; }
    }

    public class Location
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int userId { get; set; }
    }
}