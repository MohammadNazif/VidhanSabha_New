using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VishanSabha.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string Contact { get; set; }
        public string Role { get; set; }
    }

}